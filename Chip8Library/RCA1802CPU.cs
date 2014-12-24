using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    public class RCA1802CPU
    {
        private short[] m_registers = new short[16];
        private byte m_dataRegister; // Accumulator
        private bool m_dataFlag; // ALU Carry

        private byte m_programCounterPointer; // P (4 bits)
        private byte m_indexRegisterPointer; // X (4 bits)

        private byte m_nInstruction; // Low-Order instruction digit (4 bits)
        private byte m_iInstruction; // High-Order instruction digit (4 bits)

        private byte m_t; // Holds old X,P after interrupt (X is high)

        private bool m_ie; // Interrupt enable flip flop
        private bool m_q; // Output flip flop

        private bool m_io1;
        private bool m_io2;
        private bool m_io3;
        private bool m_io4;
        private bool m_io5;

        private bool m_externalFlag1;
        private bool m_externalFlag2;
        private bool m_externalFlag3;
        private bool m_externalFlag4;

        private void processOpcode()
        {
            // opcode operand mnemonic name cycles programbytes
            switch (this.m_iInstruction)
            {
                case 0x0:
                    if (this.m_nInstruction == 0x0)
                    {
                        // 00  -        IDL   IDLE                                   2  1
                    }
                    else
                    {
                        // 0N  Reg N    LDN   Load via N                             2  1
                    }
                    break;
                case 0x1:
                    // 1N  Reg N    INC   Increment reg N                        2  1
                    this.m_registers[this.m_nInstruction]++;
                    break;
                case 0x2:
                    // 2N  Reg N    DEC   Decrement reg N                        2  1
                    this.m_registers[this.m_nInstruction]--;
                    break;
                case 0x3:
                    // 30  Address  BR    Short branch                           2  2
                    // 31  Address  BQ    Short branch if Q=1                    2  2
                    // 32  Address  BZ    Short branch if D=0                    2  2
                    // 33  Address  BDF   Short branch if DF=1                   2  2
                    //     Address  BPZ   Short branch if Pos or Zero            2  2
                    //     Address  BGE   Short branch if Equ or Grtr            2  2
                    // 34  Address  B1    Short branch if EF1=1                  2  2
                    // 35  Address  B2    Short branch if EF2=1                  2  2
                    // 36  Address  B3    Short branch if EF3=1                  2  2
                    // 37  Address  B4    Short branch if EF4=1                  2  2
                    // 38  Address  NBR   No short branch                        2  2
                    //     Address  SKP   Short skip                             2  1
                    // 39  Address  BNQ   Short branch if Q=0                    2  2
                    // 3A  Address  BNZ   Short branch if D NOT 0                2  2
                    // 3B  Address  BNF   Short branch if DF=0                   2  2
                    //     Address  BM    Short branch if minus                  2  2
                    //     Address  BL    Short branch if less                   2  2
                    // 3C  Address  BN1   Short branch if EF1=0                  2  2
                    // 3D  Address  BN2   Short branch if EF2=0                  2  2
                    // 3E  Address  BN3   Short branch if EF3=0                  2  2
                    // 3F  Address  BN4   Short branch if EF4=0                  2  2
                    break;
                case 0x4:
                    // 4N  Reg N    LDA   Load advance                           2  1
                    break;
                case 0x5:
                    // 5N  Reg N    STR   Store via N                            2  1
                    break;
                case 0x6:
                    switch (this.m_nInstruction)
                    {
                        case 0x0:
                            // 60  -        IRX   Increment reg X                        2  1
                            this.m_registers[this.m_indexRegisterPointer]++;
                            break;
                        // 61  Device1  OUT1  Output 1                               2  1
                        // 62  Device2  OUT3  Output 2                               2  1
                        // 63  Device3  OUT3  Output 3                               2  1
                        // 64  Device4  OUT4  Output 4                               2  1
                        // 65  Device5  OUT5  Output 5                               2  1
                        // 66  Device6  OUT6  Output 6                               2  1
                        // 67  Device7  OUT7  Output 7                               2  1
                        // 68                 Do not use
                        // 69  Device1  INP1  Input 1                                2  1
                        // 6A  Device2  INP2  Input 2                                2  1
                        // 6B  Device3  INP3  Input 3                                2  1
                        // 6C  Device4  INP4  Input 4                                2  1
                        // 6D  Device5  INP5  Input 5                                2  1
                        // 6E  Device6  INP6  Input 6                                2  1
                        // 6F  Device7  INP7  Input 7                                2  1
                    }
                    break;
                case 0x7:
                    // 70  -        RET   Return                                 2  1
                    // 71  -        DIS   Disable                                2  1
                    // 72  -        LDXA  Load via X advance                     2  1
                    // 73  -        STXD  Store via X and decrement              2  1
                    // 74  -        ADC   Add with carry                         2  1
                    // 75  -        SDB   Subtract D with borrow                 2  1
                    // 76  -        SHRC  Shift right with carry                 2  1
                    //     -        RSHR  Ring shift right                       2  1
                    // 77  -        SMB   Subtract memory with borrow            2  1
                    // 78  -        SAV   Save                                   2  1
                    // 79  -        MARK  Push X P to stack                      2  1
                    // 7A  -        REQ   Reset Q                                2  1
                    // 7B  -        SEQ   Set Q                                  2  1
                    // 7C  Data     ADCI  Add with carry immediate               2  2
                    // 7D  Data     SDBI  Subtract D with borrow immediate       2  2
                    // 7E  -        SHLC  Shift left with carry                  2  1
                    //     -        RSHL  Ring shift left                        2  1
                    // 7F  Data     SMBI  Subtract memory with borrow immediate  2  2
                    break;
                case 0x8:
                    // 8N  Reg N    GLO   Get low reg N                          2  1
                    this.m_dataRegister = (byte)(0x0F & this.m_registers[this.m_nInstruction]);
                    break;
                case 0x9:
                    // 9N  Reg N    GHI   Get high reg N                         2  1
                    this.m_dataRegister = (byte)(0xF0 & this.m_registers[this.m_nInstruction]);
                    break;
                case 0xA:
                    // AN  Reg N    PLO   Put low reg N                          2  1
                    this.m_registers[this.m_nInstruction] = (short)(this.m_registers[this.m_nInstruction] & (0xFF00 | this.m_dataRegister));
                    break;
                case 0xB:
                    // BN  Reg N    PHI   Put high reg N                         2  1
                    this.m_registers[this.m_nInstruction] = (short)(this.m_registers[this.m_nInstruction] & (0x00FF | (this.m_dataRegister << 8)));
                    break;
                case 0xC:
                    // C0  Address  LBR   Long branch                            3  3
                    // C1  Address  LBQ   Long branch if Q=1                     3  3
                    // C2  Address  LBZ   Long branch if D=0                     3  3
                    // C3  Address  LBDF  Long branch if DF=1                    3  3
                    // C4  -        NOP   No operation                           3  1
                    // C5  -        LSNQ  Long skip if Q=0                       3  1
                    // C6  -        LSNZ  Long skip if D NOT 0                   3  1
                    // C7  -        LSNF  Long skip if DF=0                      3  1
                    // C8  -        LSKP  Long skip                              3  1
                    //     Address  NLBR  No long branch                         3  3
                    // C9  Address  LBNQ  Long branch if Q=0                     3  3
                    // CA  Address  LBNZ  Long branch if D NOT 0                 3  3
                    // CB  Address  LBNF  Long branch if DF=0                    3  3
                    // CC  -        LSIE  Long skip if IE=1                      3  1
                    // CD  -        LSQ   Long skip if Q=1                       3  1
                    // CE  -        LSZ   Long skip if D=0                       3  1
                    // CF  -        LSDF  Long skip if DF=1                      3  1
                    break;
                case 0xD:
                    // DN  Reg N    SEP   Set P                                  2  1
                    break;
                case 0xE:
                    // EN  Reg N    SEX   Set X                                  2  1
                    break;
                case 0xF:
                    // F0  -        LDX   Load via X                             2  1
                    // F1  -        OR    Or                                     2  1
                    // F2  -        AND   And                                    2  1
                    // F3  -        XOR   Exclusive or                           2  1
                    // F4  -        ADD   Add                                    2  1
                    // F5  -        SD    Subetract D                            2  1
                    // F6  -        SHR   Shift right                            2  1
                    // F7  -        SM    Subtract memory                        2  1
                    // F8  Data     LDI   Load immediate                         2  2
                    // F9  Data     ORI   Or immediate                           2  2
                    // FA  Data     ANI   And immediate                          2  2
                    // FB  Data     XRI   Exclusive or immediate                 2  2
                    // FC  Data     ADI   Add immediate                          2  2
                    // FD  Data     SDI   Subtract D immediate                   2  2
                    // FE  -        SHL   Shift left                             2  1
                    // FF  Data     SMI   Subtract memory immediate              2  2
                    break;
            }
        }
    }
}

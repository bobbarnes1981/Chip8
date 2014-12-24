using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    /// <summary>
    /// This class represents the chip 8 CPU
    /// </summary>
    public class Chip8CPU
    {
        /// <summary>
        /// System bus
        /// </summary>
        private Bus m_bus;

        /// <summary>
        /// Operation code register
        /// </summary>
        private ushort m_opcodeRegister;

        /// <summary>
        /// Register array
        /// </summary>
        private byte[] m_registers;

        /// <summary>
        /// Index register
        /// </summary>
        private ushort m_indexRegister;

        /// <summary>
        /// Program counter
        /// </summary>
        private ushort m_programCounter;

        /// <summary>
        /// Delay timer
        /// </summary>
        private byte m_delayTimer;

        /// <summary>
        /// Sound timer
        /// </summary>
        private byte m_soundTimer;

        /// <summary>
        /// Memory stack
        /// </summary>
        private ushort[] m_stack;

        /// <summary>
        /// Memory stack pointer
        /// </summary>
        private ushort m_stackPointer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Chip8Cpu" /> class.
        /// </summary>
        /// <param name="systemMemory">System memory</param>
        /// <param name="videoMemory">Video memory</param>
        /// <param name="keypad">Keypad interface</param>
        public Chip8CPU(Bus bus)
        {
            m_ticks = 0;
            m_bus = bus;
            m_opcodeRegister = 0x00;
            m_registers = new byte[16];
            m_indexRegister = 0x00;
            m_programCounter = 0x200;
            m_stack = new ushort[16];
            m_stackPointer = 0x00;
        }

        /// <summary>
        /// Sound timer zero flag
        /// </summary>
        public bool SoundTimerZero { get; private set; }

        /// <summary>
        /// Display ready flag
        /// </summary>
        public bool DisplayReady { get; private set; }

        /// <summary>
        /// Number of cycles
        /// </summary>
        private long m_ticks;

        /// <summary>
        /// Run a cycle of the CPU
        /// </summary>
        public void Step()
        {
            // fetch opcode
            byte hi = m_bus.MemoryRead(m_programCounter);
            m_programCounter++;
            byte lo = m_bus.MemoryRead(m_programCounter);
            m_programCounter++;
            m_opcodeRegister = (ushort)(hi << 8 | lo);

            // process opcode
            processOpcode();

            if (DateTime.Now.Ticks - m_ticks > 160000) // 16ms elapsed
            {
                m_ticks = DateTime.Now.Ticks;
                // update timers
                if (m_delayTimer > 0)
                {
                    m_delayTimer--;
                }

                if (m_soundTimer > 0)
                {
                    m_soundTimer--;
                    if (m_soundTimer == 0)
                    {
                        SoundTimerZero = true;
                    }
                }
            }
        }

        /// <summary>
        /// Process an operation code
        /// </summary>
        /// <param name="opcode">Operation code</param>
        private void processOpcode()
        {
            switch (m_opcodeRegister & 0xF000)
            {
                case 0x0000:
                    decode0000();
                    break;
                case 0x1000:
                    decode1000();
                    break;
                case 0x2000:
                    decode2000();
                    break;
                case 0x3000:
                    decode3000();
                    break;
                case 0x4000:
                    decode4000();
                    break;
                case 0x5000:
                    decode5000();
                    break;
                case 0x6000:
                    decode6000();
                    break;
                case 0x7000:
                    decode7000();
                    break;
                case 0x8000:
                    decode8000();
                    break;
                case 0x9000:
                    decode9000();
                    break;
                case 0xA000:
                    decodeA000();
                    break;
                case 0xB000:
                    decodeB000();
                    break;
                case 0xC000:
                    decodeC000();
                    break;
                case 0xD000:
                    decodeD000();
                    break;
                case 0xE000:
                    decodeE000();
                    break;
                case 0xF000:
                    decodeF000();
                    break;
                default:
                    throw new Exception("Unhandled opcode [" + m_opcodeRegister + "]");
            }
        }

        private void decode0000()
        {
            switch (m_opcodeRegister)
            {
                case 0x00E0:
                    // 00E0  Clears the screen.
                    m_bus.DisplayClear();
                    DisplayReady = true;
                    break;
                case 0x00EE:
                    // 00EE  Returns from a subroutine.
                    m_stackPointer = (byte)(m_stackPointer - 1);
                    m_programCounter = m_stack[m_stackPointer];
                    break;
                default:
                    // 0NNN  Calls RCA 1802 program at address NNN.
                    throw new NotImplementedException();
                    break;
            }
        }

        private byte getNN()
        {
            return (byte)(m_opcodeRegister & 0x00FF);
        }

        private ushort getNNN()
        {
            return (ushort)(0x0FFF & m_opcodeRegister);
        }

        private int getVX()
        {
            return (m_opcodeRegister & 0x0F00) >> 8;
        }

        private int getVY()
        {
            return (m_opcodeRegister & 0x00F0) >> 4;
        }

        private void decode1000()
        {
            // 1NNN  Jumps to address NNN.
            m_programCounter = getNNN();
        }

        private void decode2000()
        {
            // 2NNN  Calls subroutine at NNN.
            m_stack[m_stackPointer] = m_programCounter;
            m_stackPointer = (byte)(m_stackPointer + 1);
            m_programCounter = getNNN();
        }

        private void decode3000()
        {
            // 3XNN  Skips the next instruction if VX equals NN.
            if (m_registers[getVX()] == getNN())
            {
                m_programCounter += 2;
            }
        }

        private void decode4000()
        {
            // 4XNN  Skips the next instruction if VX doesn't equal NN.
            if (m_registers[getVX()] != getNN())
            {
                m_programCounter += 2;
            }
        }

        private void decode5000()
        {
            // 5XY0  Skips the next instruction if VX equals VY.
            if (m_registers[getVX()] == m_registers[getVY()])
            {
                m_programCounter += 2;
            }
        }

        private void decode6000()
        {
            // 6XNN  Sets VX to NN.
            m_registers[getVX()] = getNN();
        }

        private void decode7000()
        {
            // 7XNN  Adds NN to VX.
            m_registers[getVX()] = (byte)(m_registers[getVX()] + getNN());
        }

        private void decode8000()
        {
            int regX = getVX();
            int regY = getVY();
            switch (m_opcodeRegister & 0x000F)
            {
                case 0x0:
                    // 8XY0  Sets VX to the value of VY.
                    m_registers[regX] = m_registers[regY];
                    break;
                case 0x1:
                    // 8XY1  Sets VX to VX or VY.
                    m_registers[regX] = (byte)(m_registers[regX] | m_registers[regY]);
                    break;
                case 0x2:
                    // 8XY2  Sets VX to VX and VY.
                    m_registers[regX] = (byte)(m_registers[regX] & m_registers[regY]);
                    break;
                case 0x3:
                    // 8XY3  Sets VX to VX xor VY.
                    m_registers[regX] = (byte)(m_registers[regX] ^ m_registers[regY]);
                    break;
                case 0x4:
                    // 8XY4  Adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't.
                    {
                        int result = m_registers[regX] + m_registers[regY];
                        byte carry;
                        if (result > 0xFF)
                        {
                            carry = 0x01;
                        }
                        else
                        {
                            carry = 0x00;
                        }

                        m_registers[0xF] = carry;
                        m_registers[regX] = (byte)(result & 0xFF);
                    }

                    break;
                case 0x5:
                    // 8XY5  VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there
                    //       isn't.
                    {
                        int result = m_registers[regX] - m_registers[regY];
                        byte borrow;
                        if (result < 0x00)
                        {
                            borrow = 0x00;
                        }
                        else
                        {
                            borrow = 0x01;
                        }

                        m_registers[0xF] = borrow;
                        m_registers[regX] = (byte)(result & 0xFF);
                    }

                    break;
                case 0x6:
                    // 8XY6  Shifts VX right by one. VF is set to the value of the least significant bit of 
                    //       VX before the shift.[2]
                    m_registers[0xF] = (byte)(m_registers[regX] & 0x01);
                    m_registers[regX] = (byte)(m_registers[regX] >> 1);
                    break;
                case 0x7:
                    // 8XY7  Sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there 
                    //       isn't.
                    {
                        int result = m_registers[regY] - m_registers[regX];
                        byte carry;
                        if (result < 0x00)
                        {
                            carry = 0x00;
                        }
                        else
                        {
                            carry = 0x01;
                        }

                        m_registers[0xF] = carry;
                        m_registers[regX] = (byte)(result & 0xFF);
                    }

                    break;
                case 0xE:
                    // 8XYE  Shifts VX left by one. VF is set to the value of the most significant bit of VX 
                    //       before the shift.[2]
                    m_registers[0xF] = (byte)(m_registers[regX] & 0x80);
                    m_registers[regX] = (byte)(m_registers[regX] << 1);
                    break;
                default:
                    throw new Exception("Unhandled opcode [" + m_opcodeRegister + "]");
            }
        }

        private void decode9000()
        {
            // 9XY0  Skips the next instruction if VX doesn't equal VY.
            if (m_registers[getVX()] != m_registers[getVY()])
            {
                m_programCounter += 2;
            }
        }

        private void decodeA000()
        {
            // ANNN  Sets I to the address NNN.
            m_indexRegister = getNNN();
        }

        private void decodeB000()
        {
            // BNNN  Jumps to the address NNN plus V0.
            int addr = getNNN();
            addr += m_registers[0];
            m_programCounter = (ushort)addr;
        }

        private void decodeC000()
        {
            // CXNN  Sets VX to a random number and NN.
            byte random = (byte)new Random().Next(0xFF);
            m_registers[getVX()] = (byte)(random & getNN());
        }

        private void decodeD000()
        {
            // DXYN  Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels.
            //       Each row of 8 pixels is read as bit-coded (with the most significant bit of each byte 
            //       displayed on the left) starting from memory location I; I value doesn't change after the 
            //       execution of this instruction. As described above, VF is set to 1 if any screen pixels are 
            //       flipped from set to unset when the sprite is drawn, and to 0 if that doesn't happen.
            int regX = getVX();
            int regY = getVY();
            int orgX = m_registers[regX];
            int orgY = m_registers[regY];
            int height = m_opcodeRegister & 0x000F;
            ushort sysMemLoc = m_indexRegister;
            m_registers[0xF] = 0x00;
            for (int offsetY = 0; offsetY < height; offsetY++)
            {
                byte rowData = m_bus.MemoryRead(sysMemLoc);
                for (int offsetX = 0; offsetX < 8; offsetX++)
                {
                    int realX = orgX + offsetX;
                    int realY = orgY + offsetY;
                    bool oldBit = m_bus.DisplayRead(realX, realY);
                    bool newBit = ((rowData >> (7 - offsetX)) & 0x0001) == 1;
                    if (oldBit != newBit)
                    {
                        m_bus.DisplayWrite(realX, realY, true);
                    }
                    else
                    {
                        if (oldBit == true)
                        {
                            m_registers[0xF] = 0x01;
                        }

                        m_bus.DisplayWrite(realX, realY, false);
                    }
                }

                sysMemLoc++;
            }

            DisplayReady = true;
        }

        private void decodeE000()
        {
            int regX = getVX();
            switch (m_opcodeRegister & 0xF0FF)
            {
                case 0xE09E:
                    // EX9E  Skips the next instruction if the key stored in VX is pressed.
                    if (m_bus.KeyPadRead(m_registers[regX]))
                    {
                        m_programCounter += 2;
                    }

                    break;
                case 0xE0A1:
                    // EXA1  Skips the next instruction if the key stored in VX isn't pressed.
                    if (!m_bus.KeyPadRead(m_registers[regX]))
                    {
                        m_programCounter += 2;
                    }

                    break;
            }
        }

        private void decodeF000()
        {
            int regX = getVX();
            switch (m_opcodeRegister & 0x00FF)
            {
                case 0x07:
                    // FX07  Sets VX to the value of the delay timer.
                    m_registers[regX] = m_delayTimer;
                    break;
                case 0x0A:
                    // FX0A  A key press is awaited, and then stored in VX.
                    int reg = getVX();
                    int key = m_bus.KeyPadWait();
                    m_registers[reg] = (byte)key;
                    break;
                case 0x15:
                    // FX15  Sets the delay timer to VX.
                    m_delayTimer = m_registers[regX];
                    break;
                case 0x18:
                    // FX18  Sets the sound timer to VX.
                    m_soundTimer = m_registers[regX];
                    break;
                case 0x1E:
                    // FX1E  Adds VX to I.[3]
                    m_indexRegister = (ushort)(m_indexRegister + m_registers[regX]);
                    break;
                case 0x29:
                    {
                        // FX29  Sets I to the location of the sprite for the character in VX. Characters 0-F 
                        //       (in hexadecimal) are represented by a 4x5 font.
                        m_indexRegister = (byte)(m_registers[regX] * 5);
                    }

                    break;
                case 0x33:
                    // FX33  Stores the Binary-coded decimal representation of VX, with the most significant 
                    //       of three digits at the address in I, the middle digit at I plus 1, and the least
                    //       significant digit at I plus 2. (In other words, take the decimal representation 
                    //       of VX, place the hundreds digit in memory at location in I, the tens digit at
                    //       location I+1, and the ones digit at location I+2.)
                    int number = m_registers[regX];
                    int hundreds = number / 100;
                    m_bus.MemoryWrite((ushort)(m_indexRegister + 0), (byte)hundreds);
                    int tens = (number - (hundreds * 100)) / 10;
                    m_bus.MemoryWrite((ushort)(m_indexRegister + 1), (byte)tens);
                    int units = (number - (hundreds * 100)) - (tens * 10);
                    m_bus.MemoryWrite((ushort)(m_indexRegister + 2), (byte)units);
                    break;
                case 0x55:
                    // FX55  Stores V0 to VX in memory starting at address I.[4]
                    {
                        int memLoc = m_indexRegister;

                        // use <= because V0 to Vx inclusive
                        for (int i = 0; i <= regX; i++)
                        {
                            m_bus.MemoryWrite((ushort)(memLoc + i), m_registers[i]);
                        }

                        m_indexRegister = (byte)(m_indexRegister + regX + 1);
                    }

                    break;
                case 0x65:
                    // FX65  Fills V0 to VX with values from memory starting at address I.[4]
                    {
                        ushort memLoc = m_indexRegister;

                        // use <= because V0 to Vx inclusive
                        for (int i = 0; i <= regX; i++)
                        {
                            m_registers[i] = m_bus.MemoryRead((ushort)(memLoc + i));
                        }

                        m_indexRegister = (byte)(m_indexRegister + regX + 1);
                    }

                    break;
                default:
                    throw new Exception("Unhandled opcode [" + m_opcodeRegister + "]");
            }
        }
    }
}

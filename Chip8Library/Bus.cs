using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    public class Bus
    {
        private Storage m_memory;

        private Video m_display;

        public Bus(Storage memory, Video display)
        {
            m_memory = memory;
            m_display = display;
        }

        public byte MemoryRead(ushort address)
        {
            return m_memory.Read(address);
        }

        public void MemoryWrite(ushort address, byte data)
        {
            m_memory.Write(address, data);
        }

        public bool DisplayRead(int x, int y)
        {
            return m_display.Read(x, y);
        }

        public void DisplayWrite(int x, int y, bool data)
        {
            m_display.Write(x, y, data);
        }

        public void DisplayClear()
        {
            m_display.Clear();
        }

        public bool KeyPadRead(byte b)
        {
            throw new NotImplementedException();
        }

        public byte KeyPadWait()
        {
            throw new NotImplementedException();
        }
    }
}

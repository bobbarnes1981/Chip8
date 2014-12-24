using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    /// <summary>
    /// This class represents the chip 8 computer
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// System is running
        /// </summary>
        private bool m_running;

        /// <summary>
        /// Keypad
        /// </summary>
        private KeyPad m_keyPad;

        /// <summary>
        /// System memory
        /// </summary>
        private Storage m_memory;

        /// <summary>
        /// Video memory
        /// </summary>
        private Video m_video;

        /// <summary>
        /// System cpu
        /// </summary>
        private RCA1802CPU m_cpu;

        /// <summary>
        /// System bus
        /// </summary>
        private Bus m_bus;

        /// <summary>
        /// Virtual cpu
        /// </summary>
        private Chip8CPU m_vm;

        /// <summary>
        /// Video memory
        /// </summary>
        public Video Video { get { return m_video; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer" /> class.
        /// </summary>
        public Computer()
        {
            m_keyPad = new KeyPad();
            m_memory = new Storage(4096);
            m_video = new Video(64, 32);

            m_bus = new Bus(m_keyPad, m_memory, m_video);

            m_vm = new Chip8CPU(m_bus);
        }

        /// <summary>
        /// Set the state of a key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        public void SetKey(byte key, bool state)
        {
            m_keyPad.Set(key, state);
        }

        /// <summary>
        /// Load the provided byte array in to memory starting from the specified start address.
        /// </summary>
        /// <param name="startAddress">Address to start loading data to</param>
        /// <param name="data">Data to load in to memory</param>
        public void Load(int startAddress, byte[] data)
        {
            for (int offset = 0; startAddress + offset < m_memory.Size && offset < data.Length; offset++)
            {
                m_memory.Write(startAddress + offset, data[offset]);
            }
        }

        private long m_ticks;

        /// <summary>
        /// Run the computer
        /// </summary>
        public void Run()
        {
            m_running = true;
            
            do
            {
                if (DateTime.Now.Ticks - m_ticks > 1953) // 512kHz
                {
                    m_ticks = DateTime.Now.Ticks;
                    m_vm.Step();
                    if (m_vm.SoundTimerZero)
                    {
                        // make beep
                    }
                    if (m_vm.DisplayReady)
                    {
                        // update display
                    }
                }
            } while (m_running);
        }

        /// <summary>
        /// Stop the computer
        /// </summary>
        public void Stop()
        {
            m_running = false;
        }
    }
}

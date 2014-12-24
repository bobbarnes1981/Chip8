using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    /// <summary>
    /// This class represents the chip 8 keypad
    /// </summary>
    public class KeyPad
    {
        /// <summary>
        /// Keypad state
        /// </summary>
        private bool[] state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Keypad" /> class.
        /// </summary>
        public KeyPad()
        {
            this.state = new bool[16];
        }

        /// <summary>
        /// Get the key status
        /// </summary>
        /// <param name="key">Key ID</param>
        /// <returns>Key status</returns>
        public bool Get(byte key)
        {
            return this.state[key];
        }

        /// <summary>
        /// Set the key
        /// </summary>
        /// <param name="key">Key ID</param>
        /// <param name="state">key pressed state</param>
        public void Set(byte key, bool state)
        {
            this.state[key] = state;
        }

        /// <summary>
        /// Wait for a key to be pressed
        /// </summary>
        /// <returns>Key ID</returns>
        public int WaitKey()
        {
            throw new NotImplementedException();
        }
    }
}

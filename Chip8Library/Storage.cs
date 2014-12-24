using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    /// <summary>
    /// This class represents chip 8 data storage
    /// </summary>
    public class Storage : Memory<byte>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Storage" /> class.
        /// </summary>
        /// <param name="size">Memory size</param>
        public Storage(int size)
            : base(size)
        {
        }

        /// <summary>
        /// Clear the memory
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i < this.Size; i++)
            {
                this.Write(i, 0x00);
            }
        }
    }
}

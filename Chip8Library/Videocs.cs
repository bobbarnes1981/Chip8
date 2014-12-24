using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    /// <summary>
    /// This class represents chip 8 video memory
    /// </summary>
    public class Video : Memory<bool>
    {
        /// <summary>
        /// Video width in pixels
        /// </summary>
        private int width;

        /// <summary>
        /// Video height in pixels
        /// </summary>
        private int height;

        /// <summary>
        /// Initializes a new instance of the <see cref="Video" /> class.
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        public Video(int width, int height)
            : base(width * height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets the video width in pixels
        /// </summary>
        public int Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// Gets the video height in pixels
        /// </summary>
        public int Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Read a video memory location
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Pixel state</returns>
        public bool Read(int x, int y)
        {
            return this.Read(this.CalculateMemoryLocation(x, y));
        }

        /// <summary>
        /// Write a video memory location
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="data">Pixel state</param>
        public void Write(int x, int y, bool data)
        {
            this.Write(this.CalculateMemoryLocation(x, y), data);
        }

        /// <summary>
        /// Clear the video memory
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i < this.Size; i++)
            {
                this.Write(i, false);
            }
        }

        /// <summary>
        /// Calculate the video memory location from coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Memory address</returns>
        private int CalculateMemoryLocation(int x, int y)
        {
            // wrap values
            if (x >= this.width)
            {
                x = x % this.width;
            }

            if (y >= this.height)
            {
                y = y % this.height;
            }

            return x + (this.width * y);
        }
    }
}

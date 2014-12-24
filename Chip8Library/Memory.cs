using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8Library
{
    /// <summary>
    /// This class represents memory
    /// </summary>
    /// <typeparam name="T">Stored data type</typeparam>
    public abstract class Memory<T>
    {
        /// <summary>
        /// Memory array
        /// </summary>
        private T[] m_memory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory{T}" /> class.
        /// </summary>
        /// <param name="size">Size of memory</param>
        public Memory(int size)
        {
            m_memory = new T[size];
        }

        /// <summary>
        /// Gets the size of the memory
        /// </summary>
        public int Size
        {
            get
            {
                return m_memory.Length;
            }
        }

        /// <summary>
        /// Read a memory location
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <returns>Read data</returns>
        public T Read(int address)
        {
            return m_memory[address];
        }

        /// <summary>
        /// Write a memory location
        /// </summary>
        /// <param name="address">Memory address</param>
        /// <param name="data">Written data</param>
        public void Write(int address, T data)
        {
            m_memory[address] = data;
        }

        /// <summary>
        /// Get an enumerator
        /// </summary>
        /// <returns>Enumerator for memory</returns>
        public IEnumerator GetEnumerator()
        {
            return m_memory.GetEnumerator();
        }

        /// <summary>
        /// Clear the memory
        /// </summary>
        public abstract void Clear();
    }
}

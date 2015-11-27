using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Mathematics
{
    public partial struct FieldElement
    {
        #region constructors

        /// <summary>
        /// Creates a new FieldElement struct with the specified entries.
        /// </summary>
        /// <param name="entries">The entries of the Field element.</param>
        public FieldElement(params int[] entries)
        {
            if (entries == null)
                throw new ArgumentNullException("entries");

            if (entries.Length != 10)
                throw new ArgumentOutOfRangeException("entries", entries.Length, "entries length must be 10");

            X0 = entries[0];
            X1 = entries[1];
            X2 = entries[2];
            X3 = entries[3];
            X4 = entries[4];
            X5 = entries[5];
            X6 = entries[6];
            X7 = entries[7];
            X8 = entries[8];
            X9 = entries[9];
        }

        #endregion

        #region static methods for construction

        /// <summary>
        /// Creates a <see cref="FieldElement"/> struct from a byte array
        /// at the specified offset.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <param name="offset">The offset</param>
        /// <returns>A fieldset struct.</returns>
        public static FieldElement FromData(byte[] data, int offset)
        {
            long h0 = Load4(data, offset);
            long h1 = Load3(data, offset + 4) << 6;
            long h2 = Load3(data, offset + 7) << 5;
            long h3 = Load3(data, offset + 10) << 3;
            long h4 = Load3(data, offset + 13) << 2;
            long h5 = Load4(data, offset + 16);
            long h6 = Load3(data, offset + 20) << 7;
            long h7 = Load3(data, offset + 23) << 5;
            long h8 = Load3(data, offset + 26) << 4;
            long h9 = (Load3(data, offset + 29) & 8388607) << 2;
            return Combine(h0, h1, h2, h3, h4, h5, h6, h7, h8, h9);
        }

        /// <summary>
        /// Combines a <see cref="FieldElement"/> struct from the
        /// specified entries.
        /// </summary>
        /// <remarks>
        ///     Reduces to Radix 2**51 as well.
        /// </remarks>
        /// <param name="h0">entry 0</param>
        /// <param name="h1">entry 1</param>
        /// <param name="h2">entry 2</param>
        /// <param name="h3">entry 3</param>
        /// <param name="h4">entry 4</param>
        /// <param name="h5">entry 5</param>
        /// <param name="h6">entry 6</param>
        /// <param name="h7">entry 7</param>
        /// <param name="h8">entry 8</param>
        /// <param name="h9">entry 9</param>
        /// <returns>A FieldElement struct.</returns>
        private static FieldElement Combine(long h0, long h1, long h2, long h3, long h4, long h5, long h6, long h7, long h8, long h9)
        {
            long carry;
            
            carry = (h9 + (1 << 24)) >> 25; h0 += carry * 19; h9 -= carry << 25;
            carry = (h1 + (1 << 24)) >> 25; h2 += carry; h1 -= carry << 25;
            carry = (h3 + (1 << 24)) >> 25; h4 += carry; h3 -= carry << 25;
            carry = (h5 + (1 << 24)) >> 25; h6 += carry; h5 -= carry << 25;
            carry = (h7 + (1 << 24)) >> 25; h8 += carry; h7 -= carry << 25;

            carry = (h0 + (1 << 25)) >> 26; h1 += carry; h0 -= carry << 26;
            carry = (h2 + (1 << 25)) >> 26; h3 += carry; h2 -= carry << 26;
            carry = (h4 + (1 << 25)) >> 26; h5 += carry; h4 -= carry << 26;
            carry = (h6 + (1 << 25)) >> 26; h7 += carry; h6 -= carry << 26;
            carry = (h8 + (1 << 25)) >> 26; h9 += carry; h8 -= carry << 26;

            return new FieldElement()
            {
                X0 = (int)h0,
                X1 = (int)h1,
                X2 = (int)h2,
                X3 = (int)h3,
                X4 = (int)h4,
                X5 = (int)h5,
                X6 = (int)h6,
                X7 = (int)h7,
                X8 = (int)h8,
                X9 = (int)h9,
            };
        }

        /// <summary>
        /// Loads three bytes into an Int64.
        /// </summary>
        /// <remarks>data is in Little Endian.</remarks>
        /// <param name="data">The data to load.</param>
        /// <param name="offset">The offset to load the data from.</param>
        /// <returns></returns>
        private static long Load3(byte[] data, int offset)
        {
            uint result;
            result = data[offset + 0];
            result |= (uint)data[offset + 1] << 8;
            result |= (uint)data[offset + 2] << 16;
            return result;
        }

        /// <summary>
        /// Loads four bytes into an Int64.
        /// </summary>
        /// <remarks>data is in Little Endian.</remarks>
        /// <param name="data">The data to load.</param>
        /// <param name="offset">The offset to load the data from.</param>
        /// <returns></returns>
        private static long Load4(byte[] data, int offset)
        {
            uint result;
            result = data[offset + 0];
            result |= (uint)data[offset + 1] << 8;
            result |= (uint)data[offset + 2] << 16;
            result |= (uint)data[offset + 3] << 24;
            return result;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Mathematics
{
    public partial struct FieldElement
    {

        #region Operations

        /// <summary>
        /// Replaces the current value of the field element.
        /// Replaces source with alternate if b == 1;
        /// Returns source if b == 0
        /// </summary>
        /// <remarks>
        /// Functions in constant time regardless of b.
        /// </remarks>
        /// <param name="alternate">The alternate field element</param>
        /// <param name="replace">The choice</param>
        /// <returns></returns>
        public FieldElement Replace(FieldElement alternate, int replace)
        {
            replace = -replace;

            return new FieldElement
            {
                X0 = this.X0 ^ replace & (this.X0 ^ alternate.X0),
                X1 = this.X1 ^ replace & (this.X1 ^ alternate.X1),
                X2 = this.X2 ^ replace & (this.X2 ^ alternate.X2),
                X3 = this.X3 ^ replace & (this.X3 ^ alternate.X3),
                X4 = this.X4 ^ replace & (this.X4 ^ alternate.X4),
                X5 = this.X5 ^ replace & (this.X5 ^ alternate.X5),
                X6 = this.X6 ^ replace & (this.X6 ^ alternate.X6),
                X7 = this.X7 ^ replace & (this.X7 ^ alternate.X7),
                X8 = this.X8 ^ replace & (this.X8 ^ alternate.X8),
                X9 = this.X9 ^ replace & (this.X9 ^ alternate.X9)
            };
        }

        /// <summary>
        /// Swaps the current value of the field element.
        /// Swaps source with other if b == 1;
        /// Returns source if b == 0 and leaves other alone.
        /// </summary>
        /// <remarks>
        /// Functions in constant time regardless of b.
        /// </remarks>
        /// <param name="other">The other field element</param>
        /// <param name="swap">The choice</param>
        /// <returns></returns>
        public FieldElement Swap(ref FieldElement other, int swap)
        {
            var result = this.Replace(other, swap);
            other = other.Replace(this, swap);
            return result;
        }

        /// <summary>
        /// Reduces a FieldElement to it's Radix 2**51 representation.
        /// </summary>
        /// <returns>A Field Element</returns>
        public FieldElement Reduce()
        {
            int carry;
            int q = (19 * X9 + (1 << 24)) >> 25;

            q = (X0 + q) >> 26;
            q = (X1 + q) >> 25;
            q = (X2 + q) >> 26;
            q = (X3 + q) >> 25;
            q = (X4 + q) >> 26;
            q = (X5 + q) >> 25;
            q = (X6 + q) >> 26;
            q = (X7 + q) >> 25;
            q = (X8 + q) >> 26;
            q = (X9 + q) >> 25;

            var result = new FieldElement
            {
                X0 = X0,
                X1 = X1,
                X2 = X2,
                X3 = X3,
                X4 = X4,
                X5 = X5,
                X6 = X6,
                X7 = X7,
                X8 = X8,
                X9 = X9
            };

            result.X0 += 19 * q;

            carry = result.X0 >> 26; result.X1 += carry; result.X0 -= carry << 26;
            carry = result.X1 >> 25; result.X2 += carry; result.X1 -= carry << 25;
            carry = result.X2 >> 26; result.X3 += carry; result.X2 -= carry << 26;
            carry = result.X3 >> 25; result.X4 += carry; result.X3 -= carry << 25;
            carry = result.X4 >> 26; result.X5 += carry; result.X4 -= carry << 26;
            carry = result.X5 >> 25; result.X6 += carry; result.X5 -= carry << 25;
            carry = result.X6 >> 26; result.X7 += carry; result.X6 -= carry << 26;
            carry = result.X7 >> 25; result.X8 += carry; result.X7 -= carry << 25;
            carry = result.X8 >> 26; result.X9 += carry; result.X8 -= carry << 26;
            carry = result.X9 >> 25; result.X9 -= carry << 25;

            return result;
        }

        /// <summary>
        /// Copies the FieldElement to an array.
        /// </summary>
        /// <param name="data">The array to copy to</param>
        /// <param name="offset">The offset to start at.</param>
        public void CopyTo(byte[] data, int offset)
        {
            var h = Reduce();
            unchecked
            {
                data[offset + 0] = (byte)(h.X0 >> 0);
                data[offset + 1] = (byte)(h.X0 >> 8);
                data[offset + 2] = (byte)(h.X0 >> 16);
                data[offset + 3] = (byte)((h.X0 >> 24) | (h.X1 << 2));

                data[offset + 4] = (byte)(h.X1 >> 6);
                data[offset + 5] = (byte)(h.X1 >> 14);
                data[offset + 6] = (byte)((h.X1 >> 22) | (h.X2 << 3));

                data[offset + 7] = (byte)(h.X2 >> 5);
                data[offset + 8] = (byte)(h.X2 >> 13);
                data[offset + 9] = (byte)((h.X2 >> 21) | (h.X3 << 5));

                data[offset + 10] = (byte)(h.X3 >> 3);
                data[offset + 11] = (byte)(h.X3 >> 11);
                data[offset + 12] = (byte)((h.X3 >> 19) | (h.X4 << 6));

                data[offset + 13] = (byte)(h.X4 >> 2);
                data[offset + 14] = (byte)(h.X4 >> 10);
                data[offset + 15] = (byte)(h.X4 >> 18);
                data[offset + 16] = (byte)(h.X5 >> 0);

                data[offset + 17] = (byte)(h.X5 >> 8);
                data[offset + 18] = (byte)(h.X5 >> 16);
                data[offset + 19] = (byte)((h.X5 >> 24) | (h.X6 << 1));

                data[offset + 20] = (byte)(h.X6 >> 7);
                data[offset + 21] = (byte)(h.X6 >> 15);
                data[offset + 22] = (byte)((h.X6 >> 23) | (h.X7 << 3));

                data[offset + 23] = (byte)(h.X7 >> 5);
                data[offset + 24] = (byte)(h.X7 >> 13);
                data[offset + 25] = (byte)((h.X7 >> 21) | (h.X8 << 4));

                data[offset + 26] = (byte)(h.X8 >> 4);
                data[offset + 27] = (byte)(h.X8 >> 12);
                data[offset + 28] = (byte)((h.X8 >> 20) | (h.X9 << 6));

                data[offset + 29] = (byte)(h.X9 >> 2);
                data[offset + 30] = (byte)(h.X9 >> 10);
                data[offset + 31] = (byte)(h.X9 >> 18);
            }
        }

        #endregion
    }
}

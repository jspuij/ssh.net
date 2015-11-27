using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Mathematics
{
    /// <summary>
    /// Reference struct for a field element.
    /// </summary>
    /// <remarks>
    /// Here the field is \Z/(2^255-19).
    /// An element t, entries t[0]...t[9], represents the integer
    /// t[0]+2^26 t[1]+2^51 t[2]+2^77 t[3]+2^102 t[4]+...+2^230 t[9].
    /// Bounds on each t[i] vary depending on context.
    /// </remarks>
    public partial struct FieldElement
    {
        #region elements

        /// <summary>
        /// t[0]+2^26
        /// </summary>
        public int X0;

        /// <summary>
        /// t[1]+2^51
        /// </summary>
        public int X1;

        /// <summary>
        /// t[2]+2^77
        /// </summary>
        public int X2;

        /// <summary>
        /// t[3]+2^102
        /// </summary>
        public int X3;

        /// <summary>
        /// t[4]+2^128
        /// </summary>
        public int X4;

        /// <summary>
        /// t[5]+2^153
        /// </summary>
        public int X5;

        /// <summary>
        /// t[6]+2^179
        /// </summary>
        public int X6;

        /// <summary>
        /// t[7]+2^204
        /// </summary>
        public int X7;

        /// <summary>
        /// t[8]+2^230
        /// </summary>
        public int X8;

        /// <summary>
        /// t[9]
        /// </summary>
        public int X9;

        #endregion

        #region One and Zero

        /// <summary>
        /// 0
        /// </summary>
        public static FieldElement Zero = default(FieldElement);

        /// <summary>
        /// 1
        /// </summary>
        public static FieldElement One = new FieldElement() { X0 = 1 };

        #endregion

    }
}

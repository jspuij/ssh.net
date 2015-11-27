using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Renci.SshNet.Mathematics
{
    public partial struct FieldElement
    {
        /// <summary>
        /// Addition operator.
        /// </summary>
        /// <remarks>
        /// Preconditions:
		///   |fieldElement1| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24, etc.
		///   |fieldElement2| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24, etc.
        ///
        /// Postconditions:
		///   |return value| bounded by 1.1*2^26,1.1*2^25,1.1*2^26,1.1*2^25, etc.
        /// </remarks>
        /// <param name="fieldElement1">The first field element of the addition.</param>
        /// <param name="fieldElement2">The second field element of the addition.</param>
        /// <returns>A new FieldElement Struct.</returns>
        public static FieldElement operator +(FieldElement fieldElement1, FieldElement fieldElement2)
        {
            return new FieldElement
            {
                X0 = fieldElement1.X0 + fieldElement2.X0,
                X1 = fieldElement1.X1 + fieldElement2.X1,
                X2 = fieldElement1.X2 + fieldElement2.X2,
                X3 = fieldElement1.X3 + fieldElement2.X3,
                X4 = fieldElement1.X4 + fieldElement2.X4,
                X5 = fieldElement1.X5 + fieldElement2.X5,
                X6 = fieldElement1.X6 + fieldElement2.X6,
                X7 = fieldElement1.X7 + fieldElement2.X7,
                X8 = fieldElement1.X8 + fieldElement2.X8,
                X9 = fieldElement1.X9 + fieldElement2.X9
            };
        }

        /// <summary>
        /// Substraction operator.
        /// </summary>
        /// <remarks>
        /// Preconditions:
        ///   |fieldElement1| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24, etc.
        ///   |fieldElement2| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24, etc.
        ///
        /// Postconditions:
        ///   |return value| bounded by 1.1*2^26,1.1*2^25,1.1*2^26,1.1*2^25, etc.
        /// </remarks>
        /// <param name="fieldElement1">The first field element of the substraction.</param>
        /// <param name="fieldElement2">The second field element of the substraction.</param>
        /// <returns>A new FieldElement Struct.</returns>
        public static FieldElement operator -(FieldElement fieldElement1, FieldElement fieldElement2)
        {
            return new FieldElement
            {
                X0 = fieldElement1.X0 - fieldElement2.X0,
                X1 = fieldElement1.X1 - fieldElement2.X1,
                X2 = fieldElement1.X2 - fieldElement2.X2,
                X3 = fieldElement1.X3 - fieldElement2.X3,
                X4 = fieldElement1.X4 - fieldElement2.X4,
                X5 = fieldElement1.X5 - fieldElement2.X5,
                X6 = fieldElement1.X6 - fieldElement2.X6,
                X7 = fieldElement1.X7 - fieldElement2.X7,
                X8 = fieldElement1.X8 - fieldElement2.X8,
                X9 = fieldElement1.X9 - fieldElement2.X9
            };
        }
    }
}

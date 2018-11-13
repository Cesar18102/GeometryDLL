using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDLL.Figures.AuxilaryItems
{
    public class GlVectorR2
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.001f;

        protected float dX;
        protected float dY;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Vector constructor
        /// </summary>
        /// <param name="deltaX">Initial vector X coordinate</param>
        /// <param name="deltaY">Initial vector Y coordinate</param>
        public GlVectorR2(float deltaX, float deltaY)
        {
            this.dX = deltaX;
            this.dY = deltaY;

            this.Length = (float)Math.Sqrt(this.dX * this.dX + this.dY * this.dY);
        }

        /// <summary>
        /// Copying constructor
        /// </summary>
        /// <param name="copyVector">Copying vector</param>
        public GlVectorR2(GlVectorR2 copyVector) : 
            this(copyVector == null ? 0 : copyVector.dX, 
                 copyVector == null ? 0 : copyVector.dY) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Vector modular
        /// </summary>
        public float Length { get; private set; }

        /// <summary>
        /// Vector X coordinate
        /// </summary>
        public float deltaX { get { return dX; } }

        /// <summary>
        /// Vector Y coordinate
        /// </summary>
        public float deltaY { get { return dY; } }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        //////////////TRANSFORM_METHODS////////////////

        /// <returns>Counter-wise rotated vector</returns>
        public GlVectorR2 getRotatedVector(float angle)
        {
            if (this.isNullVector())
                return null;

            float CA = (float)Math.Cos(angle);
            float SA = (float)Math.Sin(angle);

            return new GlVectorR2(this.dX * CA - this.dY * SA, this.dX * SA + this.dY * CA);
        }

        /// <param name="SA">sin of counter-wise angle of rotation</param>
        /// <param name="CA">cos of counter-wise angle of rotation</param>
        /// <returns>Counter-wise rotated vector</returns>
        public GlVectorR2 getRotatedVector(float SA, float CA)
        {
            if (this.isNullVector())
                return new GlVectorR2(null);

            return this.getRotatedVector(-(float)Math.Atan(SA / CA));
        }

        /// <returns>A vector that is equal to the given one by the modular and opposite by the direction</returns>
        public GlVectorR2 getReversedVector()
        {
            return new GlVectorR2(-this.dX, -this.dY);
        }

        /// <returns>Scalar multiplication of the vectors</returns>
        public static float operator *(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? float.NaN : (V1.dX * V2.dX + V1.dY * V2.dY);
        }

        /// <returns>Result of multiplication of the vector by the number</returns>
        public static GlVectorR2 operator *(GlVectorR2 V1, float F)
        {
            return V1 == null ? new GlVectorR2(null) : new GlVectorR2(V1.dX * F, V1.dY * F);
        }

        /// <returns>Modular of vector multiplication of the vectors</returns>
        public static float operator ^(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? float.NaN : (V1.dX * V2.dY - V1.dY * V2.dX);
        }

        /// <returns>Vector equal to the summation of the vectors</returns>
        public static GlVectorR2 operator +(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? new GlVectorR2(null) : new GlVectorR2(V1.dX + V2.dX, V1.dY + V2.dY);
        }

        /// <returns>Vector equal to the substraction of the vectors</returns>
        public static GlVectorR2 operator -(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? new GlVectorR2(null) : (V1 + V2.getReversedVector());
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        /// <returns>If vectors are collinear</returns>
        public static bool isParallel(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? false : (Math.Abs(V1 ^ V2) < FAULT);
        }

        /// <returns>If vectors are perpendicular</returns>
        public static bool isPerpendicular(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? false : (Math.Abs(V1 * V2) < FAULT);
        }

        /// <returns>The point vector points to from the given point</returns>
        public GlPointR2 fromPointToPoint(GlPointR2 P)
        {
            return (P == null || P.isNullPoint()) ? new GlPointR2(null) : new GlPointR2(P.X + this.dX, P.Y + this.dY);
        }

        /// <returns>If coordinates of the vector are both equal to zeror</returns>
        public bool isNullVector()
        {
            return dX == 0 && dY == 0;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && GlVectorR2.Equals(obj as GlVectorR2, this);
        }

        /// <returns>If vectors are equal</returns>
        public static bool Equals(GlVectorR2 V1, GlVectorR2 V2)
        {
            return (V1 == null || V2 == null) ? false : (V1.dX == V2.dX && V1.dY == V2.dY);
        }

        public override string ToString()
        {
            return "(" + this.dX + ", " + this.dY + ")";
        }

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

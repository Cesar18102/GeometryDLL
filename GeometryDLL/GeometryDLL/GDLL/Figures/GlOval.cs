using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures
{
    public class GlOval : GlCurve
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.001f;

        private float Ra;
        private float Rb;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        /// <param name="R1">size of X half-axis</param>
        /// <param name="R2">size of Y half-axis</param>
        /// <param name="VectorR1">Vector of x-axis positive direction</param>
        /// <param name="OvalCenter">Point of the center of the oval</param>
        public GlOval(float R1, float R2, GlVectorR2 VectorR1, GlPointR2 OvalCenter)
        {
            this.curvePoints = new GlPointR2[360];

            if (VectorR1 == null || OvalCenter == null)
            {
                directVector = new GlVectorR2(0, 0);
                systemCenter = new GlPointR2(float.NaN, float.NaN);
                Ra = 0;
                Rb = 0;
                return;
            }

            this.systemCenter = new GlPointR2(OvalCenter);

            this.Ra = R1;
            this.Rb = R2;

            this.directVector = VectorR1;

            if (VectorR1.isNullVector())
                return;

            this.Length = (int)Math.Ceiling(Math.PI * Math.Sqrt(2 * (R1 * R1 + R2 * R2)));

            updatePointsPosition();
        }

        /// <summary>
        /// Copying constructor
        /// </summary>
        public GlOval(GlOval copyOval) :
            this(copyOval == null ? 0 : copyOval.RadA,
                 copyOval == null ? 0 : copyOval.RadB,
                 copyOval == null ? new GlVectorR2(0, 0) : copyOval.directVector,
                 copyOval == null ? new GlPointR2(float.NaN, float.NaN) : new GlPointR2(copyOval.CenterX, copyOval.CenterY)) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Length of the oval
        /// </summary>
        public float Length { get; private set; }

        /// <summary>
        /// X half-axis
        /// </summary>
        public virtual float RadA { get { return this.Ra; } set { this.Ra = Math.Abs(value); updatePointsPosition(); } }

        /// <summary>
        /// Y half-axis
        /// </summary>
        public virtual float RadB { get { return this.Rb; } set { this.Rb = Math.Abs(value); updatePointsPosition(); } }

        /// <summary>
        /// Left-side oval focus
        /// </summary>
        public GlPointR2[] Focuses
        {
            get
            {
                GlOval TO = new GlOval(Math.Max(RadA, RadB), Math.Min(RadA, RadB), RadA >= RadB ? this.directVector : this.directVector.getRotatedVector((float)Math.PI / 2), this.Center);

                return new GlPointR2[]{
                    new GlPointR2(-(float)Math.Sqrt(TO.RadA * TO.RadA - TO.RadB * TO.RadB), 0).getTranslatedBackPoint(TO.SIN, TO.COS, TO.Center),
                    new GlPointR2((float)Math.Sqrt(TO.RadA * TO.RadA - TO.RadB * TO.RadB), 0).getTranslatedBackPoint(TO.SIN, TO.COS, TO.Center)
                };
            }
        }

        public override GlRectangle RealBox
        {
            get
            {
                GlLineR2 L1 = this.getTangentFromBelongs(this[180]);
                GlLineR2 L2 = L1.getPerpendicular(this[270]);
                GlPointR2 I = L2.getIntersection(L1)[0];
                return new GlRectangle(I, new GlVectorR2(this[90].X - this[270].X, this[90].Y - this[270].Y).Length, new GlVectorR2(this[0].X - this[180].X, this[0].Y - this[180].Y).Length, this.directVector);
            }
        }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////
     

        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        //////////////TRANSFORM_METHODS////////////////

        public override GlFigure getScaled(float scale)
        {
            return new GlOval(this.RadA * scale, this.RadB * scale, this.directVector, this.Center);
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            if (!this.isIntersects(L))
                return new GlPointR2[] { };

            GlPointR2[] Intersections = new GlPointR2[2];
            GlPointR2 LineMovedPoint = L.PointOfLine.getPointTranslatedToRotatedSystem(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));
            GlLineR2 IL = new GlLineR2(LineMovedPoint, L.DirectVector.getRotatedVector(this.SIN, this.COS));

            float Devider = (float)(Math.Pow(this.RadA * IL.DirectVector.deltaY, 2.0) + Math.Pow(this.RadB * IL.DirectVector.deltaX, 2.0));
            float PartRes = (float)Math.Sqrt(Math.Pow(this.RadA * IL.DirectVector.deltaY, 2.0) + Math.Pow(this.RadB * IL.DirectVector.deltaX, 2.0) - Math.Pow(IL.PointOfLine.X * IL.DirectVector.deltaY, 2.0) + (double)(2 * IL.PointOfLine.X * IL.PointOfLine.Y * IL.DirectVector.deltaX * IL.DirectVector.deltaY) - Math.Pow(IL.DirectVector.deltaX * IL.PointOfLine.Y, 2.0));

            float xPart = this.RadA * IL.PointOfLine.X * (float)Math.Pow(IL.DirectVector.deltaY, 2.0) - this.RadA * IL.DirectVector.deltaX * IL.DirectVector.deltaY * IL.PointOfLine.Y;
            float yPart = this.RadB * IL.PointOfLine.Y * (float)Math.Pow(IL.DirectVector.deltaX, 2.0) - this.RadB * IL.DirectVector.deltaX * IL.DirectVector.deltaY * IL.PointOfLine.X;

            float xC1 = this.RadA * (this.RadB * IL.DirectVector.deltaX * PartRes + xPart) / Devider;
            float yC1 = this.RadB * (this.RadA * IL.DirectVector.deltaY * PartRes + yPart) / Devider;

            float xC2 = -this.RadA * (this.RadB * IL.DirectVector.deltaX * PartRes - xPart) / Devider;
            float yC2 = -this.RadB * (this.RadA * IL.DirectVector.deltaY * PartRes - yPart) / Devider;

            Intersections[0] = new GlPointR2(xC1, yC1).getTranslatedBackPoint(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));
            Intersections[1] = new GlPointR2(xC2, yC2).getTranslatedBackPoint(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));

            return Intersections;
        }

        /// <returns>If the line intersects the oval</returns>
        public bool isIntersects(GlLineR2 L)
        {
            if (L == null || L.isNullLine())
                return false;

            GlVectorR2 LV = L.DirectVector.getRotatedVector(this.SIN, this.COS);
            return (Math.Pow(this.RadA * LV.deltaY, 2.0) + Math.Pow(this.RadB * LV.deltaX, 2.0) != 0);
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        ////////////////TANGENT_METHODS////////////////

        public GlLineR2[] getTangentFromPoint(GlPointR2 P)
        {
            if (P == null || P.isNullPoint() || this.isPointInside(P))
                return new GlLineR2[] { };

            GlOval IO = new GlOval(this.RadA, this.RadB, new GlVectorR2(this.RadA, 0.0f), new GlPointR2(0.0f, 0.0f));
            GlPointR2 MovedPoint = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));

            if (this.RadA > this.RadB)
            {
                IO = new GlOval(this.RadB, this.RadA, this.directVector.getRotatedVector((float)Math.PI / 2), new GlPointR2(0.0f, 0.0f));
                MovedPoint.moveTo(MovedPoint.Y, -MovedPoint.X);
            }

            float Devider = (float)(Math.Pow(IO.RadA * MovedPoint.Y, 2.0) + Math.Pow(IO.RadB * MovedPoint.X, 2.0));
            float fPartRes = (float)Math.Pow(IO.RadA, 3.0) * IO.RadB * MovedPoint.X;
            float sPartRes = IO.RadA * IO.RadA * MovedPoint.Y * (float)Math.Sqrt(Math.Abs(-Math.Pow(IO.RadA, 4.0) + Math.Pow(IO.RadA * MovedPoint.Y, 2.0) + Math.Pow(IO.RadB * MovedPoint.X, 2.0)));

            float xI1 = (fPartRes + sPartRes) / Devider;
            float xI2 = (fPartRes - sPartRes) / Devider;

            float yI1 = (MovedPoint.X < 0 ? 1 : -1) * IO.RadB * (float)Math.Sqrt(Math.Abs(IO.RadA * IO.RadA - xI1 * xI1)) / IO.RadA;
            float yI2 = (MovedPoint.X < 0 ? -1 : 1) * IO.RadB * (float)Math.Sqrt(Math.Abs(IO.RadA * IO.RadA - xI2 * xI2)) / IO.RadA;

            if (Math.Abs(MovedPoint.X) < IO.RadA)
            {
                yI1 = (MovedPoint.Y < 0 && yI1 >= 0) ? -Math.Abs(yI1) : ((MovedPoint.Y > 0 && yI1 < 0) ? Math.Abs(yI1) : yI1);
                yI2 = (MovedPoint.Y < 0 && yI2 >= 0) ? -Math.Abs(yI2) : ((MovedPoint.Y > 0 && yI2 < 0) ? Math.Abs(yI2) : yI2);
            }

            if (this.RadA > this.RadB)
            {
                float temp = xI1;
                xI1 = -yI1;
                yI1 = temp;

                temp = xI2;
                xI2 = -yI2;
                yI2 = temp;
            }

            GlPointR2 P1 = new GlPointR2(xI1, yI1).getTranslatedBackPoint(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));
            GlPointR2 P2 = new GlPointR2(xI2, yI2).getTranslatedBackPoint(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));

            return new GlLineR2[] { 
                new GlLineR2(P1, new GlVectorR2(P1.X - P.X, P1.Y - P.Y)), 
                new GlLineR2(P2, new GlVectorR2(P2.X - P.X, P2.Y - P.Y))
            };
        }

        public override GlLineR2 getTangentFromBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint() || !this.isPointBelongs(P) || this.RadA == 0.0f)
                return new GlLineR2(new GlPointR2(null), new GlVectorR2(null));

            GlOval IO = new GlOval(this.RadA, this.RadB, new GlVectorR2(this.RadA, 0.0f), new GlPointR2(0.0f, 0.0f));
            GlPointR2 MovedPoint = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));

            GlLineR2 tangent = new GlLineR2(MovedPoint, new GlVectorR2(-(float)Math.Pow(this.RadA, 2.0) * MovedPoint.Y, (float)Math.Pow(this.RadB, 2.0) * MovedPoint.X));
            tangent.Rotate(this.SIN, this.COS);
            tangent.moveTo(P);

            return tangent;
        }

        ////////////////TANGENT_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INSIDE_BELONGS_METHODS////////////

        public override bool isPointInside(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return false;

            GlPointR2 LineMovedPoint = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));
            return Math.Pow(LineMovedPoint.X / this.RadA, 2.0) + Math.Pow(LineMovedPoint.Y / this.RadB, 2.0) - 1 < FAULT;
        }

        public override bool isPointBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return false;

            GlPointR2 LineMovedPoint = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, new GlPointR2(this.CenterX, this.CenterY));
            return Math.Abs(Math.Pow(LineMovedPoint.X / this.RadA, 2.0) + Math.Pow(LineMovedPoint.Y / this.RadB, 2.0) - 1) < FAULT;
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        public override float getFDiff(float X) { return -Rb * X / (Ra * (float)Math.Sqrt(Ra * Ra - X * X)); }
        public override float getSDiff(float X) { return -Ra * Rb / (float)Math.Pow(Ra * Ra - X * X, 1.5); }   

        protected override void updatePointsPosition()
        {
            this.SIN = this.directVector.deltaY / this.directVector.Length;
            this.COS = this.directVector.deltaX / this.directVector.Length;

            for (int i = -90; i < 90; i++)
            {
                float AngleTan = (float)Math.Tan(Math.PI * i / 180);

                float PointX1 = this.Ra * this.Rb * (COS - SIN * AngleTan) / (float)Math.Sqrt(this.Ra * this.Ra * AngleTan * AngleTan + this.Rb * this.Rb);
                float PointX2 = -PointX1;

                this.curvePoints[i + 90] = new GlPointR2(PointX1 + CenterX, PointX1 * (SIN + COS * AngleTan) / (COS - SIN * AngleTan) + CenterY);
                this.curvePoints[i + 270] = new GlPointR2(PointX2 + CenterX, PointX2 * (SIN + COS * AngleTan) / (COS - SIN * AngleTan) + CenterY);
            }
        }

        /// <returns>If the oval doesn't exist</returns>
        public bool isNullOval()
        {
            return RadA == 0 || RadB == 0 || directVector.isNullVector();
        }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && (obj as GlOval).Ra == this.Ra && (obj as GlOval).Rb == this.Rb && (obj as GlOval).systemCenter.Equals(this.systemCenter) && (obj as GlOval).directVector.Equals(this.directVector);
        }

        public override string ToString()
        {
            return "x^2 / " + Math.Pow(this.RadA, 2.0) + " + " + "y^2 / " + Math.Pow(this.RadB, 2.0) + " = 1";
        }

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

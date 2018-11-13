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
    public class GlHyperbola : GlCurve
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.02f;

        private float Ra;
        private float Rb;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlHyperbola(float realHalfAxis, float additionalHalfAxis, GlVectorR2 xAxisDirection, GlPointR2 systemCenter)
        {
            this.curvePoints = new GlPointR2[179];

            if (xAxisDirection == null || systemCenter == null)
            {
                directVector = new GlVectorR2(0, 0);
                systemCenter = new GlPointR2(float.NaN, float.NaN);
                Ra = 0;
                Rb = 0;
                return;
            }

            this.directVector = xAxisDirection;
            this.systemCenter = systemCenter;

            this.Ra = realHalfAxis;
            this.Rb = additionalHalfAxis;

            if (directVector.isNullVector() || systemCenter.isNullPoint())
                return;

            updatePointsPosition();
        }

        public GlHyperbola(GlHyperbola copyHyperbola) :
            this(copyHyperbola == null ? 0 : copyHyperbola.Ra,
                 copyHyperbola == null ? 0 : copyHyperbola.Rb,
                 copyHyperbola == null ? new GlVectorR2(0, 0) : copyHyperbola.directVector,
                 copyHyperbola == null ? new GlPointR2(float.NaN, float.NaN) : copyHyperbola.systemCenter) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public float RealHalfAixis { get { return Ra; } }
        public float AdditionalHalfAixis { get { return Rb; } }    

        public override GlRectangle RealBox
        {
            get
            {
                GlLineR2 L = this.getTangentFromBelongs(this[179]);
                GlPointR2 I = L.getIntersection(L.getPerpendicular(this[89]))[0];
                return new GlRectangle(this[89], new GlVectorR2(this[91].X - this[89].X, this[91].Y - this[89].Y).Length, new GlVectorR2(I.X - this[89].X, I.Y - this[89].Y).Length, this.directVector.getRotatedVector(-(float)Math.PI / 2));
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
            return new GlHyperbola(this.Ra * scale, this.Rb * scale, this.directVector, this.systemCenter);
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            GlVectorR2 translatedVector = new GlVectorR2(L.DirectVector).getRotatedVector(this.SIN, this.COS);
            GlPointR2 FP = L.PointOfLine.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Center);
            GlPointR2 SP = translatedVector.fromPointToPoint(FP);

            float fPartRes = (float)Math.Pow(AdditionalHalfAixis * (FP.X - SP.X), 2.0);
            float sPartRes = (float)Math.Pow(RealHalfAixis * (FP.Y - SP.Y), 2.0);
            float tPartRes = FP.X * SP.Y - SP.X * FP.Y;
            float sqrtPart = RealHalfAixis * AdditionalHalfAixis * (FP.X - SP.X) * (float)Math.Sqrt(fPartRes - sPartRes + Math.Pow(tPartRes, 2.0));

            float x1 = (-(float)Math.Pow(RealHalfAixis, 2.0) * (FP.Y - SP.Y) * tPartRes + sqrtPart) / (sPartRes - fPartRes);
            float x2 = (-(float)Math.Pow(RealHalfAixis, 2.0) * (FP.Y - SP.Y) * tPartRes - sqrtPart) / (sPartRes - fPartRes);

            float y1 = AdditionalHalfAixis * (float)Math.Sqrt(x1 * x1 - Math.Pow(RealHalfAixis, 2.0)) / RealHalfAixis;
            float y2 = -AdditionalHalfAixis * (float)Math.Sqrt(x2 * x2 - Math.Pow(RealHalfAixis, 2.0)) / RealHalfAixis;

            float y3 = -AdditionalHalfAixis * (float)Math.Sqrt(x1 * x1 - Math.Pow(RealHalfAixis, 2.0)) / RealHalfAixis;
            float y4 = AdditionalHalfAixis * (float)Math.Sqrt(x2 * x2 - Math.Pow(RealHalfAixis, 2.0)) / RealHalfAixis;

            GlPointR2[] RP = {
                new GlPointR2(x1, y1).getTranslatedBackPoint(this.SIN, this.COS, this.Center),
                new GlPointR2(x2, y2).getTranslatedBackPoint(this.SIN, this.COS, this.Center),
                new GlPointR2(x1, y3).getTranslatedBackPoint(this.SIN, this.COS, this.Center),
                new GlPointR2(x2, y4).getTranslatedBackPoint(this.SIN, this.COS, this.Center)
            };

            List<GlPointR2> res = new List<GlPointR2>();

            for (int i = 0; i < RP.Length; i++)
            {
                bool a = this.isPointBelongs(RP[i]);
                bool b = L.isPointBelongs(RP[i]);
                if (a && b)
                    res.Add(RP[i]);
            }

            return res.ToArray();
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        ////////////////TANGENT_METHODS////////////////

        public override GlLineR2 getTangentFromBelongs(GlPointR2 P)
        {
            if (P == null || !this.isPointBelongs(P))
                return new GlLineR2(new GlPointR2(null), new GlVectorR2(null));

            GlPointR2 TP = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Center);
            float k = (TP.Y >= 0 ? 1 : -1) * AdditionalHalfAixis * TP.X / (RealHalfAixis * (float)Math.Sqrt(TP.X * TP.X - Math.Pow(RealHalfAixis, 2.0)));

            GlLineR2 tangent = new GlLineR2(P, new GlVectorR2(1, 0).getRotatedVector((float)Math.Atan(k)));
            tangent.Rotate(this.SIN, this.COS);
            return tangent;
        }

        ////////////////TANGENT_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INSIDE_BELONGS_METHODS////////////

        public override bool isPointInside(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return false;

            GlPointR2 TP = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Center);
            return Math.Pow(TP.X / RealHalfAixis, 2.0) - Math.Pow(TP.Y / AdditionalHalfAixis, 2.0) - 1 > -FAULT;
        }

        public override bool isPointBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return false;

            GlPointR2 TP = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Center);
            return Math.Abs(TP.X - RealHalfAixis * Math.Sqrt(TP.Y * TP.Y + Math.Pow(AdditionalHalfAixis, 2.0)) / AdditionalHalfAixis) < FAULT;
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        public override float getFDiff(float X) { return Rb * X / (Ra * (float)Math.Sqrt(X * X - Ra * Ra)); }
        public override float getSDiff(float X) { return -Ra * Rb / (float)Math.Pow(X * X - Ra * Ra, 1.5); }

        protected override void updatePointsPosition()
        {
            this.SIN = this.directVector.deltaY / this.directVector.Length;
            this.COS = this.directVector.deltaX / this.directVector.Length;

            for (int i = -89; i < 90; i++)
            {
                float angleTan = (float)Math.Tan(i * Math.PI / 180.0);
                float angleTanPow = (float)Math.Pow(angleTan, 2.0);

                float x = (float)(RealHalfAixis * (Math.Pow(AdditionalHalfAixis, 2.0) * Math.Sqrt(angleTanPow + 1) - RealHalfAixis * angleTanPow * Math.Sqrt(Math.Pow(RealHalfAixis, 2.0) + Math.Pow(AdditionalHalfAixis, 2.0))) / (Math.Pow(AdditionalHalfAixis, 2.0) - Math.Pow(RealHalfAixis * angleTan, 2.0)));
                float y = (float)(angleTan * (Math.Sqrt(Math.Pow(RealHalfAixis, 2.0) + Math.Pow(AdditionalHalfAixis, 2.0)) - x));

                this.curvePoints[i + 89] = new GlPointR2(x, y).getTranslatedBackPoint(this.SIN, this.COS, this.systemCenter);
            }
        }

        public bool isNullHyperbola()
        {
            return directVector == null || systemCenter == null || directVector.isNullVector() || systemCenter.isNullPoint();
        }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && (obj as GlHyperbola).systemCenter.Equals(this.systemCenter) && (obj as GlHyperbola).directVector.Equals(this.directVector) && (obj as GlHyperbola).Ra == this.Ra && (obj as GlHyperbola).Rb == this.Rb;
        }

        public override string ToString()
        {
            return "x^2 / " + RealHalfAixis + "^2 - y^2 / " + AdditionalHalfAixis + "^2 = 1";
        }

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

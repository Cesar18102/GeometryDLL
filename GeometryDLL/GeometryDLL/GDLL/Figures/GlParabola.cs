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
    public class GlParabola : GlCurve
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.02f;

        private float a;
        private float b;
        private float c;

        private GlPointR2 parabolaFocus;
        private GlLineR2 parabolaDirectriss;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlParabola(float aCoeff, GlPointR2 Vertex, GlVectorR2 directVector)
        {
            this.curvePoints = new GlPointR2[360];

            if (Vertex == null || directVector == null)
            {
                this.directVector = new GlVectorR2(0,0);
                systemCenter = new GlPointR2(float.NaN, float.NaN);
                a = 0;
                return;
            }

            this.a = Math.Abs(aCoeff);
            this.systemCenter = Vertex;
            this.directVector = (aCoeff < 0 && !directVector.isNullVector()) ? directVector.getReversedVector() : directVector;

            if (isNullParabola())
                return;

            updatePointsPosition();
        }

        public GlParabola(GlParabola copyParabola) :
            this(copyParabola == null ? 0 : copyParabola.A,
                 copyParabola == null ? new GlPointR2(float.NaN, float.NaN) : copyParabola.systemCenter,
                 copyParabola == null ? new GlVectorR2(0, 0) : copyParabola.directVector) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public float A
        {
            get { return this.a; }
            set
            {
                if (value == 0)
                    return;

                if (value < 0)
                    this.directVector = this.directVector.getReversedVector();

                this.a = Math.Abs(value);

                updatePointsPosition();
            }
        }

        public float B { get { return this.b; } }
        public float C { get { return this.c; } }

        public GlPointR2 Focus { get { return new GlPointR2(this.parabolaFocus); } }
        public GlLineR2 Directriss { get { return new GlLineR2(this.parabolaDirectriss); } }
        public GlPointR2 Vertex { get { return new GlPointR2(this.systemCenter); } }

        public override GlRectangle RealBox
        {
            get
            {
                GlLineR2 L = this.getTangentFromBelongs(this[180]);
                GlLineR2 L2 = L.getPerpendicular(this[0]);
                GlPointR2 I = L2.getIntersection(L)[0];
                return new GlRectangle(this[0], new GlVectorR2(this[359].X - this[0].X, this[359].Y - this[0].Y).Length, new GlVectorR2(I.X - this[0].X, I.Y - this[0].Y).Length, this.directVector);
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
            return new GlParabola(this.A / scale, this.systemCenter, this.directVector);
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            if (L == null || this.isNullParabola() || L.isNullLine())
                return new GlPointR2[] { };

            GlParabola PC = new GlParabola(this.A, new GlPointR2(0, 0), new GlVectorR2(1, 0));
            GlPointR2 LP0 = L.PointOfLine.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Vertex);
            GlPointR2 LP1 = L.DirectVector.getRotatedVector(this.SIN, this.COS).fromPointToPoint(LP0);

            float fPartRes = LP0.Y - LP1.Y;
            float sPartRes = (float)Math.Sqrt(4 * PC.A * (LP0.X - LP1.X) * (LP0.X * LP1.Y - LP1.X * LP0.Y + PC.C * (LP1.X - LP0.X)) + (float)Math.Pow(PC.B * (LP0.X - LP1.X) - LP0.Y + LP1.Y, 2.0));
            float tPartRes = PC.B * (LP1.X - LP0.X);
            float devider = 2 * PC.A * (LP0.X - LP1.X);

            float x1 = (fPartRes + sPartRes + tPartRes) / devider;
            float x2 = (fPartRes - sPartRes + tPartRes) / devider;

            float y1 = PC.A * x1 * x1 + PC.B * x1 + PC.C;
            float y2 = PC.A * x2 * x2 + PC.B * x2 + PC.C;

            return new GlPointR2[]{
                new GlPointR2(x1, y1).getTranslatedBackPoint(this.SIN, this.COS, this.Vertex),
                new GlPointR2(x2, y2).getTranslatedBackPoint(this.SIN, this.COS, this.Vertex)
            };
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        ////////////////TANGENT_METHODS////////////////

        public override GlLineR2 getTangentFromBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return new GlLineR2(new GlPointR2(null), new GlVectorR2(null));

            GlParabola TPB = new GlParabola(this.A, new GlPointR2(0, 0), new GlVectorR2(1, 0));
            GlPointR2 TP = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Vertex);

            float k = 2 * TPB.A * TP.X + TPB.B;

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

            GlPointR2 RP = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Vertex);
            GlParabola RPB = new GlParabola(this.A, new GlPointR2(0, 0), new GlVectorR2(1, 0));
            return RP.Y - RPB.A * RP.X * RP.X - RPB.B * RP.X - RPB.C > FAULT;
        }

        public override bool isPointBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return false;

            GlPointR2 RP = P.getPointTranslatedToRotatedSystem(this.SIN, this.COS, this.Vertex);
            GlParabola RPB = new GlParabola(this.A, new GlPointR2(0, 0), new GlVectorR2(1, 0));
            return Math.Abs(RP.Y - RPB.A * RP.X * RP.X - RPB.B * RP.X - RPB.C) < FAULT;
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        public override float getFDiff(float X) { GlParabola PB = new GlParabola(a, new GlPointR2(0, 0), new GlVectorR2(1, 0)); return 2 * PB.A * X + PB.B; }
        public override float getSDiff(float X) { return 2 * A; }

        protected override void updatePointsPosition()
        {
            this.b = -2 * this.a * this.systemCenter.X;
            this.c = this.a * this.a * this.systemCenter.X + this.systemCenter.Y;

            this.COS = directVector.deltaX / directVector.Length;
            this.SIN = directVector.deltaY / directVector.Length;

            float yFocus = 1 / Math.Abs(4 * a);
            this.parabolaFocus = new GlPointR2(-yFocus * SIN + systemCenter.X, yFocus * COS + systemCenter.Y);

            this.parabolaDirectriss = new GlLineR2(new GlPointR2(yFocus * SIN + systemCenter.X, -yFocus * COS + systemCenter.Y), new GlVectorR2(1, 0).getRotatedVector(-SIN, COS));

            for (int i = 0; i < 180; i++)
            {
                float SIN_A = (float)Math.Sin(i * Math.PI / 180);
                float COS_A = (float)Math.Cos(i * Math.PI / 180);
                float SINH_A = (float)Math.Sin(i * Math.PI / 360);
                float AbsedA = (float)Math.Abs(a);

                float PartRes = (float)(Math.Sqrt(2 * AbsedA) * Math.Sqrt(2 * a * SIN_A * SIN_A - 8 * a * SINH_A * SINH_A + 8 * SINH_A * SINH_A * AbsedA));
                float fDevider = 8 * AbsedA * AbsedA - 4 * a * AbsedA + 4 * a * AbsedA * COS_A;

                float x1 = PartRes / fDevider;
                float x2 = PartRes / -fDevider;

                float y1 = a * x1 * x1;
                float y2 = a * x2 * x2;

                this.curvePoints[180 + i] = new GlPointR2(x1 * COS - y1 * SIN + systemCenter.X, x1 * SIN + y1 * COS + systemCenter.Y);
                this.curvePoints[179 - i] = new GlPointR2(x2 * COS - y2 * SIN + systemCenter.X, x2 * SIN + y2 * COS + systemCenter.Y);
            }
        }

        public bool isNullParabola()
        {
            return a == 0 || directVector == null || systemCenter == null || directVector.isNullVector() || systemCenter.isNullPoint();
        }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && (obj as GlParabola).a == this.a && (obj as GlParabola).systemCenter.Equals(this.systemCenter) && (obj as GlParabola).directVector.Equals(this.directVector);
        }

        public override string ToString()
        {
            return "y = " + a + " * x^2 + " + this.B + " * x + " + this.C;
        }

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform;
using GDLL.Figures.AuxilaryItems;
using GDLL.Paint.Texturing;
using GDLL.Paint.Coloring;

namespace GDLL.Figures
{
    public class GlLineR2 : GlFigure
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.001f;

        private GlPointR2 pointOfLine;
        private GlVectorR2 directVector;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlLineR2(GlPointR2 belongsPoint, GlVectorR2 directVector)
        {
            if (belongsPoint == null || directVector == null)
            {
                this.directVector = new GlVectorR2(0, 0);
                pointOfLine = new GlPointR2(float.NaN, float.NaN);
                return;
            }

            this.pointOfLine = belongsPoint;
            this.directVector = directVector;
        }

        public GlLineR2(GlLineR2 copyLine) :
            this(copyLine == null ? new GlPointR2(float.NaN, float.NaN) : copyLine.PointOfLine,
                 copyLine == null ? new GlVectorR2(0, 0) : copyLine.DirectVector) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public GlPointR2 PointOfLine { get { return new GlPointR2(this.pointOfLine); } }
        public GlVectorR2 DirectVector { get { return new GlVectorR2(directVector); } }

        public double Length { get { return double.PositiveInfinity; } }
        public override int CountOfPoints { get { return 2; } }

        public override GlPointR2 Center { get { return new GlPointR2(this.PointOfLine); } }

        public override GlRectangle BOX
        {
            get
            {
                GlPointR2 P1 = this.directVector.fromPointToPoint(this.PointOfLine);
                return new GlRectangle(P1, this.directVector.deltaX * 2, this.directVector.deltaY * 2, this.directVector.getRotatedVector(-(float)Math.PI / 2));
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

        public void moveTo(GlPointR2 newBelongsPoint)
        {
            if (newBelongsPoint == null || newBelongsPoint.isNullPoint())
                return;

            if (OnMoveStart != null)
                if (!OnMoveStart.Invoke(this, this.pointOfLine.X, this.pointOfLine.Y))
                    return;

            if (OnMoving != null)
                OnMoving.Invoke(this, this.pointOfLine.X, this.pointOfLine.Y);

            GlPointR2 OP = new GlPointR2(this.PointOfLine);

            this.pointOfLine = newBelongsPoint;

            if (OnMoved != null)
                OnMoved.Invoke(this, OP.X, OP.Y);
        }

        public override void moveTo(float x, float y)
        {
            this.moveTo(new GlPointR2(x, y));
        }

        public override void Rotate(float SIN, float COS)
        {
            if (OnRotateStart != null)
                if (!OnRotateStart.Invoke(this, SIN, COS))
                    return;

            if (OnRotating != null)
                OnRotating.Invoke(this, SIN, COS);

            this.directVector = this.DirectVector.getRotatedVector(-SIN, COS);

            if (OnRotated != null)
                OnRotated.Invoke(this, SIN, COS);
        }

        public override void Rotate(float angle)
        {
            this.Rotate((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        public override GlFigure getScaled(float scale)
        {
            return new GlLineR2(this);
        }

        public GlPointR2 getProjection(GlPointR2 P)
        {
            if (P == null || this.PointOfLine.isNullPoint() || P.isNullPoint())
                return new GlPointR2(null);

            if (this.DirectVector.isNullVector())
                return this.pointOfLine;

            float both = (this.DirectVector.deltaX * (P.X - this.pointOfLine.X) + this.DirectVector.deltaY * (P.Y - this.pointOfLine.Y)) / (float)Math.Pow(this.DirectVector.Length, 2.0);
            return new GlPointR2(this.DirectVector.deltaX * both + this.pointOfLine.X, this.DirectVector.deltaY * both + this.pointOfLine.Y);
        }

        public GlLineR2 getPerpendicular(GlPointR2 P)
        {
            GlPointR2 projection = this.getProjection(P);

            if (projection == null)
                return null;

            return new GlLineR2(projection, new GlVectorR2(projection.X - P.X, projection.Y - P.Y));
        }

        public GlLineR2 getParallel(GlPointR2 P)
        {
            if (P == null || this.pointOfLine.isNullPoint() || P.isNullPoint())
                return null;

            return new GlLineR2(P, this.DirectVector);
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            if (L == null || L.pointOfLine.isNullPoint())
                return new GlPointR2[] { };

            if (GlPointR2.Equals(this.pointOfLine, L.pointOfLine))//already have a result
                return new GlPointR2[] { new GlPointR2(L.pointOfLine) };

            bool isL1Point = this.DirectVector.isNullVector();
            bool isL2Point = L.DirectVector.isNullVector();

            if(isL1Point && isL2Point)//same points were catched in previous step
                return new GlPointR2[] { };

            if (this.DirectVector.isNullVector() && L.isPointBelongs(this.pointOfLine))//line and a point
                return new GlPointR2[] { new GlPointR2(this.pointOfLine) };

            if (L.DirectVector.isNullVector() && this.isPointBelongs(L.pointOfLine))//line and a point
                return new GlPointR2[] { new GlPointR2(L.pointOfLine) };

            if (GlLineR2.Equals(this, L))//lines are identical
                return new GlPointR2[] { new GlPointR2(this.pointOfLine), new GlPointR2(L.PointOfLine) };

            if (GlVectorR2.isParallel(this.DirectVector, L.DirectVector))//lines are parallel
                return new GlPointR2[] { };

            if (L.DirectVector.deltaX == 0)//L2 is parallel to Y axis
                return new GlPointR2[] { new GlPointR2(L.pointOfLine.X, this.DirectVector.deltaY * (L.pointOfLine.X - this.pointOfLine.X) / this.DirectVector.deltaX + this.pointOfLine.Y) };

            if (this.DirectVector.deltaY == 0)//L1 is parallel to X axis
                return new GlPointR2[] { new GlPointR2(L.DirectVector.deltaX * (this.pointOfLine.Y - L.pointOfLine.Y) / L.DirectVector.deltaY + L.pointOfLine.X, this.pointOfLine.Y) };

            float v2RatYX = L.DirectVector.deltaY / L.DirectVector.deltaX;
            float v1RatXY = this.DirectVector.deltaX / this.DirectVector.deltaY;
            float yInter = (v1RatXY * v2RatYX * this.pointOfLine.Y - v2RatYX * this.pointOfLine.X + v2RatYX * L.pointOfLine.X - L.pointOfLine.Y) / (v1RatXY * v2RatYX - 1);
            float xInter = v1RatXY * (yInter - this.pointOfLine.Y) + this.pointOfLine.X;
            return new GlPointR2[] { new GlPointR2(xInter, yInter) };//common situation
        }

        public override GlPointR2[] getIntersection(GlCurve C)
        {
            return C == null ? new GlPointR2[] { } : C.getIntersection(this);
        }

        public override GlPointR2[] getIntersection(GlPolygon POLY)
        {
            if (POLY == null || POLY.CountOfPoints == 0)
                return new GlPointR2[] { };

            GlPointR2[] Intersections = new GlPointR2[POLY.CountOfPoints];
            int countOfIntersections = 0;

            GlPointR2[] faultInter = new GlLineR2(POLY[POLY.CountOfPoints - 1], new GlVectorR2(POLY[0].X - POLY[POLY.CountOfPoints - 1].X, POLY[0].Y - POLY[POLY.CountOfPoints - 1].Y)).getIntersection(this);

            if (faultInter.Length == 1 && new GlLineSegment(POLY[0], POLY[POLY.CountOfPoints - 1]).isPointBelongs(faultInter[0]))
                Intersections[countOfIntersections++] = faultInter[0];

            for (int i = 0; i < POLY.CountOfPoints - 1; i++)
            {
                faultInter = new GlLineR2(POLY[i], new GlVectorR2(POLY[i + 1].X - POLY[i].X, POLY[i + 1].Y - POLY[i].Y)).getIntersection(this);

                if (faultInter.Length == 1 && new GlLineSegment(POLY[i + 1], POLY[i]).isPointBelongs(faultInter[0]))
                    Intersections[countOfIntersections++] = faultInter[0];
            }

            GlPointR2[] result = new GlPointR2[countOfIntersections];
            Array.Copy(Intersections, result, countOfIntersections);
            return result;
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        ////////////////DRAW_METHODS///////////////////

        public override void Draw()
        {
            if (this.pointOfLine.isNullPoint() || !ActivateDrawStart())//is it possible???
                return;

            ActivateDrawing();

            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2f(this.pointOfLine.X - this.DirectVector.deltaX, this.pointOfLine.Y - this.DirectVector.deltaY);
            Gl.glVertex2f(this.pointOfLine.X + this.DirectVector.deltaX, this.pointOfLine.Y + this.DirectVector.deltaY);
            Gl.glEnd();

            ActivateDrawed();
        }

        public void Draw(float length)
        {
            if (this.pointOfLine.isNullPoint())//is it possible???
                return;

            if (OnDrawStart != null)
                if (!OnDrawStart.Invoke(this))
                    return;

            if (OnDrawing != null)
                OnDrawing.Invoke(this);

            float ratio = 2 * length / this.DirectVector.Length;
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2f(this.pointOfLine.X - ratio * this.DirectVector.deltaX, this.pointOfLine.Y - ratio * this.DirectVector.deltaY);
            Gl.glVertex2f(this.pointOfLine.X + ratio * this.DirectVector.deltaX, this.pointOfLine.Y + ratio * this.DirectVector.deltaY);
            Gl.glEnd();

            if (OnDrawed != null)
                OnDrawed.Invoke(this);
        }

        public void Draw(float length, float drawWidth)
        {
            float curWidth = base.getCurrentGlWidth();
            Gl.glLineWidth(drawWidth);
            this.Draw(length);
            Gl.glLineWidth(curWidth);
        }

        public void Draw(float length, GlColor drawColor)
        {
            GlColor curColor = base.getCurrentGlColor();

            if (drawColor != null)
                Gl.glColor3f(drawColor.R, drawColor.G, drawColor.B);

            this.Draw(length);
            Gl.glColor3f(curColor.R, curColor.G, curColor.B);
        }

        public void Draw(float length, float drawWidth, GlColor drawColor)
        {
            GlColor curColor = base.getCurrentGlColor();
            float curWidth = base.getCurrentGlWidth();

            Gl.glLineWidth(drawWidth);

            if (drawColor != null)
                Gl.glColor3f(drawColor.R, drawColor.G, drawColor.B);

            this.Draw(length);

            Gl.glColor3f(curColor.R, curColor.G, curColor.B);
            Gl.glLineWidth(curWidth);
        }

        public override void Draw(GlRectangle Border)
        {
            if (Border == null || !ActivateDrawStart())
                return;

            ActivateDrawing();

            GlPointR2[] I = this.getIntersection(Border);

            switch (I.Length)
            {
                case 0:
                    if (OnDrawed != null)
                        OnDrawed.Invoke(this);
                    return;
                case 1: new GlLineSegment(I[0], I[0]).Draw();
                    break;
                case 2: new GlLineSegment(I[0], I[1]).Draw();
                    break;
            }

            ActivateDrawed();
        }

        public override void DrawFill()
        {
            this.Draw();
        }

        public override void DrawFill(GlRectangle Border)
        {
            this.Draw(Border);
        }

        public override void Draw(GlTexture T)
        {
            if (!ActivateDrawStart())
                return;

            T.BindTexture();

            ActivateDrawing();

            Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2f(this.pointOfLine.X - this.DirectVector.deltaX, this.pointOfLine.Y - this.DirectVector.deltaY);
                Gl.glTexCoord2d(this.pointOfLine.X - this.DirectVector.deltaX, this.pointOfLine.Y - this.DirectVector.deltaY);

                Gl.glVertex2f(this.pointOfLine.X + this.DirectVector.deltaX, this.pointOfLine.Y + this.DirectVector.deltaY);
                Gl.glTexCoord2d(this.pointOfLine.X + this.DirectVector.deltaX, this.pointOfLine.Y + this.DirectVector.deltaY);
            Gl.glEnd();

            T.UnbindTexture();

            ActivateDrawed();
        }

        private bool ActivateDrawStart()
        {
            if (OnDrawStart == null)
                return true;
            return OnDrawStart.Invoke(this);
        }

        private void ActivateDrawing()
        {
            if (OnDrawing != null)
                OnDrawing.Invoke(this);
        }

        private void ActivateDrawed()
        {
            if (OnDrawed != null)
                OnDrawed.Invoke(this);
        }

        ////////////////DRAW_METHODS///////////////////
        ///////////////////////////////////////////////
        /////////////INSIDE_BELONGS_METHODS////////////

        public override bool isPointBelongs(GlPointR2 P)
        {
            return (P == null || P.isNullPoint() || this.isNullLine()) ? false : Math.Abs((P.X - this.pointOfLine.X) * this.DirectVector.deltaY - (P.Y - this.pointOfLine.Y) * this.DirectVector.deltaX) < FAULT;
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        public float getDistance(GlPointR2 P)
        {
            if (P == null || this.pointOfLine.isNullPoint() || P.isNullPoint())
                return float.NaN;

            if (GlPointR2.Equals(P, this.pointOfLine))
                return 0;

            if (this.DirectVector.isNullVector())
                return P.getDistance(this.PointOfLine);

            GlVectorR2 directVector = new GlVectorR2(this.DirectVector.deltaX, this.DirectVector.deltaY);
            return (new GlVectorR2(P.X - this.pointOfLine.X, P.Y - this.pointOfLine.Y) ^ directVector) / (float)directVector.Length;
        }

        public bool isNullLine()
        {
            return this.PointOfLine.isNullPoint() || this.DirectVector.isNullVector();
        }

        public static bool Equals(GlLineR2 L1, GlLineR2 L2)
        {
            return (L1 == null || L2 == null || L1.isNullLine() || L2.isNullLine()) ? false : GlVectorR2.isParallel(L1.DirectVector, L2.DirectVector) && GlVectorR2.isParallel(L1.DirectVector, new GlVectorR2(L2.pointOfLine.X - L1.pointOfLine.X, L2.pointOfLine.Y - L1.pointOfLine.Y));
        }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && (obj as GlLineR2).directVector.Equals(this.directVector) && (obj as GlLineR2).isPointBelongs(this.pointOfLine) && this.isPointBelongs((obj as GlLineR2).pointOfLine);
        }

        public override string ToString()
        {
            return this.pointOfLine.isNullPoint() ? "" : "(x - " + this.pointOfLine.X + ") / " + this.DirectVector.deltaX + " = " + "(y - " + this.pointOfLine.Y + ") / " + this.DirectVector.deltaY;
        }

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////EVENTS/////////////////////
        ///////////////////////////////////////////////

        public override event GlFigure.DrawingStart OnDrawStart;
        public override event GlFigure.Drawing OnDrawing;
        public override event GlFigure.Drawing OnDrawed;

        public override event GlFigure.RotationStart OnRotateStart;
        public override event GlFigure.Rotation OnRotating;
        public override event GlFigure.Rotation OnRotated;

        public override event GlFigure.MovingStart OnMoveStart;
        public override event GlFigure.Moving OnMoving;
        public override event GlFigure.Moving OnMoved;

        ///////////////////////////////////////////////
        ////////////////////EVENTS/////////////////////
        ///////////////////////////////////////////////
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using GDLL.Figures.AuxilaryItems;
using GDLL.Paint.Texturing;

namespace GDLL.Figures
{
    public abstract class GlCurve : GlFigure
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        protected GlPointR2 systemCenter;
        protected GlVectorR2 directVector;
        protected GlPointR2[] curvePoints;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        /// <returns>An embracing rectangle with sides parallel to the axises</returns>
        public override GlRectangle BOX
        {
            get
            {
                float Xmax = this[0].X, Xmin = this[0].X, Ymax = this[0].Y, Ymin = this[0].Y;
                for (int i = 0; i < CountOfPoints; i++)
                {
                    if (this[i].X > Xmax)
                        Xmax = this[i].X;
                    else if (this[i].X < Xmin)
                        Xmin = this[i].X;
                    if (this[i].Y > Ymax)
                        Ymax = this[i].Y;
                    else if (this[i].Y < Ymin)
                        Ymin = this[i].Y;
                }

                return new GlRectangle(new GlPointR2(Xmin, Ymax), Xmax - Xmin, Ymax - Ymin, new GlVectorR2(1, 0));
            }
        }

        /// <returns>An embracing rectangle</returns>
        public abstract GlRectangle RealBox { get; }

        public override int CountOfPoints { get { return curvePoints.Length; } }

        /// <returns>A point of the curve by its index</returns>
        public GlPointR2 this[int i] { get { return new GlPointR2(this.curvePoints[Math.Abs(i) % this.CountOfPoints]); } }

        /// <returns>Vector collinear the curve's system x-axis positive direction</returns>
        public GlVectorR2 DirectVector { get { return new GlVectorR2(directVector); } }

        /// <returns>Returns a point that is center of the oval</returns>
        public override GlPointR2 Center { get { return new GlPointR2(systemCenter); } }    

        public float SIN { get; protected set; }
        public float COS { get; protected set; }

        public float CenterX { get { return systemCenter.X; } set { this.moveTo(value, systemCenter.Y); } }
        public float CenterY { get { return systemCenter.Y; } set { this.moveTo(systemCenter.X, value); } }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        //////////////TRANSFORM_METHODS////////////////

        public override void moveTo(float x, float y)
        {
            float oldX = this.Center.X;
            float oldY = this.Center.Y;

            if (OnMoveStart != null)
                if (!OnMoveStart.Invoke(this, oldX, oldY))
                    return;

            if (OnMoving != null)
                OnMoving.Invoke(this, oldX, oldY);

            this.systemCenter = new GlPointR2(x, y);
            updatePointsPosition();

            if (OnMoved != null)
                OnMoved.Invoke(this, oldX, oldY);
        }

        public override void Rotate(float SIN, float COS)
        {
            float oldX = this.Center.X;
            float oldY = this.Center.Y;

            if (OnRotateStart != null)
                if (!OnRotateStart.Invoke(this, SIN, COS))
                    return;

            if (OnRotating != null)
                OnRotating.Invoke(this, SIN, COS);

            this.directVector = this.directVector.getRotatedVector(-SIN, COS);
            updatePointsPosition();

            if (OnRotated != null)
                OnRotated.Invoke(this, SIN, COS);
        }

        public override void Rotate(float angle)
        {
            this.Rotate((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        public override GlPointR2[] getIntersection(GlCurve C)
        {
            if (C == null || C.CountOfPoints == 0)
                return new GlPointR2[] { };

            List<GlPointR2> Intersections = new List<GlPointR2>();
            bool Side = this.isPointInside(C[0]);

            for (int i = 1; i < C.CountOfPoints; i++)
                if (this.isPointInside(C[i]) != Side)
                {
                    int next = (i == C.CountOfPoints - 1) ? 0 : i + 1;
                    int prev = i == 1 ? C.CountOfPoints - 1 : i - 2;
                    GlPointR2[] faultInter = this.getIntersection(new GlLineR2(C[i], new GlVectorR2(C[next].X - C[prev].X, C[next].Y - C[prev].Y)));

                    for (int k = 0; k < faultInter.Length; k++)
                        if (C.isPointBelongs(faultInter[k]))
                        {
                            Intersections.Add(faultInter[k]);
                            Side = !Side;
                        }
                }

            return Intersections.ToArray();
        }

        public override GlPointR2[] getIntersection(GlPolygon POLY)
        {
            if (POLY == null || POLY.CountOfPoints == 0)
                return new GlPointR2[] { };

            List<GlPointR2> Intersections = new List<GlPointR2>();
            GlPointR2[] faultInter = this.getIntersection(new GlLineR2(POLY[POLY.CountOfPoints - 1],
                                                        new GlVectorR2(POLY[0].X - POLY[POLY.CountOfPoints - 1].X,
                                                                     POLY[0].Y - POLY[POLY.CountOfPoints - 1].Y)));

            for (int i = 0; i < faultInter.Length; i++)
                if (new GlLineSegment(POLY[0], POLY[POLY.CountOfPoints - 1]).isPointBelongs(faultInter[i]))
                    Intersections.Add(faultInter[i]);

            for (int i = 0; i < POLY.CountOfPoints - 1; i++)
            {
                faultInter = this.getIntersection(new GlLineR2(POLY[i], new GlVectorR2(POLY[i + 1].X - POLY[i].X, POLY[i + 1].Y - POLY[i].Y)));

                for (int j = 0; j < faultInter.Length; j++)
                    if (new GlLineSegment(POLY[i + 1], POLY[i]).isPointBelongs(faultInter[j]))
                        Intersections.Add(faultInter[j]);
            }

            return Intersections.ToArray();
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        /////////////////DRAW_METHODS//////////////////

        public override void Draw() { DrawPoints(Gl.GL_LINE_STRIP); }
        public override void Draw(GlRectangle Border) { DrawPoints(Border, Gl.GL_LINE_STRIP); }

        public override void Draw(GlTexture T)
        {
            if (T == null || this.CountOfPoints == 0 || !ActivateDrawStart())
                return;

            T.BindTexture();

            ActivateDrawing();

            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 0; i < this.CountOfPoints; i++)
                if (this[i] != null)
                {
                    Gl.glVertex2f(this[i].X, this[i].Y);
                    Gl.glTexCoord2d((double)this[i].X, (double)this[i].Y);
                }
            Gl.glEnd();

            T.UnbindTexture();

            ActivateDrawed();
        }

        public override void DrawFill() { DrawPoints(Gl.GL_POLYGON); }
        public override void DrawFill(GlRectangle Border) { DrawPoints(Border, Gl.GL_POLYGON); }

        private void DrawPoints(GlRectangle Border, int GlDrawMode)
        {
            if (Border == null || !ActivateDrawStart())
                return;

            ActivateDrawing();

            bool isInside = Border.isPointInside(this[0]);

            GlPolygon ToDraw = new GlPolygon(this.Center);

            if (Border.isPointInside(this[this.CountOfPoints - 1]) != isInside)
            {
                if (!isInside)
                    ToDraw.AddVertex(this[this.CountOfPoints - 1]);

                GlPointR2[] I = new GlLineR2(this[0], new GlVectorR2(this[0].X - this[this.CountOfPoints - 1].X, 
                                                               this[0].Y - this[this.CountOfPoints - 1].Y)).getIntersection(Border);

                if (I.Length == 2)
                    ToDraw.AddVertex(new GlLineSegment(this[this.CountOfPoints - 1], this[0]).isPointBelongs(I[0]) ? I[0] : I[1]);
                else if (I.Length == 1 && new GlLineSegment(this[this.CountOfPoints - 1], this[0]).isPointBelongs(I[0]))
                    ToDraw.AddVertex(I[0]);
            }

            for (int i = 0; i < this.CountOfPoints - 1; i++)
            {
                if (isInside)
                    ToDraw.AddVertex(this[i]);
                if (Border.isPointInside(this[i + 1]) != isInside)
                {
                    GlPointR2[] I = new GlLineR2(this[i], new GlVectorR2(this[i + 1].X - this[i].X, 
                                                                   this[i + 1].Y - this[i].Y)).getIntersection(Border);

                    if (I.Length == 2)
                        ToDraw.AddVertex(new GlLineSegment(this[i], this[i + 1]).isPointBelongs(I[0]) ? I[0] : I[1]);
                    else if (I.Length == 1 && new GlLineSegment(this[i], this[i + 1]).isPointBelongs(I[0]))
                        ToDraw.AddVertex(I[0]);

                    isInside = !isInside;
                }
            }

            Gl.glBegin(GlDrawMode);
            for (int i = 0; i < ToDraw.CountOfPoints; i++)
                if (ToDraw[i] != null)
                    Gl.glVertex2f(ToDraw[i].X, ToDraw[i].Y);
            Gl.glEnd();

            ActivateDrawed();
        }

        private void DrawPoints(int GlDrawMode)
        {
            if (!ActivateDrawStart())
                return;

            ActivateDrawing();

            Gl.glBegin(GlDrawMode);
            for (int i = 0; i < this.CountOfPoints; i++)
                if (this[i] != null)
                    Gl.glVertex2f(this[i].X, this[i].Y);
            Gl.glEnd();

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

        /////////////////DRAW_METHODS//////////////////
        ///////////////////////////////////////////////
        ////////////////TANGENT_METHODS////////////////

        /// <returns>A tangent from a belongs point to the curve</returns>
        public abstract GlLineR2 getTangentFromBelongs(GlPointR2 P);

        /// <returns>A circle of curvature in a given point of curve</returns>
        public GlCircle getCurvatureCircle(GlPointR2 belongsPoint)
        {
            if (belongsPoint == null || belongsPoint.isNullPoint() || !this.isPointBelongs(belongsPoint))
                return new GlCircle(float.NaN, new GlPointR2(float.NaN, float.NaN));

            GlPointR2 P = belongsPoint.getPointTranslatedToRotatedSystem(SIN, COS, Center);

            float sDiff = getSDiff(P.X);

            if (sDiff == 0)
                return new GlCircle(float.NaN, new GlPointR2(float.NaN, float.NaN));

            float fDiff = getFDiff(P.X);
            float fDiffPow = (float)Math.Pow(fDiff, 2.0);
            
            return new GlCircle((float)(Math.Pow(1 + fDiffPow, 1.5) / Math.Abs(sDiff)), 
                              new GlPointR2(P.X - fDiff * (1 + fDiffPow) / sDiff, 
                                          P.Y + (P.Y < 0 ? -1 : 1) * (1 + fDiffPow) / sDiff).getTranslatedBackPoint(SIN, COS, Center));
        }

        ////////////////TANGENT_METHODS////////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        /// <returns>If the point inside the curve</returns>
        public abstract bool isPointInside(GlPointR2 P);

        /// <returns>Value of the first differencial at given X</returns>
        public abstract float getFDiff(float X);

        /// <returns>Value of the second differencial at given X</returns>
        public abstract float getSDiff(float X);

        /// <summary>
        /// should be called after some transformations
        /// </summary>
        protected abstract void updatePointsPosition();

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////////EVENTS////////////////////
        ///////////////////////////////////////////////

        /// <returns>Should return false to cancel drawing, true - to continue</returns>
        public override event GlFigure.DrawingStart OnDrawStart;
        public override event GlFigure.Drawing OnDrawing;
        public override event GlFigure.Drawing OnDrawed;

        /// <returns>Should return false to cancel rotation, true - to continue</returns>
        public override event GlFigure.RotationStart OnRotateStart;
        public override event GlFigure.Rotation OnRotating;
        public override event GlFigure.Rotation OnRotated;

        /// <returns>Should return false to cancel moving, true - to continue</returns>
        public override event GlFigure.MovingStart OnMoveStart;
        public override event GlFigure.Moving OnMoving;
        public override event GlFigure.Moving OnMoved;

        ///////////////////////////////////////////////
        /////////////////////EVENTS////////////////////
        ///////////////////////////////////////////////
    }
}

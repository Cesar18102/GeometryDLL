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

namespace GDLL.Figures
{
    public class GlPointR2 : GlFigure
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private float x;
        private float y;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        /// <param name="x">Initial x position</param>
        /// <param name="y">Initial y position</param>
        public GlPointR2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Copying constructor
        /// </summary>
        public GlPointR2(GlPointR2 copyPoint) : 
            this(copyPoint == null ? float.NaN : copyPoint.x, 
                 copyPoint == null ? float.NaN : copyPoint.Y) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public override int CountOfPoints { get { return 1; } }

        public float X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public float Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public override GlPointR2 Center { get { return new GlPointR2(this); } }

        public override GlRectangle BOX { get { return new GlSqaure(this, 0.01f, new GlVectorR2(1, 0)); } }

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
            if (OnMoveStart != null)
                if (!OnMoveStart.Invoke(this, this.x, this.y))
                    return;

            if (OnMoving != null)
                OnMoving.Invoke(this, this.x, this.y);

            float oldX = this.x, oldy = this.y;

            this.X = x;
            this.Y = y;

            if (OnMoved != null)
                OnMoved.Invoke(this, oldX, oldy);
        }

        /// <summary>
        /// Rotates the point around itself
        /// </summary>
        /// <param name="angle">Counter-wise rotation angle</param>
        public override void Rotate(float angle)
        {
            this.Rotate((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        /// <summary>
        /// Rotates the point around itself
        /// </summary>
        /// <param name="SIN">sin of counter-wise rotation angle</param>
        /// <param name="COS">cos of counter-wise rotation angle</param>
        public override void Rotate(float SIN, float COS)
        {
            if (OnRotateStart != null)
                if (!OnRotateStart.Invoke(this, SIN, COS))
                    return;

            if (OnRotating != null)
                OnRotating.Invoke(this, SIN, COS);

            if (OnRotated != null)
                OnRotated.Invoke(this, SIN, COS);

            return;
        }

        /// <summary>
        /// Determines the position of given point in a rotated and parallel-moved coordinate system
        /// </summary>
        /// <param name="P">Point to be translated</param>
        /// <param name="SIN">sin of a counter-wise angle of system rotation</param>
        /// <param name="COS">cos of a counter-wise angle of system rotation</param>
        /// <param name="systemCenter">Center of the system point is being tanslated to</param>
        /// <returns>Translated point</returns>
        public GlPointR2 getPointTranslatedToRotatedSystem(float SIN, float COS, GlPointR2 systemCenter)
        {
            GlPointR2 TranslatedPoint = new GlPointR2(this);
            TranslatedPoint.moveTo((TranslatedPoint.X - systemCenter.X) * COS + (TranslatedPoint.Y - systemCenter.Y) * SIN, -(TranslatedPoint.X - systemCenter.X) * SIN + (TranslatedPoint.Y - systemCenter.Y) * COS);
            return TranslatedPoint;
        }

        /// <summary>
        /// Determines the position of given point in a standard coordinate system
        /// </summary>
        /// <param name="P">Point to be translated back</param>
        /// <param name="SIN">sin of a counter-wise angle of system rotation</param>
        /// <param name="COS">cos of a counter-wise angle of system rotation</param>
        /// <param name="systemCenter">Center of the system point was tanslated to</param>
        /// <returns>Translated back point</returns>
        public GlPointR2 getTranslatedBackPoint(float SIN, float COS, GlPointR2 systemCenter)
        {
            GlPointR2 TranslatedPoint = new GlPointR2(this);
            TranslatedPoint.moveTo(TranslatedPoint.X * COS - TranslatedPoint.Y * SIN + systemCenter.X, TranslatedPoint.X * SIN + TranslatedPoint.Y * COS + systemCenter.Y);
            return TranslatedPoint;
        }

        /// <returns>A copy of given point</returns>
        public override GlFigure getScaled(float scale)
        {
            return new GlPointR2(this);
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        /// <summary>
        /// Determines the intersection of a point and a line
        /// </summary>
        /// <returns>An array containing a copy of given point if it belongs to the line</returns>
        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            return L == null ? new GlPointR2[] { } : L.getIntersection(this);
        }

        /// <summary>
        /// Determines the intersection of a point and a parabola
        /// </summary>
        /// <returns>An array containing a copy of given point if it belongs to the parabola</returns>
        public override GlPointR2[] getIntersection(GlCurve C)
        {
            return C == null ? new GlPointR2[] { } : C.getIntersection(this);
        }

        /// <summary>
        /// Determines the intersection of a point and a polygon
        /// </summary>
        /// <returns>An array containing a copy of given point if it belongs to the polygon</returns>
        public override GlPointR2[] getIntersection(GlPolygon POLY)
        {
            return POLY == null ? new GlPointR2[] { } : POLY.getIntersection(this);
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        /////////////INSIDE_BELONGS_METHODS////////////

        /// <summary>
        /// Determines the point belongs to another point
        /// </summary>
        /// <returns>If points are equal</returns>
        public override bool isPointBelongs(GlPointR2 P)
        {
            return this.Equals(P);
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ////////////////DRAW_METHODS///////////////////

        /// <summary>
        /// Draws the point
        /// </summary>
        public override void Draw()
        {
            if (this.isNullPoint() || !ActivateDrawStart())
                return;

            ActivateDrawing();

            Gl.glBegin(Gl.GL_POINTS);
                Gl.glVertex2f(this.X, this.Y);
            Gl.glEnd();

            ActivateDrawed();
        }

        /// <summary>
        /// Draw the point if it is inside the border rectangle
        /// </summary>
        public override void Draw(GlRectangle Border)
        {
            if (Border == null || !Border.isPointInside(this))
                return;

            this.Draw();
        }

        public override void Draw(GlTexture T)
        {
            if (this.isNullPoint() || !ActivateDrawStart())
                return;

            T.BindTexture();

            ActivateDrawing();

            Gl.glBegin(Gl.GL_POINTS);
            Gl.glTexCoord2d(this.X, this.Y);
            Gl.glVertex2f(this.X, this.Y);
            Gl.glEnd();

            T.UnbindTexture();

            ActivateDrawed();
        }

        /// <summary>
        /// Draws the point
        /// </summary>
        public override void DrawFill()
        {
            this.Draw();
        }

        /// <summary>
        /// Draw the point if it is inside the border rectangle
        /// </summary>
        public override void DrawFill(GlRectangle Border)
        {
            this.Draw(Border);
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
        ///////////////ADDITIONAL_METHODS//////////////

        /// <summary>
        /// Determines the distance between given points
        /// </summary>
        public float getDistance(GlPointR2 P)
        {
            return (this == null || P == null || this.isNullPoint() || P.isNullPoint()) ? float.NaN : (float)Math.Sqrt(Math.Pow(this.X - P.X, 2.0) + Math.Pow(this.Y - P.Y, 2.0));
        }

        /// <summary>
        /// Checks if the point has defined coordinates
        /// </summary>
        public bool isNullPoint()
        {
            return float.IsNaN(this.X) || float.IsNaN(this.Y);
        }

        /// <summary>
        /// Checks if given points are equal
        /// </summary>
        public static bool Equals(GlPointR2 P1, GlPointR2 P2)
        {
            return (P1 == null || P2 == null || P1.isNullPoint() || P2.isNullPoint()) ? false : P1.X == P2.X && P1.Y == P2.Y;
        }

        /// <summary>
        /// Checks if given points are equal
        /// </summary>
        public bool Equals(GlPointR2 P)
        {
            return (P == null || this.isNullPoint() || P.isNullPoint()) ? false : this.X == P.X && this.Y == P.Y;
        }

        /// <summary>
        /// Checks if given points are equal
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && (obj as GlPointR2).x == this.x && (obj as GlPointR2).y == this.y;
        }

        public override string ToString()
        {
            return this.isNullPoint() ? "" : "(" + this.X + "; " + this.Y + ")";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using GDLL.Paint.Coloring;
using GDLL.Paint.Texturing;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures
{
    public abstract class GlFigure
    {
        ///////////////////////////////////////////////
        ///////////////////PROPERTIES//////////////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Count of points the figure can be drawed with
        /// </summary>
        public abstract int CountOfPoints { get; }

        /// <summary>
        /// Rectangle region of figure with sides parallel to the axises
        /// </summary>
        public abstract GlRectangle BOX { get; }

        /// <summary>
        /// Geometrical center of the figure, a point the figure can be rotated around
        /// </summary>
        public abstract GlPointR2 Center { get; }

        ///////////////////////////////////////////////
        ///////////////////PROPERTIES//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////////METHODS///////////////////
        ///////////////////////////////////////////////

        ////////////////TRANSFORM_METHODS//////////////

        /// <summary>
        /// Sets the coordinates of the figure
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public abstract void moveTo(float x, float y);

        /// <summary>
        /// Rotates the figure around its center
        /// </summary>
        /// <param name="angle">Conter-wise rotation angle</param>
        public abstract void Rotate(float angle);

        /// <summary>
        /// Rotates the figure around its center
        /// </summary>
        /// <param name="SIN">sin of a conter-wise rotation angle</param>
        /// <param name="COS">cos of a conter-wise rotation angle</param>
        public abstract void Rotate(float SIN, float COS);

        /// <returns>Scaled figure</returns>
        public abstract GlFigure getScaled(float scale);

        ////////////////TRANSFORM_METHODS//////////////
        ///////////////////////////////////////////////
        ///////////////INTERSECTION_METHODS////////////

        /// <summary>
        /// Determines an anmount of points, created by intersection of a figure and a point
        /// </summary>
        /// <returns>An array of intersections</returns>
        public GlPointR2[] getIntersection(GlPointR2 P)
        {
            return (P == null || P.isNullPoint() || !this.isPointBelongs(P)) ? new GlPointR2[] { } : new GlPointR2[] { P };
        }

        /// <summary>
        /// Determines an anmount of points, created by intersection of a figure and a line
        /// </summary>
        /// <returns>An array of intersections</returns>
        public abstract GlPointR2[] getIntersection(GlLineR2 L);

        /// <summary>
        /// Determines an anmount of points, created by intersection of a figure and an oval
        /// </summary>
        /// <returns>An array of intersections</returns>
        public abstract GlPointR2[] getIntersection(GlCurve C);

        /// <summary>
        /// Determines an anmount of points, created by intersection of a figure and a polygon
        /// </summary>
        /// <returns>An array of intersections</returns>
        public abstract GlPointR2[] getIntersection(GlPolygon POLY);

        /// <summary>
        /// Determines an anmount of points, created by intersection of a figure and a figure system
        /// </summary>
        /// <returns>An array of intersections</returns>
        public GlPointR2[] getIntersection(GlFigureSystem FS)
        {
            if (FS == null)
                return new GlPointR2[] { };

            List<GlPointR2> I = new List<GlPointR2>();
            for(int i = 0; i < FS.CountOfFigures; i++)
                foreach (GlPointR2 P in this.getIntersection(FS[i]))
                    I.Add(P);
            return I.ToArray();
        }

        public GlPointR2[] getIntersection(GlFigure F)
        {
            if (F == null)
                return new GlPointR2[] { };

            try { return this.getIntersection(F as GlCurve); } catch {
                try { return this.getIntersection(F as GlPolygon); } catch {
                    try { return this.getIntersection(F as GlLineR2); } catch {
                        try { return this.getIntersection(F as GlFigureSystem); } catch {
                            try { return this.getIntersection(F as GlPointR2); } catch { 
                                return new GlPointR2[] { }; 
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a point belongs to the figure
        /// </summary>
        public abstract bool isPointBelongs(GlPointR2 P);

        ///////////////INTERSECTION_METHODS////////////
        ///////////////////////////////////////////////
        ///////////////////DRAW_METHODS////////////////

        /// <summary>
        /// Draws the figure with openGl
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Draws the figure filled with openGl
        /// </summary>
        public abstract void DrawFill();

        public abstract void Draw(GlTexture T);

        /// <summary>
        /// Draws a part of the figure that is inside the border rectangle
        /// </summary>
        /// <param name="Border">border rectangle</param>
        public abstract void Draw(GlRectangle Border);

        /// <summary>
        /// Draws filled a part of the figure that is inside the border rectangle
        /// </summary>
        /// <param name="Border">border rectangle</param>
        public abstract void DrawFill(GlRectangle Border);

        /// <summary>
        /// Draws the figure with given color. Changes Gl drawing color and then rolls it back. If drawColor is equal to null draws the figure with current drawing color.
        /// </summary>
        public void Draw(GlColor drawColor)
        {
            GlColor curColor = this.getCurrentGlColor();

            if (drawColor != null)
                Gl.glColor3f(drawColor.R, drawColor.G, drawColor.B);

            this.Draw();
            Gl.glColor3f(curColor.R, curColor.G, curColor.B);
        }

        /// <summary>
        /// Draws the figure with given line width. Changes Gl line width and then rolls it back
        /// </summary>
        public void Draw(float drawWidth)
        {
            float curWidth = this.getCurrentGlWidth();
            Gl.glLineWidth(drawWidth);
            this.Draw();
            Gl.glLineWidth(curWidth);
        }

        public void Draw(GlColor drawColor, float drawWidth)
        {
            float curWidth = this.getCurrentGlWidth();
            Gl.glLineWidth(drawWidth);
            this.Draw(drawColor);
            Gl.glLineWidth(curWidth);
        }

        public void DrawFill(GlColor drawColor)
        {
            GlColor curColor = this.getCurrentGlColor();

            if (drawColor != null)
                Gl.glColor3f(drawColor.R, drawColor.G, drawColor.B);

            this.DrawFill();
            Gl.glColor3f(curColor.R, curColor.G, curColor.B); ;
        }

        public void Draw(GlRectangle Border, GlColor drawColor)
        {
            GlColor curColor = this.getCurrentGlColor();

            if (drawColor != null)
                Gl.glColor3f(drawColor.R, drawColor.G, drawColor.B);

            this.Draw(Border);
            Gl.glColor3f(curColor.R, curColor.G, curColor.B);
        }

        public void Draw(GlRectangle Border, float drawWidth)
        {
            float curWidth = this.getCurrentGlWidth();
            Gl.glLineWidth(drawWidth);
            this.Draw(Border);
            Gl.glLineWidth(curWidth);
        }

        public void Draw(GlRectangle Border, GlColor drawColor, float drawWidth)
        {
            float curWidth = this.getCurrentGlWidth();
            Gl.glLineWidth(drawWidth);
            this.Draw(Border, drawColor);
            Gl.glLineWidth(curWidth);
        }

        public void DrawFill(GlRectangle Border, GlColor drawColor)
        {
            GlColor curColor = this.getCurrentGlColor();

            if (drawColor != null)
                Gl.glColor3f(drawColor.R, drawColor.G, drawColor.B);

            this.DrawFill(Border);
            Gl.glColor3f(curColor.R, curColor.G, curColor.B); ;
        }

        ///////////////////DRAW_METHODS////////////////
        ///////////////////////////////////////////////
        ////////////////ADDITIONAL_METHODS/////////////

        /// <summary>
        /// Gets current Gl drawing color
        /// </summary>
        protected GlColor getCurrentGlColor()
        {
            float[] curColor = new float[3];
            Gl.glGetFloatv(Gl.GL_CURRENT_COLOR, curColor);
            return new GlColor(curColor[0], curColor[1], curColor[2]);
        }

        /// <summary>
        /// Gets current Gl line width
        /// </summary>
        protected float getCurrentGlWidth()
        {
            float[] curWidth = new float[3];
            Gl.glGetFloatv(Gl.GL_LINE_WIDTH, curWidth);
            return curWidth[0];
        }

        /// <returns>A copy of the figure</returns>
        public GlFigure getCopy()
        {
            return (GlFigure)this.GetType().GetConstructor(new Type[] { this.GetType() }).Invoke(new object[] { this });
        }

        ////////////////ADDITIONAL_METHODS/////////////

        ///////////////////////////////////////////////
        /////////////////////METHODS///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ///////////////////DELEGATES///////////////////
        ///////////////////////////////////////////////

        public delegate bool RotationStart(GlFigure sender, float SIN, float COS);
        public delegate bool MovingStart(GlFigure sender, float oldX, float oldY);
        public delegate bool DrawingStart(GlFigure sender);

        public delegate void Rotation(GlFigure sender, float SIN, float COS);
        public delegate void Moving(GlFigure sender, float oldX, float oldY);
        public delegate void Drawing(GlFigure sender);

        ///////////////////////////////////////////////
        ///////////////////DELEGATES///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////////EVENTS////////////////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Figure sender - unrotated figure; Return false to cancel rotation;
        /// </summary>
        public abstract event RotationStart OnRotateStart;

        /// <summary>
        /// Figure sender - unrotated figure
        /// </summary>
        public abstract event Rotation OnRotating;

        /// <summary>
        /// Figure sender - rotated figure
        /// </summary>
        public abstract event Rotation OnRotated;

        /// <summary>
        /// Figure sender - unmoved figure; Return false to cancel moving;
        /// </summary>
        public abstract event MovingStart OnMoveStart;

        /// <summary>
        /// Figure sender - unmoved figure
        /// </summary>
        public abstract event Moving OnMoving;

        /// <summary>
        /// Figure sender - moved figure
        /// </summary>
        public abstract event Moving OnMoved;

        /// <summary>
        /// Figure sender - undrawed figure; Return false to cancel drawing;
        /// </summary>
        public abstract event DrawingStart OnDrawStart;

        /// <summary>
        /// Figure sender - undrawed figure
        /// </summary>
        public abstract event Drawing OnDrawing;

        /// <summary>
        /// Figure sender - drawed figure
        /// </summary>
        public abstract event Drawing OnDrawed;

        ///////////////////////////////////////////////
        /////////////////////EVENTS////////////////////
        ///////////////////////////////////////////////
    }
}

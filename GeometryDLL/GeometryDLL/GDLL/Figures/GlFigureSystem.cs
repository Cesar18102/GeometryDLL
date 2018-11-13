using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures.AuxilaryItems;
using GDLL.Paint.Texturing;

namespace GDLL.Figures
{
    public class GlFigureSystem : GlFigure
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private GlFigure[] figuresAmount;
        private GlPointR2 systemCenter;
        private int countOfPoints = 0;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlFigureSystem(params GlFigure[] figures)
        {
            if (figures == null)
            {
                figuresAmount = new GlFigure[0];
                systemCenter = new GlPointR2(float.NaN, float.NaN);
                return;
            }

            figuresAmount = new GlFigure[figures.Length];
            Array.Copy(figures, figuresAmount, figures.Length);

            float X = 0, Y = 0;

            foreach (GlFigure F in figuresAmount)
            {
                countOfPoints += F.CountOfPoints;
                X += F.Center.X;
                Y += F.Center.Y;
            }

            this.systemCenter = new GlPointR2(X / figuresAmount.Length, Y / figuresAmount.Length);
        }

        public GlFigureSystem(GlFigureSystem copyFigureSystem) : 
            this(copyFigureSystem == null ? new GlFigure[] { } : copyFigureSystem.figuresAmount) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public int CountOfFigures { get { return figuresAmount.Length; } }
        public GlFigure this[int i] { get { return CountOfFigures != 0 ? figuresAmount[Math.Abs(i) % CountOfFigures] : null; } }

        public override int CountOfPoints { get { return countOfPoints; } }

        /// <returns>AVG point of figures centers</returns>
        public override GlPointR2 Center { get { return this.systemCenter; } }

        /// <returns>An embracing rectangle</returns>
        public override GlRectangle BOX
        {
            get
            {
                if (CountOfFigures == 0)
                    return new GlRectangle(new GlPointR2(float.NaN, float.NaN), 0, 0, new GlVectorR2(0, 0));

                float maxX = figuresAmount[0].BOX[0].X, minX = figuresAmount[0].BOX[0].X, maxY = figuresAmount[0].BOX[0].Y, minY = figuresAmount[0].BOX[0].Y;
                foreach (GlFigure F in figuresAmount)
                {
                    GlRectangle FBOX = F.BOX;
                    for (int i = 0; i < FBOX.CountOfPoints; i++)
                    {
                        if (FBOX[i].X > maxX)
                            maxX = FBOX[i].X;
                        else if (FBOX[i].X < minX)
                            minX = FBOX[i].X;

                        if (FBOX[i].Y > maxY)
                            maxY = FBOX[i].Y;
                        else if (FBOX[i].Y < minY)
                            minY = FBOX[i].Y;
                    }
                }
                return new GlRectangle(new GlPointR2(minX, maxY), maxX - minX, maxY - minY, new GlVectorR2(1, 0));
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

        public override void moveTo(float x, float y)
        {
            float oldX = systemCenter.X, oldY = systemCenter.Y;

            if (OnMoveStart != null)
                if (!OnMoveStart.Invoke(this, oldX, oldY))
                    return;

            if (OnMoving != null)
                OnMoving.Invoke(this, oldX, oldY);

            GlVectorR2 moveVector = new GlVectorR2(x - this.systemCenter.X, y - this.systemCenter.Y);
            this.systemCenter = new GlPointR2(x, y);

            foreach (GlFigure F in this.figuresAmount)
            {
                GlPointR2 newCenter = moveVector.fromPointToPoint(F.Center);
                F.moveTo(newCenter.X, newCenter.Y);
            }

            if (OnMoved != null)
                OnMoving.Invoke(this, oldX, oldY);
        }

        public override void Rotate(float SIN, float COS)
        {
            if (OnRotateStart != null)
                if (!OnRotateStart.Invoke(this, SIN, COS))
                    return;

            if (OnRotating != null)
                OnRotating.Invoke(this, SIN, COS);

            foreach (GlFigure F in this.figuresAmount)
                F.Rotate(SIN, COS);

            if (OnRotated != null)
                OnRotated.Invoke(this, SIN, COS);
        }

        public override void Rotate(float angle)
        {
            this.Rotate((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        public override GlFigure getScaled(float scale)
        {
            GlFigureSystem FS = new GlFigureSystem();
            foreach (GlFigure F in figuresAmount)
                FS.AddFigure(F.getScaled(scale));
            return FS;
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        ////////////INTERSECTION_METHODS///////////////

        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            return L == null ? new GlPointR2[] { } : L.getIntersection(this);
        }

        public override GlPointR2[] getIntersection(GlCurve O)
        {
            return O == null ? new GlPointR2[] { } : O.getIntersection(this);
        }

        public override GlPointR2[] getIntersection(GlPolygon POLY)
        {
            return POLY == null ? new GlPointR2[] { } : POLY.getIntersection(this);
        }

        ////////////INTERSECTION_METHODS///////////////
        ///////////////////////////////////////////////
        /////////////INSIDE_BELONGS_METHODS////////////

        public override bool isPointBelongs(GlPointR2 P)
        {
            foreach (GlFigure F in figuresAmount)
                if (F.isPointBelongs(P))
                    return true;
            return false;
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ////////////////DRAW_METHODS///////////////////

        public override void Draw()
        {
            this.DrawFigures((F) => { F.Draw(); });
        }

        public override void Draw(GlRectangle Border)
        {
            if (Border == null)
                return;

            this.DrawFigures((F) => { F.Draw(Border); });
        }

        public override void Draw(GlTexture T)
        {
            if (T == null || !ActivateDrawStart())
                return;

            ActivateDrawing();

            foreach (GlFigure F in this.figuresAmount)
                F.Draw(T);

            ActivateDrawed();
        }

        public override void DrawFill()
        {
            this.DrawFigures((F) => { F.DrawFill(); });
        }

        public override void DrawFill(GlRectangle Border)
        {
            this.DrawFigures((F) => { F.DrawFill(Border); });
        }

        private void DrawFigures(FigureDraw DrawMethod)
        {
            if (!ActivateDrawStart())
                return;

            ActivateDrawing();

            foreach (GlFigure F in this.figuresAmount)
                DrawMethod.Invoke(F);

            ActivateDrawed();
        }

        private void DrawFigures(FigureDrawBorder DrawMethod, GlRectangle Border)
        {
            if (!ActivateDrawStart())
                return;

            ActivateDrawing();

            foreach (GlFigure F in this.figuresAmount)
                DrawMethod.Invoke(F, Border);

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
        /////////////ADDITIONAL_METHODS////////////////

        public void AddFigure(GlFigure F)
        {
            if (F == null)
                return;

            if (OnFigureAddingStart != null)
                if (!OnFigureAddingStart.Invoke(F, this))
                    return;

            if (OnFigureAdding != null)
                OnFigureAdding.Invoke(F, this);

            GlFigure[] copy = this.figuresAmount;
            this.figuresAmount = new GlFigure[this.figuresAmount.Length + 1];
            Array.Copy(copy, this.figuresAmount, copy.Length);
            this.figuresAmount[this.figuresAmount.Length - 1] = F;

            float X = 0, Y = 0;
            foreach (GlFigure FG in figuresAmount)
            {
                countOfPoints += F.CountOfPoints;
                X += F.Center.X;
                Y += F.Center.Y;
            }

            this.systemCenter = new GlPointR2(X / figuresAmount.Length, Y / figuresAmount.Length);

            if (OnFigureAdded != null)
                OnFigureAdded.Invoke(F, this);
        }

        public override bool Equals(object obj)
        {
            foreach (GlFigure F in figuresAmount)
                if (!F.Equals(obj))
                    return false;
            return true;
        }

        public override string ToString()
        {
            string info = "";

            foreach (GlFigure F in figuresAmount)
                info += F.ToString() + Environment.NewLine;

            return info;
        }

        /////////////ADDITIONAL_METHODS////////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        
        /*********************************************/


        ///////////////////////////////////////////////
        ///////////////////DELEGATES///////////////////
        ///////////////////////////////////////////////

        public delegate bool FigureAddStart(GlFigure ToAdd, GlFigure sender);
        public delegate void FigureAdd(GlFigure Added, GlFigure sender);

        public delegate void FigureDraw(GlFigure sender);
        public delegate void FigureDrawBorder(GlFigure sender, GlRectangle Border);

        ///////////////////////////////////////////////
        ///////////////////DELEGATES///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////EVENTS/////////////////////
        ///////////////////////////////////////////////

        public override event GlFigure.DrawingStart OnDrawStart;
        public override event GlFigure.Drawing OnDrawing;
        public override event GlFigure.Drawing OnDrawed;

        public override event GlFigure.MovingStart OnMoveStart;
        public override event GlFigure.Moving OnMoving;
        public override event GlFigure.Moving OnMoved;

        public override event GlFigure.RotationStart OnRotateStart;
        public override event GlFigure.Rotation OnRotating;
        public override event GlFigure.Rotation OnRotated;

        public event FigureAddStart OnFigureAddingStart;
        public event FigureAdd OnFigureAdding;
        public event FigureAdd OnFigureAdded;

        ///////////////////////////////////////////////
        ////////////////////EVENTS/////////////////////
        ///////////////////////////////////////////////
    }
}

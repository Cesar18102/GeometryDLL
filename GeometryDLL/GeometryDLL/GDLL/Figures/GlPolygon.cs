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
    public class GlPolygon : GlFigure
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.01f;

        private GlPointR2[] vertexes;
        private GlPointR2 polyCenter;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlPolygon(GlPointR2 Center, params GlPointR2[] POLY)
        {
            S = 0;

            if (POLY == null || Center == null)
            {
                polyCenter = new GlPointR2(float.NaN, float.NaN);
                vertexes = new GlPointR2[0];

                return;
            }
            
            if (POLY.Length > 3)
            {
                GlPointR2 P = new GlPointR2((POLY[0].X + POLY[POLY.Length / 2].X) / 2, (POLY[0].Y + POLY[POLY.Length / 2].Y) / 2);
                for (int i = 0; i < POLY.Length - 1; i++)
                    S += new GlTriangle(P, POLY[i], POLY[i + 1]).S;
                S += new GlTriangle(P, POLY[POLY.Length - 1], POLY[0]).S;
            }

            this.polyCenter = Center;

            this.vertexes = new GlPointR2[POLY.Length];
            Array.Copy(POLY, this.vertexes, this.vertexes.Length);

            if (this.vertexes.Length == 0)
                return;

            for (int i = 0; i < this.vertexes.Length - 1; i++)
                P += new GlVectorR2(this.vertexes[i + 1].X - this.vertexes[i].X, this.vertexes[i + 1].Y - this.vertexes[i].Y).Length;
            P += new GlVectorR2(this.vertexes[0].X - this.vertexes[this.vertexes.Length - 1].X, this.vertexes[0].Y - this.vertexes[this.vertexes.Length - 1].Y).Length;
        }

        public GlPolygon(GlPolygon copyPolygon) :
            this(copyPolygon == null ? new GlPointR2(float.NaN, float.NaN) : copyPolygon.polyCenter,
                 copyPolygon == null ? new GlPointR2[] { } : copyPolygon.vertexes) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public override int CountOfPoints { get { return vertexes.Length; } }

        public GlPointR2 this[int i]
        {
            get { return new GlPointR2(this.vertexes[Math.Abs(i) % this.vertexes.Length]); }
            set { if (value != null && !value.isNullPoint()) vertexes[i] = value; }
        }

        public float P { get; protected set; }
        public float S { get; protected set; }

        public override GlPointR2 Center { get { return new GlPointR2(polyCenter); } }

        public float CenterX { get { return polyCenter.X; } set { this.moveTo(value, polyCenter.Y); } }
        public float CenterY { get { return polyCenter.Y; } set { this.moveTo(polyCenter.X, value); } }

        public override GlRectangle BOX
        {
            get
            {
                float Xmax = this[0].X, Xmin = this[0].X, Ymax = this[0].Y, Ymin = this[0].Y;
                foreach (GlPointR2 i in this.vertexes)
                {
                    if (i.X > Xmax)
                        Xmax = i.X;
                    else if (i.X < Xmin)
                        Xmin = i.X;
                    if (i.Y > Ymax)
                        Ymax = i.Y;
                    else if (i.Y < Ymin)
                        Ymin = i.Y;
                }

                return new GlRectangle(new GlPointR2(Xmin, Ymax), Xmax - Xmin, Ymax - Ymin, new GlVectorR2(1, 0));
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
            if (OnMoveStart != null)
                if (!OnMoveStart.Invoke(this, this.polyCenter.X, this.polyCenter.Y))
                    return;

            if (OnMoving != null)
                OnMoving.Invoke(this, this.polyCenter.X, this.polyCenter.Y);

            GlVectorR2 delta = new GlVectorR2(x - polyCenter.X, y - polyCenter.Y);
            for (int i = 0; i < vertexes.Length; i++)
                vertexes[i] = delta.fromPointToPoint(vertexes[i]);

            GlPointR2 OP = new GlPointR2(this.polyCenter);
            this.polyCenter = new GlPointR2(x, y);

            if (OnMoved != null)
                OnMoved.Invoke(this, OP.X, OP.Y);
        }

        public override void Rotate(float SIN, float COS)
        {
            if (OnRotateStart != null)
                if (!OnRotateStart.Invoke(this, SIN, COS))
                    return;

            if (OnRotating != null)
                OnRotating.Invoke(this, SIN, COS);

            for (int i = 0; i < vertexes.Length; i++)
                vertexes[i] = new GlVectorR2(vertexes[i].X - polyCenter.X, vertexes[i].Y - polyCenter.Y).getRotatedVector(-SIN, COS).fromPointToPoint(polyCenter);

            if (OnRotated != null)
                OnRotated.Invoke(this, SIN, COS);
        }

        public override void Rotate(float angle)
        {
            this.Rotate((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        public override GlFigure getScaled(float scale)
        {
            GlPointR2 C = new GlPointR2(this.Center);
            C.moveTo(C.X * scale, C.Y * scale);

            GlPolygon PR = new GlPolygon(C);
            for (int i = 0; i < this.CountOfPoints; i++)
            {
                GlPointR2 P = this[i];
                P.moveTo(P.X * scale, P.Y * scale);
                PR.AddVertex(new GlPointR2(P));
            }

            PR.moveTo(this.Center.X, this.Center.Y);
            return PR;
        }

        //////////////TRANSFORM_METHODS////////////////
        ///////////////////////////////////////////////
        /////////////INTERSECTION_METHODS//////////////

        public override GlPointR2[] getIntersection(GlLineR2 L)
        {
            return L == null ? new GlPointR2[] { } : L.getIntersection(this);
        }

        public override GlPointR2[] getIntersection(GlCurve C)
        {
            return C == null ? new GlPointR2[] { } : C.getIntersection(this);
        }

        public override GlPointR2[] getIntersection(GlPolygon POLY)
        {
            List<GlPointR2> Intersections = new List<GlPointR2>();

            GlLineR2 fCurLine = new GlLineR2(this[this.CountOfPoints - 1], new GlVectorR2(this[0].X - this[this.CountOfPoints - 1].X, this[0].Y - this[this.CountOfPoints - 1].Y));
            GlPointR2[] faultInter = fCurLine.getIntersection(POLY);
            foreach (GlPointR2 j in faultInter)
                if (new GlLineSegment(this[this.CountOfPoints - 1], this[0]).isPointBelongs(j))
                    Intersections.Add(j);

            for (int i = 0; i < this.CountOfPoints - 1; i++)
            {
                fCurLine = new GlLineR2(this[i], new GlVectorR2(this[i + 1].X - this[i].X, this[i + 1].Y - this[i].Y));
                faultInter = fCurLine.getIntersection(POLY);
                foreach (GlPointR2 j in faultInter)
                    if (new GlLineSegment(this[i], this[i + 1]).isPointBelongs(j))
                        Intersections.Add(j);
            }

            return Intersections.ToArray();
        }

        /////////////INTERSECTION_METHODS//////////////
        ///////////////////////////////////////////////
        ////////////////DRAW_METHODS///////////////////

        public override void Draw() { DrawPoints(Gl.GL_LINE_LOOP); }
        public override void Draw(GlRectangle Border) { DrawPoints(Border, Gl.GL_LINE_LOOP); }

        public override void Draw(GlTexture T)
        {
            if (this.vertexes == null || !ActivateDrawStart())
                return;

            T.BindTexture();

            ActivateDrawing();

            Gl.glBegin(Gl.GL_POLYGON);
            for (int i = 0; i < this.vertexes.Length; i++)
                if (this.vertexes[i] != null)
                {
                    Gl.glVertex2f(this.vertexes[i].X, this.vertexes[i].Y);
                    Gl.glTexCoord2f(this.vertexes[i].X, this.vertexes[i].Y);
                }
            Gl.glEnd();

            T.UnbindTexture();

            ActivateDrawed();
        }

        public override void DrawFill() { DrawPoints(Gl.GL_POLYGON); }
        public override void DrawFill(GlRectangle Border) { DrawPoints(Border, Gl.GL_POLYGON); }

        private void DrawPoints(int GlDrawMode)
        {
            if (this.vertexes == null || !ActivateDrawStart()) 
                return;

            ActivateDrawing();

            Gl.glBegin(GlDrawMode);
            for (int i = 0; i < this.vertexes.Length; i++)
                if (this.vertexes[i] != null)
                    Gl.glVertex2f(this.vertexes[i].X, this.vertexes[i].Y);
            Gl.glEnd();

            ActivateDrawed();
        }

        private void DrawPoints(GlRectangle Border, int GlDrawMode)
        {
            if (!ActivateDrawStart())
                return;

            ActivateDrawing();

            bool isInside = Border.isPointInside(this.vertexes[0]);

            GlPolygon ToDraw = new GlPolygon(this.Center);

            if (Border.isPointInside(this.vertexes[this.CountOfPoints - 1]) != isInside)
            {
                if (!isInside)
                    ToDraw.AddVertex(this.vertexes[this.CountOfPoints - 1]);

                GlPointR2[] I = new GlLineR2(this.vertexes[0], new GlVectorR2(this.vertexes[0].X - this.vertexes[this.CountOfPoints - 1].X, this.vertexes[0].Y - this.vertexes[this.CountOfPoints - 1].Y)).getIntersection(Border);

                if (I.Length == 2)
                    ToDraw.AddVertex(new GlLineSegment(this.vertexes[this.CountOfPoints - 1], this.vertexes[0]).isPointBelongs(I[0]) ? I[0] : I[1]);
                else if (I.Length == 1 && new GlLineSegment(this.vertexes[this.CountOfPoints - 1], this.vertexes[0]).isPointBelongs(I[0]))
                    ToDraw.AddVertex(I[0]);
            }

            for (int i = 0; i < this.CountOfPoints - 1; i++)
            {
                if (isInside)
                    ToDraw.AddVertex(this.vertexes[i]);
                if (Border.isPointInside(this[i + 1]) != isInside)
                {
                    GlPointR2[] I = new GlLineR2(this.vertexes[i], new GlVectorR2(this.vertexes[i + 1].X - this.vertexes[i].X, this.vertexes[i + 1].Y - this.vertexes[i].Y)).getIntersection(Border);

                    if (I.Length == 2)
                        ToDraw.AddVertex(new GlLineSegment(this.vertexes[i], this.vertexes[i + 1]).isPointBelongs(I[0]) ? I[0] : I[1]);
                    else if (I.Length == 1 && new GlLineSegment(this.vertexes[i], this.vertexes[i + 1]).isPointBelongs(I[0]))
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

        public bool isPointInside(GlPointR2 P)
        {
            float SP = 0;
            for (int i = 0; i < this.CountOfPoints - 1; i++)
                SP += new GlTriangle(P, this.vertexes[i], this.vertexes[i + 1]).S;
            SP += new GlTriangle(P, this.vertexes[this.CountOfPoints - 1], this.vertexes[0]).S;

            return Math.Abs(SP - S) < FAULT;
        }

        public override bool isPointBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return false;

            if (new GlLineSegment(vertexes[this.CountOfPoints - 1], vertexes[0]).isPointBelongs(P))
                return true;

            for (int i = 0; i < this.CountOfPoints - 1; i++)
                if (new GlLineSegment(vertexes[i], vertexes[i + 1]).isPointBelongs(P))
                    return true;

            return false;
        }

        /////////////INSIDE_BELONGS_METHODS////////////
        ///////////////////////////////////////////////
        ///////////////ADDITIONAL_METHODS//////////////

        public virtual void AddVertex(GlPointR2 P)
        {
            if (P == null || P.isNullPoint())
                return;

            if (OnVertexAddingStart != null)
                if (!OnVertexAddingStart.Invoke(P))
                    return;

            if (OnVertexAdding != null)
                OnVertexAdding.Invoke(P);

            if (polyCenter == null)
                polyCenter = P;

            GlPointR2[] copy = this.vertexes;
            this.vertexes = new GlPointR2[this.vertexes.Length + 1];
            Array.Copy(copy, this.vertexes, copy.Length);
            this.vertexes[this.vertexes.Length - 1] = P;

            if (OnVertexAdded != null)
                OnVertexAdded.Invoke(P);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(this.GetType()) && (obj as GlPolygon).polyCenter.Equals(this.polyCenter) && (obj as GlPolygon).CountOfPoints == this.CountOfPoints)
            {
                List<GlPointR2> PS = new List<GlPointR2>(this.vertexes);

                foreach (GlPointR2 i in (obj as GlPolygon).vertexes)
                    foreach (GlPointR2 j in PS)
                        if (i.Equals(j))
                        {
                            PS.Remove(i);
                            break;
                        }
                return PS.Count == 0;
            }
            return false;
        }

        public override string ToString()
        {
            string info = "";
            foreach (GlPointR2 i in this.vertexes)
                info += i.ToString() + ";";
            return info;
        }

        ///////////////ADDITIONAL_METHODS//////////////

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ///////////////////DELEGATES///////////////////
        ///////////////////////////////////////////////

        public delegate bool VertexAddStart(GlPointR2 P);
        public delegate void VertexAdd(GlPointR2 P);

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

        public override event GlFigure.RotationStart OnRotateStart;
        public override event GlFigure.Rotation OnRotating;
        public override event GlFigure.Rotation OnRotated;

        public override event GlFigure.MovingStart OnMoveStart;
        public override event GlFigure.Moving OnMoving;
        public override event GlFigure.Moving OnMoved;

        public event VertexAddStart OnVertexAddingStart;
        public event VertexAdd OnVertexAdding;
        public event VertexAdd OnVertexAdded;

        ///////////////////////////////////////////////
        ////////////////////EVENTS/////////////////////
        ///////////////////////////////////////////////        
    }
}

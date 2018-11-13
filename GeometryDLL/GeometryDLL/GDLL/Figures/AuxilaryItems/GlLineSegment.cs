using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures.AuxilaryItems
{
    class GlLineSegment : GlVectorR2
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const float FAULT = 0.07f;

        private GlPointR2 startPoint;
        private GlPointR2 endPoint;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlLineSegment(GlPointR2 startPoint, GlPointR2 endPoint)
            : base(new GlVectorR2(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y))
        {
            if (startPoint == null || endPoint == null)
            {
                startPoint = new GlPointR2(float.NaN, float.NaN);
                endPoint = new GlPointR2(float.NaN, float.NaN);
                return;
            }

            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public GlPointR2 StartPoint { get { return new GlPointR2(startPoint); } set { if (value != null && !value.isNullPoint()) { startPoint = value; endPoint = base.fromPointToPoint(startPoint); } } }
        public GlPointR2 EndPoint { get { return new GlPointR2(endPoint); } set { if (value != null && !value.isNullPoint()) { endPoint = value; startPoint = base.getReversedVector().fromPointToPoint(endPoint); } } }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        public void Draw()
        {
            Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2f(this.startPoint.X, this.startPoint.Y);
                Gl.glVertex2f(this.endPoint.X, this.endPoint.Y);
            Gl.glEnd();
        }

        public bool isPointBelongs(GlPointR2 P)
        {
            if (P == null || P.isNullPoint() || !new GlLineR2(startPoint, new GlVectorR2(base.deltaX, base.deltaY)).isPointBelongs(P))
                return false;

            return Math.Abs(new GlVectorR2(P.X - startPoint.X, P.Y - startPoint.Y).Length + new GlVectorR2(EndPoint.X - P.X, EndPoint.Y - P.Y).Length - new GlVectorR2(EndPoint.X - startPoint.X, EndPoint.Y - startPoint.Y).Length) < FAULT;
        }

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

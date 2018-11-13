using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using GDLL.Interfaces.AuxilaryItems;
using GDLL.Paint.Coloring;

namespace GDLL.Interfaces
{
    public abstract class GlContext
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        protected Action InitGraphicsMeth;
        protected Action RedisplayMeth;
        protected Action<int, int> ReshapeMeth;

        protected int width;
        protected int height;

        protected int x;
        protected int y;

        protected string name;

        protected int FPS;

        protected const float defLineWidth = 1.0f;
        protected float curLineWidth;

        protected volatile GlWinDrawColor defColor;
        protected GlWinDrawColor curDrawColor;

        protected Observer windowObserver;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public GlWinDrawColor DrawingColor
        {
            get { return new GlWinDrawColor(curDrawColor); }
            set
            {
                if (value == null)
                    return;
                curDrawColor.R = value.R;
                curDrawColor.G = value.G;
                curDrawColor.B = value.B;
            }
        }

        public GlColor DefColor
        {
            get { return new GlColor(defColor); }
        }

        public float LineDrawingWidth
        {
            get { return curLineWidth; }
            set { curLineWidth = Math.Abs(value); Gl.glLineWidth(curLineWidth); }
        }

        public Observer WindowObserver { get { return windowObserver; } }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        public abstract void InitGlSetting();
        public abstract void RenderStart();
        public abstract void InitGraphSetting();
        public abstract void TimerFunc(int time);
        public abstract void WindowReshape(int width, int height);
        public abstract void DrawFunc();

        protected void UpdateLookerPosition()
        {
            Glu.gluLookAt(windowObserver.EyeX, windowObserver.EyeY, windowObserver.EyeZ, windowObserver.LookToX, windowObserver.LookToY, windowObserver.LookToZ, windowObserver.UpToHeadVectorX, windowObserver.UpToHeadVectorY, windowObserver.UoToHeadVectorZ);
        }

        public void setBackgroundColor(GlColor C)
        {
            Gl.glClearColor(C.R, C.G, C.B, 1);//set window background
        }

        public void setDrawingColor(GlColor newColor)
        {
            if (newColor == null)
                return;

            this.DrawingColor = newColor as GlWinDrawColor;
        }

        public void setDrawingColor(short R, short G, short B)
        {
            this.DrawingColor = new GlWinDrawColor(R, G, B);
        }

        public void setDrawingColor(float R, float G, float B)
        {
            this.DrawingColor = new GlWinDrawColor(R, G, B);
        }

        public void setDefColor()
        {
            this.curDrawColor = defColor;
        }

        public void setDefLineWidth()
        {
            this.curLineWidth = defLineWidth;
        }

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

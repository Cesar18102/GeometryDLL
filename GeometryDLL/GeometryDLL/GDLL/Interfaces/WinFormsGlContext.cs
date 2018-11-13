using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;
using Tao.Platform.Windows;
using System.Windows.Forms;
using GDLL.Interfaces.AuxilaryItems;
using GDLL.Paint.Coloring;

namespace GDLL.Interfaces
{
    public class WinFormsGlContext : GlContext
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private SimpleOpenGlControl DrawContext;
        private Form ParentForm;
        private Timer T;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public WinFormsGlContext(Timer T, Form FatherForm, SimpleOpenGlControl drawContext, Action graphicsInit, Action drawMeth, Action<int, int> reshapeMeth, int FPS)
        {
            drawContext.InitializeContexts();

            this.width = drawContext.Width;
            this.height = drawContext.Height;
            this.x = drawContext.Left;
            this.y = drawContext.Top;
            this.name = drawContext.Name;
            this.DrawContext = drawContext;
            this.ParentForm = FatherForm;
            this.T = T;
            this.FPS = FPS;

            T.Interval = (int)(1000f / FPS);
            T.Tick += new System.EventHandler(WinFormsTimerFunc);

            this.windowObserver = new Observer(0, 0, 5, 0, 0, 0, 0, 1, 0);//setting looker

            this.InitGraphicsMeth = graphicsInit;
            this.RedisplayMeth = drawMeth;
            this.ReshapeMeth = reshapeMeth;


            this.defColor = new GlWinDrawColor(0.0f, 0.0f, 0.0f);

            this.setDefColor();
            this.setDefLineWidth();

            this.ParentForm.Resize += this.WinFormsReshapeFunc;
            this.InitGraphicsMeth();

            this.InitGlSetting();
        }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        public override void InitGlSetting()
        {
            int size = Math.Min(DrawContext.Width, DrawContext.Height);

            Glut.glutInit();//initialize glut
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            Gl.glClearColor(255, 255, 255, 1);
            Gl.glViewport(0, 0, size, size); // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Glu.gluPerspective((360 * Math.Atan(size / 2) / Math.PI) % 90, 1, 1, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
        }

        public override void InitGraphSetting()
        {
            Gl.glLoadIdentity();
            this.InitGraphicsMeth();
        }

        public override void RenderStart()
        {
            T.Start();
        }

        public override void DrawFunc()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            UpdateLookerPosition();

            Gl.glPushMatrix();
            Gl.glTranslated(0, 0, -5);

            this.RedisplayMeth();

            Gl.glPopMatrix();
            Gl.glFlush();
            DrawContext.Invalidate();
        }

        public void WinFormsTimerFunc(object sender, EventArgs e)
        {
            TimerFunc((int)(sender as Timer).Interval);
        }

        public override void TimerFunc(int time)
        {
            DrawFunc();
        }

        private void WinFormsReshapeFunc(object sender, EventArgs e)
        {
            WindowReshape((sender as Form).Width, (sender as Form).Height);
        }

        public override void WindowReshape(int width, int height)
        {
            int size = Math.Min(DrawContext.Width, DrawContext.Height);
            Gl.glViewport(0, 0, size, size); // активация проекционной матрицы 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective((360 * Math.Atan(size / 2) / Math.PI) % 90, 1, 1, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity(); // настройка параметров OpenGL для визуализации Gl.glEnable(Gl.GL_DEPTH_TEST);

            this.ReshapeMeth(width, height);
        }
        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

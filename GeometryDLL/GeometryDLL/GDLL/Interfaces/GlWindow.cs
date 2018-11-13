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
    public class GlWindow : GlContext
    {
        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlWindow(int Width, int Height, int PositionX, int PositionY, string Name, Action graphicsInit, Action drawMeth, Action<int, int> reshapeMeth, int FPS)
        {
            this.width = Width;
            this.height = Height;

            this.x = PositionX;
            this.y = PositionY;

            this.name = Name;
            this.FPS = FPS;

            this.windowObserver = new Observer(0, 0, 5, 0, 0, 0, 0, 1, 0);//setting looker

            this.InitGraphicsMeth = graphicsInit;
            this.RedisplayMeth = drawMeth;
            this.ReshapeMeth = reshapeMeth;

            this.defColor = new GlWinDrawColor(0.0f, 0.0f, 0.0f);

            this.setDefColor();
            this.setDefLineWidth();

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
            Glut.glutInit();
            Glut.glutInitWindowSize(this.width, this.height);//setting window size
            Glut.glutInitWindowPosition(this.x, this.y);//setting window position
            Glut.glutCreateWindow(this.name);//creating a window with some name
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            this.InitGraphSetting();

            Glut.glutDisplayFunc(this.DrawFunc);//rendering method setting
            Glut.glutTimerFunc(1000 / FPS, this.TimerFunc, 0);//SET SPEED OF RENDERING HERE
            Glut.glutReshapeFunc(this.WindowReshape);//on window reshape method setting
        }

        public override void InitGraphSetting()
        {
            Gl.glLoadIdentity();
            this.InitGraphicsMeth();
        }

        public override void RenderStart()
        {
            Glut.glutMainLoop();
        }

        public override void DrawFunc()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            UpdateLookerPosition();

            this.RedisplayMeth();

            Glut.glutSwapBuffers();
        }

        public override void TimerFunc(int time)
        {
            Gl.glLoadIdentity();
            Glut.glutPostRedisplay();
            Glut.glutTimerFunc(1000 / FPS, this.TimerFunc, 0);
        }

        public override void WindowReshape(int width, int height)
        {
            int size = Math.Min(width, height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, size, size);
            Glu.gluPerspective((360 * Math.Atan(size / 2) / Math.PI) % 90, 1, 1, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            this.ReshapeMeth(width, height);
        }

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

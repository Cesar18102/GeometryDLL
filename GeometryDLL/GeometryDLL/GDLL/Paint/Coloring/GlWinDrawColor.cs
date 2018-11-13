using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform;

namespace GDLL.Paint.Coloring
{
    public class GlWinDrawColor : GlColor
    {
        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlWinDrawColor(short R, short G, short B) : base(R, G, B) { }
        public GlWinDrawColor(float R, float G, float B) : base(R, G, B) { }
        public GlWinDrawColor(GlWinDrawColor copyColor) : base(copyColor) { }
        public GlWinDrawColor(GlColor copyColor) : base(copyColor) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public new float R
        {
            get { return Rv; }
            set { float vR = Validate(value); Rv = vR; Gl.glColor3f(Rv, Gv, Bv); }
        }

        public new float G
        {
            get { return Gv; }
            set { float vG = Validate(value); Gv = vG; Gl.glColor3f(Rv, Gv, Bv); }
        }

        public new float B
        {
            get { return Bv; }
            set { float vB = Validate(value); Bv = vB; Gl.glColor3f(Rv, Gv, Bv); }
        }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////
    }
}

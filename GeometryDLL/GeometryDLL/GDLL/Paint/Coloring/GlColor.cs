using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDLL.Paint.Coloring
{
    public class GlColor
    {
        ///////////////////////////////////////////////
        /////////////////////FIELDS////////////////////
        ///////////////////////////////////////////////

        protected float Rv;
        protected float Gv;
        protected float Bv;

        ///////////////////////////////////////////////
        /////////////////////FIELDS////////////////////
        ///////////////////////////////////////////////

        /*********************************************/

        ///////////////////////////////////////////////
        //////////////////CONSTRUCTORS/////////////////
        ///////////////////////////////////////////////

        public GlColor(short R, short G, short B)
        {
            this.setColor(R, G, B);
        }

        public GlColor(float R, float G, float B)
        {
            this.setColor(R, G, B);
        }

        public GlColor(GlColor copyColor)
        {
            if (copyColor == null)
                this.setColor(float.NaN, float.NaN, float.NaN);
            else
                this.setColor(copyColor.R, copyColor.G, copyColor.B);
        }

        public GlColor(GlWinDrawColor copyColor) : this(copyColor as GlColor) { }

        ///////////////////////////////////////////////
        //////////////////CONSTRUCTORS/////////////////
        ///////////////////////////////////////////////

        /*********************************************/

        ///////////////////////////////////////////////
        ///////////////////PROPERTIES//////////////////
        ///////////////////////////////////////////////

        public float R
        {
            get { return this.Rv; }
            set { this.Rv = Validate(value); }
        }

        public float G
        {
            get { return this.Gv; }
            set { this.Gv = Validate(value); }
        }

        public float B
        {
            get { return this.Bv; }
            set { this.Bv = Validate(value); }
        }

        ///////////////////////////////////////////////
        ///////////////////PROPERTIES//////////////////
        ///////////////////////////////////////////////

        /*********************************************/

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        public void setColor(short R, short G, short B)
        {
            this.R = (float)R / 255.0f;
            this.G = (float)G / 255.0f;
            this.B = (float)B / 255.0f;
        }

        public void setColor(float R, float G, float B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public void invertColor()
        {
            this.R = 1.0f - this.R;
            this.G = 1.0f - this.G;
            this.B = 1.0f - this.B;
        }

        public static float Validate(float value)
        {
            return (Math.Abs(value) % 1.0f == 0 && value != 0) ? 1.0f : Math.Abs(value) % 1.0f;
        }

        public static GlColor Validate(GlColor color)
        {
            return new GlColor(Validate(color.R), Validate(color.G), Validate(color.B));
        }

        public static bool Equals(GlColor C1, GlColor C2)
        {
            return C1.R == C2.R && C1.G == C2.G && C1.B == C2.B;
        }

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

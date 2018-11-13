using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures
{
    public class GlCircle : GlOval
    {
        ///////////////////////////////////////////////
        ///////////////////CONSTRUCTORS////////////////
        ///////////////////////////////////////////////

        public GlCircle(float Rad, GlPointR2 center) : base(Rad, Rad, new GlVectorR2(Rad, 0), center) { }

        public GlCircle(GlCircle copyCircle) : 
            this(copyCircle == null ? 0 : copyCircle.RadA, 
                 copyCircle == null ? new GlPointR2(float.NaN, float.NaN) : new GlPointR2(copyCircle.CenterX, copyCircle.CenterY)) { }

        ///////////////////////////////////////////////
        ///////////////////CONSTRUCTORS////////////////
        ///////////////////////////////////////////////

        /*********************************************/

        ///////////////////////////////////////////////
        ////////////////////PROPERTIES/////////////////
        ///////////////////////////////////////////////

        public override float RadA
        {
            get { return base.RadA; }
            set { base.RadA = value; base.RadB = value; }
        }

        public override float RadB
        {
            get { return base.RadB; }
            set { base.RadA = value; base.RadB = value; }
        }

        ///////////////////////////////////////////////
        ////////////////////PROPERTIES/////////////////
        ///////////////////////////////////////////////

        /*********************************************/

        ///////////////////////////////////////////////
        /////////////////////METHODS///////////////////
        ///////////////////////////////////////////////

        public override string ToString()
        {
            return "(x - " + CenterX + ")^2 + (y - " + CenterY + ") = " + RadA + "^2";
        }

        ///////////////////////////////////////////////
        /////////////////////METHODS///////////////////
        ///////////////////////////////////////////////
    }
}

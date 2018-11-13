using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures
{
    public class GlTriangle : GlPolygon
    {
        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlTriangle(GlPointR2 P1, GlPointR2 P2, GlPointR2 P3) : 
            base((P1 == null || P2 == null || P3 == null)? 
                    new GlPointR2(float.NaN, float.NaN) : 
                    new GlPointR2((P1.X + P2.X + P3.X) / 3, (P1.Y + P2.Y + P3.Y) / 3), 
                new GlPointR2[] { P1, P2, P3 }) 
        {
            this.S = Math.Abs((P1.X - P3.X) * (P2.Y - P3.Y) - (P1.Y - P3.Y) * (P2.X - P3.X)) / 2;
        }

        public GlTriangle(GlTriangle copyTiangle) :
            this(copyTiangle == null ? new GlPointR2(float.NaN, float.NaN) : copyTiangle[0],
                 copyTiangle == null ? new GlPointR2(float.NaN, float.NaN) : copyTiangle[1],
                 copyTiangle == null ? new GlPointR2(float.NaN, float.NaN) : copyTiangle[2]) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////
        

        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        public override void AddVertex(GlPointR2 P)
        {
            return;
        }

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures
{
    public class GlSqaure : GlRectangle
    {
        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlSqaure(GlPointR2 PLT, float sideLength, GlVectorR2 directVector) : base(PLT, sideLength, sideLength, directVector) { }

        public GlSqaure(GlSqaure copySqare) :
            this(copySqare == null ? new GlPointR2(float.NaN, float.NaN) : copySqare.LeftTopPoint,
                 copySqare == null ? 0 : copySqare.Width,
                 copySqare == null ? new GlVectorR2(0, 0) : copySqare.DirectVector) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////
    }
}

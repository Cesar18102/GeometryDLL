using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures;
using GDLL.Figures.AuxilaryItems;
using GDLL.Paint.Coloring;

namespace GDLL.Constants
{
    /// <summary>
    /// Constant values class
    /// </summary>
    public static class GlConstants
    {
        ///////////////////////////////////////////////
        /////////////////////ANGLES////////////////////
        ///////////////////////////////////////////////

        public const float DEG_RAD_0 = 0;

        public const float DEG_RAD_1 = (float)Math.PI / 180;
        public const float DEG_RAD_2 = (float)Math.PI / 90;
        public const float DEG_RAD_4 = (float)Math.PI / 45;
        public const float DEG_RAD_5 = (float)Math.PI / 36;
        public const float DEG_RAD_10 = (float)Math.PI / 18;
        public const float DEG_RAD_15 = (float)Math.PI / 12;
        public const float DEG_RAD_20 = (float)Math.PI / 9;

        public const float DEG_RAD_30 = (float)Math.PI / 6;
        public const float DEG_RAD_45 = (float)Math.PI / 4;
        public const float DEG_RAD_60 = (float)Math.PI / 3;
        public const float DEG_RAD_90 = (float)Math.PI / 2;

        public const float DEG_RAD_120 = (float)Math.PI * 2 / 3;
        public const float DEG_RAD_135 = (float)Math.PI * 3 / 4;
        public const float DEG_RAD_150 = (float)Math.PI * 5 / 6;

        public const float DEG_RAD_180 = (float)Math.PI;

        public const float DEG_RAD_210 = -(float)Math.PI * 5 / 6;
        public const float DEG_RAD_225 = -(float)Math.PI * 3 / 4;
        public const float DEG_RAD_240 = -(float)Math.PI * 2 / 3;

        public const float DEG_RAD_270 = -(float)Math.PI / 2;
        public const float DEG_RAD_300 = -(float)Math.PI / 3;
        public const float DEG_RAD_315 = -(float)Math.PI / 4;
        public const float DEG_RAD_330 = -(float)Math.PI / 6;

        public const float DEG_RAD_360 = 0;

        ///////////////////////////////////////////////
        /////////////////////ANGLES////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////////COLORS////////////////////
        ///////////////////////////////////////////////

        public static readonly GlColor WHITE = new GlColor(255, 255, 255);
        public static readonly GlColor SILVER = new GlColor(192, 192, 192);
        public static readonly GlColor GREY = new GlColor(128, 128, 128);
        public static readonly GlColor BLACK = new GlColor(0, 0, 0);

        public static readonly GlColor YELLOW = new GlColor(255, 255, 0);
        public static readonly GlColor OLIVE = new GlColor(128, 128, 0);

        public static readonly GlColor MAGNETTA = new GlColor(255, 0, 255);
        public static readonly GlColor PURPLE = new GlColor(128, 0, 128);

        public static readonly GlColor CYAN = new GlColor(0, 255, 255);
        public static readonly GlColor TEAL = new GlColor(0, 128, 128);

        public static readonly GlColor RED = new GlColor(255, 0, 0);
        public static readonly GlColor MAROON = new GlColor(128, 0, 0);

        public static readonly GlColor LIME = new GlColor(0, 255, 0);
        public static readonly GlColor GREEN = new GlColor(0, 128, 0);

        public static readonly GlColor BLUE = new GlColor(0, 0, 255);
        public static readonly GlColor NAVY = new GlColor(0, 0, 128);


        public static readonly GlColor KRAJOL_PERIWINKLE = new GlColor(197, 208, 230);
        public static readonly GlColor FRIGHTENED_NYMPH_HIPS = new GlColor(250, 208, 231);
        public static readonly GlColor BEIGE = new GlColor(245, 245, 220);
        public static readonly GlColor WHITE_ANTIQUE = new GlColor(250, 235, 215);
        public static readonly GlColor PALE_PINK = new GlColor(250, 218, 221);
        public static readonly GlColor GAINSBOROUGH = new GlColor(220, 220, 220);
        public static readonly GlColor SMOKE_WHITE = new GlColor(245, 245, 245);
        public static readonly GlColor PEARL_WHITE = new GlColor(234, 230, 202);
        public static readonly GlColor GREENISH_WHITE = new GlColor(245, 230, 203);
        //public static readonly GlColor KRAJOL_SPRING_GREEN = new GlColor(236, 234, 190);



        ///////////////////////////////////////////////
        /////////////////////COLORS////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////////FIGURES///////////////////
        ///////////////////////////////////////////////

        public static readonly Type FIGURE = typeof(GlFigure);
        public static readonly Type POINT = typeof(GlPointR2);
        public static readonly Type LINE = typeof(GlLineR2);

        public static readonly Type CURVE = typeof(GlCurve);
        public static readonly Type OVAL = typeof(GlOval);
        public static readonly Type CIRCLE = typeof(GlCircle);
        public static readonly Type PARABOLA = typeof(GlParabola);
        public static readonly Type HYPERBOLA = typeof(GlHyperbola);

        public static readonly Type POLYGON = typeof(GlPolygon);
        public static readonly Type TRIANGLE = typeof(GlTriangle);
        public static readonly Type RECTANGLE = typeof(GlRectangle);
        public static readonly Type SQUARE = typeof(GlSqaure);
        public static readonly Type FIGURE_SYSTEM = typeof(GlFigureSystem);

        public static readonly Type VECTOR = typeof(GlVectorR2);
        public static readonly Type LINE_SEGMENT = typeof(GlLineSegment);

        public static readonly Type[] CERTAIN_FIGURES = new Type[] { POINT, LINE, OVAL, CIRCLE, PARABOLA, HYPERBOLA, POLYGON, TRIANGLE, RECTANGLE, SQUARE, FIGURE_SYSTEM, VECTOR };
        public static readonly System.Reflection.ConstructorInfo[] CONSTRUCT_FIGURE = new System.Reflection.ConstructorInfo[] { 
                                                                     POINT.GetConstructor(new Type[] { typeof(float), typeof(float) }), 
                                                                     LINE.GetConstructor(new Type[] { POINT, VECTOR }), 
                                                                     OVAL.GetConstructor(new Type[] { typeof(float), typeof(float), VECTOR, POINT}), 
                                                                     CIRCLE.GetConstructor(new Type[] { typeof(float), POINT }), 
                                                                     PARABOLA.GetConstructor(new Type[] { typeof(float), POINT, VECTOR }), 
                                                                     HYPERBOLA.GetConstructor(new Type[] { typeof(float), typeof(float), VECTOR, POINT }), 
                                                                     POLYGON.GetConstructor(new Type[] { POINT, POINT.MakeArrayType() }), 
                                                                     TRIANGLE.GetConstructor(new Type[] { POINT,POINT,POINT }), 
                                                                     RECTANGLE.GetConstructor(new Type[] { POINT, typeof(float), typeof(float), VECTOR }), 
                                                                     SQUARE.GetConstructor(new Type[] { POINT, typeof(float), VECTOR }), 
                                                                     FIGURE_SYSTEM.GetConstructor(new Type[] { FIGURE.MakeArrayType()}),
                                                                     VECTOR.GetConstructor(new Type[]{ typeof(float), typeof(float) })
        };

        ///////////////////////////////////////////////
        /////////////////////FIGURES///////////////////
        ///////////////////////////////////////////////
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDLL.Figures.AuxilaryItems;

namespace GDLL.Figures
{
    public class GlRectangle : GlPolygon
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private GlVectorR2 directVector;
        private GlPointR2 leftTopPoint;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlRectangle(GlPointR2 PLT, float width, float height, GlVectorR2 directVector)
            : base((PLT == null || directVector == null)? 
                        new GlPointR2(float.NaN, float.NaN) :
                        new GlPointR2(PLT.X + (height * directVector.deltaY + width * directVector.deltaX) / (2 * directVector.Length), 
                                    PLT.Y + (width * directVector.deltaY - height * directVector.deltaX) / (2 * directVector.Length)),
                    (PLT == null || directVector == null)? 
                        new GlPointR2[]{new GlPointR2(float.NaN, float.NaN), new GlPointR2(float.NaN, float.NaN),
                                      new GlPointR2(float.NaN, float.NaN),new GlPointR2(float.NaN, float.NaN)} : 
                        new GlPointR2[] { 
                            PLT, new GlPointR2(PLT.X + width * directVector.deltaX / directVector.Length, 
                                             PLT.Y + width * directVector.deltaY / directVector.Length),
                            new GlLineR2(new GlPointR2(PLT.X + height * directVector.deltaY / directVector.Length, 
                                                   PLT.Y - height * directVector.deltaX / directVector.Length), directVector)
                            .getIntersection(new GlLineR2(new GlPointR2(PLT.X + width * directVector.deltaX / directVector.Length, 
                                                                    PLT.Y + width * directVector.deltaY / directVector.Length), 
                                                        directVector.getRotatedVector((float)Math.PI / 2)))[0],
                            new GlPointR2(PLT.X + height * directVector.deltaY / directVector.Length, 
                                        PLT.Y - height * directVector.deltaX / directVector.Length) 
                }
            )
        {
            if (PLT == null || directVector == null)
            {
                this.directVector = new GlVectorR2(0, 0);
                this.leftTopPoint = new GlPointR2(float.NaN, float.NaN);
                this.Width = 0;
                this.Height = 0;
                this.S = 0;
                this.P = 0;
            }
            this.directVector = directVector;
            this.Width = width;
            this.Height = height;
            this.leftTopPoint = PLT;

            this.S = width * height;
        }

        public GlRectangle(GlRectangle copyRectangle) :
            this(copyRectangle == null ? new GlPointR2(float.NaN, float.NaN) : copyRectangle.leftTopPoint,
                 copyRectangle == null ? 0 : copyRectangle.Width,
                 copyRectangle == null ? 0 : copyRectangle.Height,
                 copyRectangle == null ? new GlVectorR2(0, 0) : copyRectangle.directVector) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public GlPointR2 LeftTopPoint { get { return new GlPointR2(leftTopPoint); } }
        public GlVectorR2 DirectVector { get { return new GlVectorR2(directVector); } }

        public float Width { get; private set; }
        public float Height { get; private set; }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
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

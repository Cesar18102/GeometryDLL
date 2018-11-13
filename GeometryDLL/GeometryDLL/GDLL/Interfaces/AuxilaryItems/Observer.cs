using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDLL.Interfaces.AuxilaryItems
{
    public class Observer
    {
        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public Observer(float ObserverX, float ObserverY, float ObserverZ, float LookToX, float LookToY, float LookToZ, float HeadUpX, float HeadUpY, float HeadUpZ)
        {
            this.EyeX = ObserverX;
            this.EyeY = ObserverY;
            this.EyeZ = ObserverZ;

            this.LookToX = LookToX;
            this.LookToY = LookToY;
            this.LookToZ = LookToZ;

            this.UpToHeadVectorX = HeadUpX;
            this.UpToHeadVectorY = HeadUpY;
            this.UoToHeadVectorZ = HeadUpZ;
        }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        /*********************************************/

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////

        public float EyeX { get; set; }
        public float EyeY { get; set; }
        public float EyeZ { get; set; }

        public float LookToX { get;set; }
        public float LookToY { get; set; }
        public float LookToZ { get; set; }

        public float UpToHeadVectorX { get; set; }
        public float UpToHeadVectorY { get; set; }
        public float UoToHeadVectorZ { get; set; }

        ///////////////////////////////////////////////
        //////////////////PROPERTIES///////////////////
        ///////////////////////////////////////////////
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.Platform;

namespace GDLL.Paint.Texturing
{
    public class GlTexture
    {
        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////

        private const int FC_TextureCount = 1;
        private int[] Textures = new int[FC_TextureCount];

        private Bitmap textureIMG;
        private BitmapData bitmapData;

        ///////////////////////////////////////////////
        ////////////////////FIELDS/////////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////

        public GlTexture(Bitmap bmp)
        {
            this.textureIMG = bmp;
        }

        public GlTexture(string filename) : this(new Bitmap(filename)) { }

        ///////////////////////////////////////////////
        /////////////////CONSTRUCTORS//////////////////
        ///////////////////////////////////////////////


        /*********************************************/


        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////

        public void BindTexture()
        {
            Gl.glGenTextures(FC_TextureCount, Textures);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures[0]);

            this.bitmapData = this.textureIMG.LockBits(new System.Drawing.Rectangle(0, 0, textureIMG.Width, textureIMG.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, bitmapData.Width, bitmapData.Height, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
            this.textureIMG.UnlockBits(this.bitmapData);

            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, Textures[0]);
        }

        public void UnbindTexture()
        {
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
        }

        ///////////////////////////////////////////////
        ////////////////////METHODS////////////////////
        ///////////////////////////////////////////////
    }
}

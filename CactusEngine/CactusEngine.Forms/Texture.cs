using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Texture
    {
        public Texture(string assetPath)
        {
            _assetPath = assetPath;
        }

        public Bitmap Bitmap
        {
            get { return _bitmap; }
        }

        public void Initialize()
        {
            _bitmap = new Bitmap(_assetPath);
        }

        public void Shutdown()
        {
            _bitmap = null;
        }

        public Bitmap Rotate(double angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(_bitmap.Width, _bitmap.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)_bitmap.Width / 2, (float)_bitmap.Height / 2);
                //rotate
                g.RotateTransform((float)(angle * (180/Math.PI)));
                //move image back
                g.TranslateTransform(-(float)_bitmap.Width / 2, -(float)_bitmap.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(_bitmap, new Point(0, 0));
            }
            return returnBitmap;
        }

        private Bitmap _bitmap;
        private string _assetPath;
    }
}

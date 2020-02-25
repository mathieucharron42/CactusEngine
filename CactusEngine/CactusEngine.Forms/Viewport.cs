using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine
{
    public class Viewport
    {
        public delegate void ViewportMouseClick(MouseEventArgs e);
        public event ViewportMouseClick OnMouseClick;

        public delegate void ViewportPaint(PaintEventArgs e);
        public event ViewportPaint OnPaint;

        public Vector2 Size
        {
            get { return new Vector2(_panel.Size.Width, _panel.Size.Height); }
        }

        public Viewport(GameForm form)
        {
            _panel = form.GetViewport();
            _panel.MouseClick += OnPanelMouseClick;
            _panel.Paint += OnPanelPaint;
        }

        public void Invalidate()
        {
            _panel.Invalidate();
        }

        private void OnPanelMouseClick(object sender, MouseEventArgs e)
        {
            if(OnMouseClick != null)
            {
                OnMouseClick(e);
            }
        }

        private void OnPanelPaint(object sender, PaintEventArgs e)
        {
            if (OnPaint != null)
            {
                OnPaint(e);
            }
        }

        private Panel _panel;
    }
}

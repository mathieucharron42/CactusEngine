using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

namespace GameEngine
{
    public class Renderer : IDisposable
    {
        public enum StringAlignment
        {
            TopLeft,
            Centered
        }

        public Renderer(Viewport targetViewport, Graphics graphics)
        {
            _viewport = targetViewport;
            _graphics = graphics;
            //_graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //_graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            _graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
        }

        public Viewport TargetViewport
        {
            get { return _viewport; }
        }

        public Graphics Graphics
        {
            get { return _graphics; }
        }

        public void Clear()
        {
            _graphics.Clear(Color.Green);
        }

        public void Dispose()
        {

        }

        public void RenderTexture(Texture texture, Transform transform)
        {
            RenderTexture(texture, transform.Position, transform.Position+transform.Size, transform.Angle, transform.RotationPivot);
        }

        public void RenderTexture(Texture texture, Vector2 from, Vector2 to)
        {
            RenderTexture(texture, from, to, 0, Vector2.Zero);
        }

        public void RenderTexture(Texture texture, Vector2 from, Vector2 to, float angle, Vector2 pivot)
        {
            PixelOffsetMode previousPixelOffsetMode = _graphics.PixelOffsetMode;
            try
            {
                _graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                RectangleF rectangle = ToRectangleF(from, to);
                RectangleF sourceRectangle = new RectangleF(0, 0, texture.Bitmap.Width, texture.Bitmap.Height);
                RenderRotated(angle, pivot, () =>
                {
                    _graphics.DrawImage(texture.Bitmap, rectangle, sourceRectangle, GraphicsUnit.Pixel);
                });
            }
            finally
            {
                _graphics.PixelOffsetMode = previousPixelOffsetMode;
            }
        }

        public void RenderString(Transform transform, string str, int fontSize, Color color, StringAlignment alignment)
        {
            RenderString(transform.Position, transform.Position + transform.Size, transform.Angle, transform.RotationPivot, str, fontSize, color, alignment);
        }

        public void RenderString(Vector2 from, Vector2 to, string str, int fontSize, Color color, StringAlignment alignment)
        {
            RenderString(from, to, 0, Vector2.Zero, str, fontSize, color, alignment);
        }
        
        public void RenderString(Vector2 from, Vector2 to, float angle, Vector2 pivot, string str, int fontSize, Color color, StringAlignment alignment)
        {
            StringFormat format = new StringFormat(StringFormat.GenericTypographic);
            format.Trimming = StringTrimming.None;
            format.FormatFlags = StringFormatFlags.NoWrap;
            format.Alignment = System.Drawing.StringAlignment.Near;

            using (Brush brush = GetSolidBrush(color))
            {
                using (Font font = GetFont("Arial", fontSize))
                {
                    Vector2 padding;
                    if (alignment == StringAlignment.Centered)
                    {
                        CharacterRange[] ranges = { new CharacterRange(0, str.Length) };
                        format.SetMeasurableCharacterRanges(ranges);
                        RectangleF zone = ToRectangleF(from, to);
                        Region[] boundings = _graphics.MeasureCharacterRanges(str, font, zone, format);
                        RectangleF stringZone = boundings[0].GetBounds(_graphics);
                        padding = new Vector2((zone.Width - stringZone.Width) / 2, (zone.Height - stringZone.Height) / 2);
                    }
                    else if (alignment == StringAlignment.TopLeft)
                    {
                        padding = Vector2.Zero;
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Unhandled case {0}", alignment));
                    }

                    RectangleF rectangle = ToRectangleF(from + padding, to - padding);
                    RenderRotated(angle, pivot, () =>
                    {
                        _graphics.DrawString(str, font, brush, rectangle, format);
                    });
                }
            }
        }

        public void RenderRectangle(Transform transform, Color color)
        {
            RenderRectangle(transform.Position, transform.Position + transform.Size, transform.Angle, transform.RotationPivot, color);
        }

        public void RenderRectangle(Vector2 from, Vector2 to, Color color)
        {
            RenderRectangle(from, to, 0, Vector2.Zero, color);
        }

        public void RenderRectangle(Vector2 from, Vector2 to, float angle, Vector2 pivot, Color color)
        {
            using (Brush brush = GetSolidBrush(color))
            {
                RectangleF rectangle = ToRectangleF(from, to);
                RenderRotated(angle, pivot, () =>
                {
                    _graphics.FillRectangles(brush, new RectangleF[] { rectangle });
                });
            }
        }

        private void RenderRotated(float angle, Vector2 pivot, Action renderFunction)
        {
            using (Matrix rotation = new Matrix())
            {
                rotation.RotateAt(MathHelper.ToDegree(angle), ToPointF(pivot));
                RenderTransformed(rotation, renderFunction);
            }
        }

        private void RenderTransformed(Matrix transform, Action renderFunction)
        {
            try
            {
                _graphics.Transform = transform;
                renderFunction();
            }
            finally
            {
                _graphics.ResetTransform();
            }
        }

        public void RenderLine(Transform transform, float width, Color color)
        {
            RenderLine(transform.GetTopLeftPosition(), transform.GetBottomRightPosition(), width, color);
        }

        public void RenderLine(Vector2 from, Vector2 to, float width, Color color)
        {
            using (SolidBrush brush = GetSolidBrush(color))
            {
                using (Pen pen = GetPen(brush, width))
                {
                    _graphics.DrawLine(pen, ToPointF(from), ToPointF(to));
                }
            }
        }

        public void RenderArrow(Transform transform, float width, Color color)
        {
            RenderArrow(transform.GetTopLeftPosition(), transform.GetBottomRightPosition(), width, color);
        }

        public void RenderArrow(Vector2 from, Vector2 to, float width, Color color)
        {
            using (SolidBrush brush = GetSolidBrush(color))
            {
                using (Pen pen = GetPen(brush, width))
                {
                    pen.CustomEndCap = new AdjustableArrowCap(width/2, width/2);
                    _graphics.DrawLine(pen, ToPointF(from), ToPointF(to));
                }
            }
        }

        private Font GetFont(string fontFamily, int size)
        {
            return new Font(fontFamily, size);
        }

        private Pen GetPen(SolidBrush brush, float width)
        {
            return new Pen(brush, width);
        }

        private SolidBrush GetSolidBrush(Color color)
        {
            return new SolidBrush(color);
        }

        private RectangleF ToRectangleF(Vector2 from, Vector2 to)
        {
            Vector2 topLeft = new Vector2(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
            Vector2 size = new Vector2(Math.Abs(from.X - to.X), Math.Abs(from.Y - to.Y)) + Vector2.One;
            return new RectangleF(ToPointF(topLeft), ToSizeF(size));
        }

        private SizeF ToSizeF(Vector2 vec)
        {
            return new SizeF(vec.X, vec.Y);
        }

        private PointF ToPointF(Vector2 vec)
        {
            return new PointF(vec.X, vec.Y);
        }

        private void ToParalelogram(Vector2 from, Vector2 to, out PointF[] points)
        {
            points = new PointF[3];
            points[0] = ToPointF(from); // top-left;

            points[1] = ToPointF(new Vector2(to.X, from.Y)); // top-right
            points[2] = ToPointF(new Vector2(from.X, to.Y)); // bottom-left
        }

        private Viewport _viewport;
        private Graphics _graphics;
    }
}

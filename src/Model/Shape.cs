using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    public abstract class Shape
    {
        #region Constructors

        public Shape()
        {
        }

        public Shape(PointF upperLeft, PointF bottomRight)
        {
            Name = "Noone";
            Location = upperLeft;
            this.bottomRight = bottomRight;
            Width = bottomRight.X - Location.X;
            Height = bottomRight.Y - Location.Y;
            //  Outline = new RectangleF(Location.X, Location.Y, Width, Height);
            path = new GraphicsPath();
            Outline = new RectangleF(path.GetBounds().Height, path.GetBounds().Left, Width, Height);
        }


        #endregion

        #region Properties

        public bool Selected { get; set; }
        public virtual float Width { get; set; }
        public virtual float Height { get; set; }
        public virtual List<PointF> Points { get; set; }
        public virtual PointF Location { get; set; }
        public virtual PointF bottomRight { get; set; }
        public Color FillColor { get; set; }
        public virtual RectangleF Outline { get; set; }
        public virtual string Name { get; set; }
        public abstract string Type { get; set; }
        public abstract int Opacity { get; set; }
        public virtual GraphicsPath path { get; set; }

        #endregion
        Pen notSel = new Pen(Color.Black, 3f);


        public virtual bool Contains(PointF point)
        {
            if (path.IsVisible(point) || path.IsOutlineVisible(point, notSel))
                return true;
            return false;
        }

        public virtual void DrawSelf(Graphics grfx, bool selected)
        {
            path = new GraphicsPath();

            switch (Type)
            {
                case "Line": path.AddLine(Location, bottomRight);
                    break;
                case "Circle":
                    path.AddEllipse(new RectangleF(Location.X, Location.Y, Width, Height));
                    break;
                case "Rectangle":
                    path.AddRectangle(new RectangleF(Location.X, Location.Y, Width, Height));
                    break;
               /* case "Polygon":
                    path.AddPolygon(PointF[] pts);*/
                default:
                    break;
            }

            FillColor = Color.FromArgb(Opacity, FillColor);
            Pen pen = new Pen(Color.Black, 3f);
            Pen notSel = new Pen(Color.Black, 3f);
            notSel.Color = FillColor;
            pen.DashStyle = DashStyle.Dash;
            if (selected)
            {
                grfx.DrawPath(notSel, path);
                grfx.DrawRectangle(pen, (int)path.GetBounds().X, (int)path.GetBounds().Y, Width, Height);
                switch (Type)
                {
                    case "Circle": grfx.FillEllipse(new SolidBrush(FillColor), path.GetBounds());
                        break;
                    case "Rectangle": grfx.FillRectangle(new SolidBrush(FillColor), path.GetBounds());
                        break;
                    default:
                        break;
                }
            }
            else
            {
                grfx.DrawPath(notSel, path);
                switch (Type)
                {
                    case "Circle":
                        grfx.FillEllipse(new SolidBrush(FillColor), path.GetBounds());
                        break;
                    case "Rectangle":
                        grfx.FillRectangle(new SolidBrush(FillColor), path.GetBounds());
                        break;
                    default:
                        break;
                }
            }
        }

        public virtual void Translate(PointF newP)
        {
            bottomRight = newP;
            Width = (Location.X > newP.X) ? Location.X - newP.X : newP.X - Location.X;
            Height = (Location.Y > newP.Y) ? Location.Y - newP.Y : newP.Y - Location.Y;
            Matrix translateMatrix = new Matrix();
            translateMatrix.Translate(newP.X - Location.X, newP.Y - Location.Y);
            path.Transform(translateMatrix);
            /* if (newP.X < Location.X)
             {
                 Width = Location.X - newP.X;
                 Location = new PointF(Location.X - Width, Location.Y);
             }
             else
                 Width = newP.X - Location.X;
             if (newP.Y < Location.Y)
             {
                 Height = Location.Y - newP.Y;
                 Location = new PointF(Location.X, Location.Y - Height);
             }
             else
                 Height = newP.Y - Location.Y;*/

        }

        public virtual void Transform(Shape shape, PointF cursorPos)
        {
            if (cursorPos.X > shape.Location.X && cursorPos.Y > shape.Location.Y)
            {
                Width = cursorPos.X - Location.X;
                Height = cursorPos.Y - Location.Y;
            }
            bottomRight = new PointF(Location.X + Width, Location.Y + Height);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.src.Model
{
    public class Line : Shape
    {
        public override string Type { get; set; }
        public override int Opacity { get; set; }

        public Line(PointF upperLeft, PointF bottomRight) : base(upperLeft, bottomRight)
        {
            Type = "Line";
        }

        public Line(Shape shape)
        {
            Type = shape.Type;
            Location = shape.Location;
            path = new GraphicsPath();
            path.AddLine(shape.Location, shape.bottomRight);
            Name = shape.Name;
            bottomRight = shape.bottomRight;
            Width = shape.Width;
            Height = shape.Height;
            Outline = shape.Outline;
        }
               
    }
}

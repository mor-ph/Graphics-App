using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    public class Rectangle : Shape
    {
        #region Constructor
        public override string Type { get; set; }
        public override int Opacity { get; set; }

        public Rectangle(PointF upperLeft, PointF bottomRight) : base(upperLeft, bottomRight)
        {
            Type = "Rectangle";
        }

        #endregion
        
    }
}

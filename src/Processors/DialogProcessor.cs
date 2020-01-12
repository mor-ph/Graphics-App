using Draw.src.Model;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
        }

        #endregion

        #region Properties

        private Shape selection;
        public Shape Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        private bool isDragging;
        public bool IsDragging
        {
            get { return isDragging; }
            set { isDragging = value; }
        }
        
        public bool isTransforming { get; set; }

        public PointF pressedAt { get; set; }

        public PointF releasedAt { get; set; }

        public PointF currentPos { get; set; }

        #endregion
        public void updateSelected()
        {
            foreach (Shape sh in ShapeList)
            {
                sh.Selected = (Selection == sh) ? true : false;
            }
        }
        public void AddRectangle()
        {
            Rectangle rect = new Rectangle(pressedAt, currentPos);
            rect.Outline = new RectangleF(rect.Location.X, rect.Location.Y, rect.Width, rect.Height);          
            rect.Opacity = 255;
            rect.FillColor = Color.FromArgb(rect.Opacity, Color.Wheat);
            ShapeList.Add(rect);
            Selection = rect;

            updateSelected();
        }

        public void AddLine(PointF pt1, PointF pt2)
        {
            
            Line line = new Line(pt1, pt2);
            line.Outline = new RectangleF(line.path.GetBounds().X, line.path.GetBounds().Y, line.Width, line.Height);
            line.Opacity = 255;
            line.FillColor = Color.FromArgb(line.Opacity, 45, 134, 89);
            ShapeList.Add(line);
            Selection = line;

            updateSelected();
        }

        public void AddCircle(PointF pt1, PointF pt2)
        {

            Circle circle = new Circle(pt1, pt2);
            circle.Outline = new RectangleF(circle.path.GetBounds().X, circle.path.GetBounds().Y, circle.Width, circle.Height);
            circle.Opacity = 255;
            circle.FillColor = Color.FromArgb(circle.Opacity, 45, 134, 89);
            ShapeList.Add(circle);
            Selection = circle;

            updateSelected();
        }

        public Shape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--)
            {
                if (ShapeList[i].Contains(point))
                {
                    ShapeList[i].Selected = true;
                    return ShapeList[i];
                }
            }
            return null;
        }

        public void TranslateTo(PointF p)
        {
            if (selection != null)
            {
                selection.Location = new PointF(p.X - selection.Width / 2,
                                                p.Y - selection.Height / 2);
                selection.bottomRight = new PointF(selection.Location.X + selection.Width,
                                             selection.Location.Y + selection.Height);
                 
            }
        }

    }
}

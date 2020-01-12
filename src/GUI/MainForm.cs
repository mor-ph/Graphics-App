using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Върху главната форма е поставен потребителски контрол,
    /// в който се осъществява визуализацията
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
        /// </summary>
        /// 
        private DialogProcessor dialogProcessor = new DialogProcessor();

        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //ctor
            

        }

        void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        void ViewPortPaint(object sender, PaintEventArgs e)
        {
            dialogProcessor.ReDraw(sender, e);
        }

        public void setProperties(Shape shape)
        {
            nameTxt.Text = shape.Name;
            typeTxt.Text = shape.Type;
            colorBtn.BackColor = shape.FillColor;
            opacityTxt.Text = shape.Opacity.ToString();
            xtxt.Text = "X: " + shape.Location.X.ToString();
            ytxt.Text = "Y: " + shape.Location.Y.ToString();
            widthTxt.Text = shape.Width.ToString();
            heightTxt.Text = shape.Height.ToString();
        }
        
        #region
        private void ManageButtons(object sender)
        {
            foreach (ToolStripButton item in speedMenu.Items)
            {
                item.Checked = (item != sender) ? false : true;
            }
        }

        private void nameTxt_TextChanged(object sender, EventArgs e)
        {
            if(dialogProcessor.Selection!=null)
            {
                dialogProcessor.Selection.Name = (nameTxt.Text == null) ? "" : nameTxt.Text;
            }
        }

        private void opacityTxt_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dialogProcessor.Selection != null && opacityTxt.Text != "")
            {
                dialogProcessor.Selection.Opacity = int.Parse(opacityTxt.Text);
            }
        }
        #endregion

        void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (pickUpSpeedButton.Checked)
            {
                dialogProcessor.Selection = dialogProcessor.ContainsPoint(e.Location);
                if (dialogProcessor.Selection != null)
                {
                    foreach(Shape sh in dialogProcessor.ShapeList)
                    {
                        sh.Selected = (dialogProcessor.Selection == sh) ? true : false;
                    }
                    setProperties(dialogProcessor.Selection);

                    if ((int)e.Location.X < (int)dialogProcessor.Selection.Location.X + dialogProcessor.Selection.Width + 4 &&
                     (int)e.Location.X > (int)dialogProcessor.Selection.Location.X + dialogProcessor.Selection.Width - 4 &&
                     (int)e.Location.Y < (int)dialogProcessor.Selection.Location.Y + dialogProcessor.Selection.Height + 4 &&
                     (int)e.Location.Y > (int)dialogProcessor.Selection.Location.Y + dialogProcessor.Selection.Height - 4)
                    {
                        dialogProcessor.isTransforming = true;
                    }
                    else
                        dialogProcessor.IsDragging = true;
                    statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
                    viewPort.Invalidate();
                }
            }

        }

        void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {            
            if (dialogProcessor.Selection != null)
            {
                if (dialogProcessor.isTransforming)
                {
                    Cursor = Cursors.SizeNWSE;
                    dialogProcessor.Selection.Transform(dialogProcessor.Selection, e.Location);
                    setProperties(dialogProcessor.Selection);
                    viewPort.Invalidate();
                }
                else if((int)e.Location.X < (int)dialogProcessor.Selection.Location.X + dialogProcessor.Selection.Width + 4 &&
                       (int)e.Location.X > (int)dialogProcessor.Selection.Location.X + dialogProcessor.Selection.Width - 4 &&
                       (int)e.Location.Y < (int)dialogProcessor.Selection.Location.Y + dialogProcessor.Selection.Height + 4 &&
                       (int)e.Location.Y > (int)dialogProcessor.Selection.Location.Y + dialogProcessor.Selection.Height - 4 &&
                       pickUpSpeedButton.Checked)
                Cursor = Cursors.SizeNWSE;

            }
            // if you are drawing 
            if (drawing)
            {
                if (drawRectangleSpeedButton.Checked)
                {
                    dialogProcessor.Selection.Translate(e.Location);
                    setProperties(dialogProcessor.Selection);
                    statusBar.Items[0].Text = "Drawing a rectangle..";

                    viewPort.Invalidate();
                }
                else if (lineBtn.Checked)
                {
                    dialogProcessor.Selection.Translate(e.Location);
                    setProperties(dialogProcessor.Selection);
                    statusBar.Items[0].Text = "Drawing a line..";

                    viewPort.Invalidate();
                }
                else if (ellipseBtn.Checked)
                {
                    dialogProcessor.Selection.Translate(e.Location);
                    setProperties(dialogProcessor.Selection);
                    statusBar.Items[0].Text = "Drawing an ellipse..";

                    viewPort.Invalidate();
                }
            }         

            // if you are dragging a shape
            if (dialogProcessor.IsDragging && dialogProcessor.Selection != null)
            {
                    statusBar.Items[0].Text = $"Dragging {dialogProcessor.Selection.ToString().Remove(0, 5)}; " +
                        $" Width: {dialogProcessor.Selection.Width};  Height: {dialogProcessor.Selection.Height};  " +
                            $"Cursor x: {Cursor.Position.X}, y: {Cursor.Position.Y}";
                    dialogProcessor.TranslateTo(e.Location);
                setProperties(dialogProcessor.Selection);
                viewPort.Invalidate();
            }
            
        }

        bool drawing = false;
        PointF pt1;// = new PointF();
        void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (drawRectangleSpeedButton.Checked)
            {
                if (drawing == false)
                {
                    dialogProcessor.pressedAt = e.Location;
                    dialogProcessor.currentPos = e.Location;
                    dialogProcessor.AddRectangle();
                    if (dialogProcessor.Selection != null)
                    {
                        setProperties(dialogProcessor.Selection);
                    }
                    drawing = true;
                }
                else
                {
                    dialogProcessor.Selection.Translate(e.Location);
                    setProperties(dialogProcessor.Selection);
                    drawing = false;
                }
            }
            else if (lineBtn.Checked)
            {
                if (drawing == false)
                {
                    dialogProcessor.pressedAt = e.Location;
                    dialogProcessor.currentPos = e.Location;
                    pt1 = new PointF();
                    pt1 = e.Location;
                    if (pt1 != null)
                    {
                        dialogProcessor.AddLine(pt1, e.Location);
                    }
                    if (dialogProcessor.Selection != null)
                    {
                        setProperties(dialogProcessor.Selection);
                    }
                    drawing = true;
                }
                else
                {
                    dialogProcessor.Selection.Translate(e.Location);
                    setProperties(dialogProcessor.Selection);
                    drawing = false;
                }
            }
            else if (ellipseBtn.Checked)
            {

                if (drawing == false)
                {
                    dialogProcessor.pressedAt = e.Location;
                    dialogProcessor.currentPos = e.Location;
                    pt1 = new PointF();
                    pt1 = e.Location;
                    if (pt1 != null)
                    {
                        dialogProcessor.AddCircle(pt1, e.Location);
                    }
                    if (dialogProcessor.Selection != null)
                    {
                        setProperties(dialogProcessor.Selection);
                    }
                    drawing = true;
                }
                else
                {
                    dialogProcessor.Selection.Translate(e.Location);
                    setProperties(dialogProcessor.Selection);
                    drawing = false;
                }
            }

            if (dialogProcessor.Selection == null)
            {
                nameTxt.Text = "";
                colorBtn.BackColor = SystemColors.Control;
                opacityTxt.Text = "";
                xtxt.Text = "";
                ytxt.Text = "";
                typeTxt.Text = "";
                widthTxt.Text = "";
                heightTxt.Text = "";
               foreach(Shape shape in dialogProcessor.ShapeList)
                {
                    shape.Selected = false;
                }
            }
                
            dialogProcessor.releasedAt = e.Location;
            dialogProcessor.IsDragging = false;
            dialogProcessor.isTransforming = false;
            viewPort.Invalidate();
        }

        void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
        {
            ManageButtons(sender);
        }       
        
        private void pickUpSpeedButton_Click(object sender, EventArgs e)
        {
            ManageButtons(sender);
        }

        private void lineBtn_Click_1(object sender, EventArgs e)
        {
            ManageButtons(sender);
        }

        private void elipseBtn_Click(object sender, EventArgs e)
        {
            ManageButtons(sender);
        }
        
        private void colorBtn_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.ShowHelp = true;
            dialog.Color = colorBtn.BackColor;
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                colorBtn.BackColor = dialog.Color;
                dialogProcessor.Selection.FillColor = dialog.Color;
                viewPort.Invalidate();
            }
        }

    }
}

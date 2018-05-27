using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using WinFormsTriangle.Manager;
using WinFormsTriangle.Shape;
using WinFormsTriangle.Utils;

namespace WinFormsTriangle
{
    public partial class FigureForm : Form
    {
        List<Point> points = new List<Point>();
        List<IShape> shapes = new List<IShape>();
        bool isMouseDown = false;
        int activeIndex = -1;

        int OffsetX;
        int OffsetY;

        public FigureForm()
        {
            InitializeComponent();
        }

        private void FigureForm_Load(object sender, EventArgs e)
        {
            List<string> samples = SampleShapes.LoadFileNames();
            foreach (var file in samples)
            {
                ToolStripItem shapesItem = new ToolStripMenuItem(file);
                shapesItem.Click += shapes_Click;
                shapesMenuItem.DropDownItems.Add(shapesItem);
            }
        }

        private void picBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Draw(e.Graphics);
            }
        }

        #region picBox_Mouse

        private void picBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseEvent = e as MouseEventArgs;
            if (mouseEvent.Button == MouseButtons.Left)
            {
                Point point = new Point(mouseEvent.Location.X, mouseEvent.Location.Y);
                points.Add(point);

                if (points.Count == 3)
                {
                    ColorDialog colorDialog = new ColorDialog();
                    Color color = Color.Black;
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        color = colorDialog.Color;
                    }
                    var graphics = this.picBox.CreateGraphics();
                    SolidBrush brush = new SolidBrush(color);
                    graphics.FillPolygon(brush, points.ToArray());
                    shapes.Add(new Triangle(color, points.ToList()));
                    points.Clear();
                }
            }
        }

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                if (activeIndex == -1)
                {
                    return;
                }

                int new_x1 = e.X + OffsetX;
                int new_y1 = e.Y + OffsetY;

                int dx = new_x1 - shapes[activeIndex].Points[0].X;
                int dy = new_y1 - shapes[activeIndex].Points[0].Y;

                if (dx == 0 && dy == 0)
                {
                    return;
                }

                shapes[activeIndex].Move(dx, dy);

                picBox.Invalidate();
            }
        }        

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            SetActiveIndex(e);
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        #endregion


        #region Menu

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shapes.Clear();

            picBox.Invalidate();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xml";
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                SerializationManager manager = new SerializationManager();
                string fileName = openFileDialog.FileName;
                List<Triangle> triangles = manager.Deserialize(fileName);
                shapes = triangles.ConvertAll(s => (IShape)s);
            }

            picBox.Invalidate();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SerializationManager manager = new SerializationManager();
                string fileName = saveFileDialog.FileName;
                manager.Serialize(fileName, shapes.ConvertAll(s => (Triangle)s));
            }

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Context Menu

        private void picBox_MouseClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseEvent = e as MouseEventArgs;
            if (mouseEvent.Button == MouseButtons.Right)
            {
                SetActiveIndex(e);
                if (activeIndex != -1)
                {
                    MenuItem changeColor = new MenuItem("Change Color");
                    changeColor.Click += changeColor_Click;
                    MenuItem[] menuItems = new MenuItem[] { changeColor };
                    ContextMenu buttonMenu = new ContextMenu(menuItems);
                    buttonMenu.Show(picBox, new Point(e.X + 20, e.Y + 20));
                }
            }
        }

        private void shapes_Click(object sender, EventArgs e)
        {
            ToolStripItem toolStripItem = (sender as ToolStripItem);
            string name = toolStripItem.Text;

            string filePath = SampleShapes.BuildFilePath(name);
            SerializationManager manager = new SerializationManager();
            List<Triangle> triangles = manager.Deserialize(filePath);
            shapes.AddRange(triangles);

            picBox.Invalidate();
        }

        private void changeColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                shapes[activeIndex].Color = colorDialog.Color;
                picBox.Invalidate();
            }
        }

        #endregion

        private void SetActiveIndex(MouseEventArgs e)
        {
            activeIndex = -1;
            for (int i = 0; i < shapes.Count; i++)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddPolygon(shapes[i].Points.ToArray());

                if (path.IsVisible(e.Location))
                {
                    activeIndex = i;
                    OffsetX = shapes[i].Points[0].X - e.X;
                    OffsetY = shapes[i].Points[0].Y - e.Y;
                }
            }
        }
    }
}
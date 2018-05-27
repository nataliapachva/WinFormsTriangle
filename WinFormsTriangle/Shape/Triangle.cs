using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;

namespace WinFormsTriangle.Shape
{
    [Serializable]
    public class Triangle : IShape
    {
        public Triangle() { }

        public Triangle(Color color, List<Point> points)
        {
            this.Color = color;
            this.Points = points;
        }

        public List<Point> Points { get; set; }

        public Color Color { get; set; }

        public int XmlColor
        {
            get
            {
                return Color.ToArgb();
            }
            set
            {
                Color = Color.FromArgb(value);
            }
        }

        public void Move(int dx, int dy)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Points[i] = new Point(Points[i].X + dx, Points[i].Y + dy);
            }
        }

        public void Draw(Graphics graphics)
        {
            SolidBrush brush = new SolidBrush(Color);
            graphics.FillPolygon(brush, Points.ToArray());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTriangle.Shape
{
    public interface IShape: IMoveable, IPaintable
    {
        Color Color { get; set; }
        List<Point> Points { get; set; }
    }
}

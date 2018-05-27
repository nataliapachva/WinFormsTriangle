using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTriangle.Shape
{
    public interface IPaintable
    {
        void Draw(Graphics graphics);
    }
}

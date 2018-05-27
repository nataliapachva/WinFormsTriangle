using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormsTriangle.Shape;
using System.Drawing;

namespace WinFormsTriangleUnitTests.Shape
{
    /// <summary>
    /// Summary description for TriangleTests
    /// </summary>
    [TestClass]
    public class TriangleTests
    {
        [TestMethod]
        public void MoveTest()
        {
            Triangle triangle = new Triangle
            {
                Points = new List<Point> { new Point(100, 300), new Point(200, 500), new Point(800, 200) }
            };
            int dx = 50;
            int dy = 70;

            triangle.Move(dx, dy);

            Assert.AreEqual(triangle.Points[0].X, 150);
            Assert.AreEqual(triangle.Points[0].Y, 370);
            Assert.AreEqual(triangle.Points[1].X, 250);
            Assert.AreEqual(triangle.Points[1].Y, 570);
            Assert.AreEqual(triangle.Points[2].X, 850);
            Assert.AreEqual(triangle.Points[2].Y, 270);
        }    
    }
}

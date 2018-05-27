using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using WinFormsTriangle.Shape;

namespace WinFormsTriangle.Manager
{
    public class SerializationManager
    {
        public XmlSerializer formatter;

        public void Serialize(string fileName, IShape shape)
        {
            formatter = new XmlSerializer(typeof(Triangle));

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, shape);
            }
        }


        public void Serialize(string fileName, List<Triangle> shapes)
        {
            formatter = new XmlSerializer(typeof(List<Triangle>));
           
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, shapes);
            }
        }

        public List<Triangle> Deserialize(string fileName)
        {
            formatter = new XmlSerializer(typeof(List<Triangle>));
            
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                List<Triangle> triangles = (List<Triangle>)formatter.Deserialize(fs);

                return triangles;
            }
        }
    }
}
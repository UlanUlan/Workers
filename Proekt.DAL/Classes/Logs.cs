using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Proekt.DAL.Classes
{
    public class Logs
    {
        public Logs()
        {

        }
        public void WriteLogs(Exception ex)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(string));
            using (FileStream fs = new FileStream("Logs.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, ex.Message);
            }
        }
    }
}

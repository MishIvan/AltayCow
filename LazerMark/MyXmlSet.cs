using System;
using System.Windows.Forms;
using System.Xml;

namespace LazerMark
{
    internal class MyXmlSet
    {

        public static string GetMyConfig(string path)
        {
            string innerText;
            try
            {
                string xmlfile = LazerMark.Properties.Resources.CFG_FILE;
                string s1 = System.IO.Path.GetFullPath(xmlfile);
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlfile);
                innerText = doc.SelectSingleNode(path).InnerText;
                return innerText;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                MessageBox.Show(string.Concat("MyXmlSet::GetMyConfig() ", ex.Message));
            }
            innerText = "";
            return innerText;
        }

        public static void SetMyConfig(string path, string fn)
        {
            string xmlfile = LazerMark.Properties.Resources.CFG_FILE; 
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfile);
            doc.SelectSingleNode(path).InnerText = fn;
            doc.Save(xmlfile);
        }
    }
}
using System;
using System.Windows.Forms;
using System.Xml;

namespace SocketSample
{
    internal class MyXmlSet
    {
        public MyXmlSet()
        {
        }

        public static string GetMyConfig(string path)
        {
            string innerText;
            try
            {
                string xmlfile = "MyAppConfig.xml";
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
            string xmlfile = "MyAppConfig.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfile);
            doc.SelectSingleNode(path).InnerText = fn;
            doc.Save(xmlfile);
        }
    }
}
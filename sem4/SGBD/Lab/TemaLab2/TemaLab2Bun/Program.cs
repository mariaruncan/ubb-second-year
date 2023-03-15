using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;
using TemaLab1;

namespace Seminar_2_226_SGBD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigurationManager.AppSettings.Get("xmlFilePath"));
            XmlElement elems = xmlDoc.DocumentElement;

            XmlNode parentNode = elems.SelectSingleNode("parent");
            string parentName = parentNode.SelectSingleNode("name").InnerText;
            int parentNoFields = Int32.Parse(parentNode.SelectSingleNode("noFields").InnerText);
            Table parent = new Table(parentName, parentNoFields);
            XmlNode parentFields = parentNode.SelectSingleNode("fields");
            foreach (XmlNode field in parentFields)
            {
                Column c = new Column(field.SelectSingleNode("name").InnerText, Convert.ToBoolean(field.SelectSingleNode("PK").InnerText),
                    Convert.ToBoolean(field.SelectSingleNode("FK").InnerText));
                parent.Columns.Add(c);
            }
            parent.SelectCmd = parentNode.SelectSingleNode("selectCmd").InnerText;

            XmlNode childNode = elems.SelectSingleNode("child");
            string childName = childNode.SelectSingleNode("name").InnerText;
            int childNoFields = Int32.Parse(childNode.SelectSingleNode("noFields").InnerText);
            Table child = new Table(childName, childNoFields);
            XmlNode childFields = childNode.SelectSingleNode("fields");
            foreach (XmlNode field in childFields)
            {
                Column c = new Column(field.SelectSingleNode("name").InnerText, Convert.ToBoolean(field.SelectSingleNode("PK").InnerText),
                    Convert.ToBoolean(field.SelectSingleNode("FK").InnerText));
                child.Columns.Add(c);
            }
            child.SelectCmd = childNode.SelectSingleNode("selectCmd").InnerText;
            child.InsertCmd = childNode.SelectSingleNode("insertCmd").InnerText;
            child.UpdateCmd = childNode.SelectSingleNode("updateCmd").InnerText;
            child.DeleteCmd = childNode.SelectSingleNode("deleteCmd").InnerText;

            Application.Run(new Form1(parent, child));
        }
    }
}

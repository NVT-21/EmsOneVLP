using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace EE.PartnerGetData.API.Utility
{
    public static class Common
    {
        public static string GetException(this Exception exception)
        {
            string _exMessage = exception.Message;

            if (exception.InnerException != null)
            {
                _exMessage = GetException(exception.InnerException);
            }

            return _exMessage;
        }

        public static string ToXML(this DataTable table)
        {
            string[] columnNames = table.Columns.Cast<DataColumn>()
                                 .Where(c => c.DataType == typeof(DateTime) || "DateCreated".Equals(c.ColumnName) || "DateUpdated".Equals(c.ColumnName))
                                 .Select(x => x.ColumnName)
                                 .ToArray();

            ExtentedStringWriter writer = new ExtentedStringWriter();
            table.WriteXml(new CustomXmlTextWriter(writer, columnNames), XmlWriteMode.IgnoreSchema, false);

            return writer.ToString();
        }
        public static string ToXML(this object value)
        {
            try
            {
                using (var stringwriter = new ExtentedStringWriter())
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType(), new System.Xml.Serialization.XmlRootAttribute("Data"));
                    serializer.Serialize(stringwriter, value);
                    return stringwriter.ToString();
                };
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }

    public sealed class ExtentedStringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }

    public class CustomXmlTextWriter : XmlTextWriter
    {
        private string[] watchElement;
        private bool onWatch;

        public CustomXmlTextWriter(TextWriter writer, string[] columnNames) : base(writer)
        {
            this.watchElement = columnNames;
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            base.WriteStartElement(prefix, localName, ns);
            if (watchElement.Contains(localName))
            {
                onWatch = true;
            }
        }

        public override void WriteString(string text)
        {
            if (onWatch)
            {
                try
                {
                    DateTime dt = DateTime.Parse(text);
                    text = dt.ToString("dd/MM/yyyy HH:mm:ss");
                }
                catch (FormatException)
                {

                }
                onWatch = false;
            }
            base.WriteString(text);
        }
    }
}
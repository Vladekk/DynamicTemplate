using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace DynamicTemplate
{
    public class Template
    {
        private ArgumentDescription[] _arguments = new ArgumentDescription[0];
        private string _body = string.Empty;

        [XmlArrayItem(ElementName = "Argument")]
        public ArgumentDescription[] Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                _arguments = value;
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = FixupWhiteSpace(value);
            }
        }

        public static Template Load(string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Template));
            using (Stream s = File.OpenRead(filePath))
            {
                return (Template)ser.Deserialize(s);
            }
        }

        public void Save(string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Template));
            using (Stream s = File.Create(filePath))
            {
                ser.Serialize(s, this);
            }
        }

        /// <summary>
        /// HACK: Converts standalone \n to \r\n. For some reason this happens
        /// after roundtripping to XML.
        /// </summary>
        private string FixupWhiteSpace(string value)
        {
            return Regex.Replace(value, @"(?<!\r)\n", "\r\n");
        }
    }
}

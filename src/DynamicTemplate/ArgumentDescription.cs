using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DynamicTemplate
{
    public enum ArgumentType
    {
        TextString,
        TextStringExtended,
        HtmlString,
        HtmlStringExtended,
        Integer,
        Double,
        Boolean,
        DateTime
    }

    public class ArgumentDescription
    {
        private ArgumentType _type;
        private string _identifier;
        private string _label;

        public ArgumentDescription() { }

        public ArgumentDescription(ArgumentType type, string identifier, string label)
        {
            _type = type;
            _identifier = identifier;
            _label = label;
        }

        [XmlAttribute]
        public ArgumentType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [XmlAttribute]
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        [XmlText]
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
    }
}

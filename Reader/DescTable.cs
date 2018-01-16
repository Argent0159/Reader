using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Reader
{
    public class DescTable
    {

        public DescTable() : this(0,null)
        {

        }

        public DescTable(int id, string text)
        {
            Id = id;
            Text = text;
        }

        [XmlAttribute("ID")]
        public int Id { get; set; }
        [XmlElement]
        public string Text { get; set; }


        public override string ToString()
        {
            return "ID:" + Id + Environment.NewLine + "Text:" + Environment.NewLine + Text;
        }
    }
}

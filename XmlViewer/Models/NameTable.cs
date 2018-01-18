using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Reader
{
    public class NameTable
    {
        public NameTable() : this(0, null)
        {

        }

        public NameTable(int id,string name)
        {
            Id = id;
            Name = name;
        }

        public NameTable(string id, string name)
        {
            Id = int.Parse(id);
            Name = name;
        }

        [XmlAttribute("ID")]
        public int Id { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}

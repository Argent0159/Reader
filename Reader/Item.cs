using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Reader
{
    
    public class Item : DescTable
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        
        public Item() : this(null,null)
        {

        }


        public Item(DescTable desc,string name) : base(desc.Id,desc.Text)
        {

        }
    }
}

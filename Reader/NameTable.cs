using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace Reader
{
    [DataContract]
    class NameTable
    {
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

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}

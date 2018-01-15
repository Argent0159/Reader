using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Reader
{
    [DataContract]
    class Item : DescTable
    {
        [DataMember]
        public string Name { get; set; }

        public Item(DescTable desc,string name) : base(desc.Id,desc.Text,desc.Category,desc.Equip,desc.Weight)
        {
            Name = name;
        }
    }
}

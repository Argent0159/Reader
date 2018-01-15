using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;

namespace Reader
{
    [DataContract]
    public class DescTable
    {
        
        public DescTable(int id,string text,string category,string equip,int weight)
        {
            Id = id;
            Text = text;
            Category = category;
            Equip = equip;
            Weight = weight;
        }
        

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Equip { get; set; }
        [DataMember]
        public int Weight { get; set; }

        public override string ToString()
        {
            return "ID:" + Id + Environment.NewLine + "Text:" + Environment.NewLine + Text;
        }
    }
}

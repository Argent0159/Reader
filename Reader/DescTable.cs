﻿using System;
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

        public void SetParameter(DescTable desc)
        {
            desc = Factory<DescTable>.Veritifate(desc);
            Id = desc.Id;
            Text = desc.Text;
        }

        public DescTable(int id, string text)
        {
            Id = id;
            Text = text;
        }

        public DescTable(string id,string text)
        {
            Id = int.TryParse(id, out var number) ? number : 0;
            Text = text;
        }

        public DescTable SetProperty(IEnumerable<ItemProperty> properties)
        {
            Properties = properties.ToList();

            return this;
        }

        [XmlAttribute("ID")]
        public int Id { get; set; }
        [XmlElement]
        public string Text { get; set; }
        [XmlArray("Properties")]
        public List<ItemProperty> Properties { get; set; }

        public override string ToString()
        {
            return "ID:" + Id + Environment.NewLine + "Text:" + Environment.NewLine + Text;
        }
    }

    [XmlRoot]
    public class ItemProperty
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Text { get; set; }
    }
}

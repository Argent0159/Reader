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
    [XmlRoot("Items")]
    public class ItemCollection
    {
        [XmlElement(Type =typeof(Item),ElementName ="Item")]
        public Item[] Items { get; set; }

        public static ItemCollection Create(IEnumerable<Item> items)
        {
            var back = new ItemCollection();
            back.Items = items.ToArray();
            return back;
        }

        public Item this[int index]
        {
            get
            {
                return Items[index];
            }
        }
    }

    [XmlRoot("Item")]
    public class Item : DescTable
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement]
        public Illust illust { get; set; }

        public Item() : base(0, string.Empty)
        {
            Name = string.Empty;
            illust = new Illust();
        }


        public Item(DescTable desc,string name) : base(desc.Id,desc.Text)
        {
            Name = name;
        }

        public Item(Item item,Illust illust) : base(item.Id,item.Text)
        {
            Name = item.Name;
            this.illust = illust;
        }

        public Item InsertIllust(Illust illust)
        {
            this.illust = illust;
            return this;
        }
    }

    //パスを割り当てるときはカード、アイテム画像の順で割り当てること
    public class Illust
    {
        public Illust() : this(0, string.Empty, string.Empty)
        {

        }

        public Illust(int id, string card)
        {
            Id = id;
            Card = card;
        }

        public Illust(int id, string card, string icon) : this(id,card)
        {
            Icon = icon;
        }

        [XmlIgnore]
        public int Id { get; set; }
        [XmlAttribute]
        public string Card { get; set; }
        [XmlAttribute]
        public string Icon { get; set; }
    }
}

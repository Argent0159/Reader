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
        public IList<Item> Items { get; private set; }

        public ItemCollection()
        {
            Items = new List<Item>();
        }

        public static ItemCollection Create(IEnumerable<Item> items)
        {
            return new ItemCollection
            {
                Items = items.ToArray()
            };
        }
    }

    [XmlRoot("Item")]
    public class Item : DescTable
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement]
        public Illust Illust { get; set; }

        public void SetParameter(Item item)
        {
            item = Factory<Item>.Veritifate(item);

            Id = item.Id;
            Name = item.Name;
            Illust = item.Illust;
        }

        public Item() : base(0, string.Empty)
        {
            Name = string.Empty;
            Illust = new Illust();
        }

        public Item(DescTable desc,string name) 
        {
            desc = Factory<DescTable>.Veritifate(desc);
            SetParameter(desc);

            Name = name;
        }

        public Item(Item item,Illust illust)
        {
            item = Factory<Item>.Veritifate(item);
            illust = Factory<Illust>.Veritifate(illust);

            SetParameter(item);
            Illust.SetParameter(illust);

            Name = item.Name;
            Illust = illust;
        }

        public Item InsertIllust(Illust illust)
        {
            Illust = Factory<Illust>.Veritifate(illust);
            return this;
        }
    }

    //パスを割り当てるときはカード、アイテム画像の順で割り当てること
    public class Illust
    {
        public Illust() : this(0, string.Empty, string.Empty)
        {

        }

        public void SetParameter(Illust illust)
        {
            illust = Factory<Illust>.Veritifate(illust);

            Id = illust.Id;
            Card = illust.Card;
            Icon = illust.Icon;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reader;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XmlViewer
{
    class ItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void notifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ItemCollection itemCollection;


        public ItemModel()
        {
            using (var reader = XmlReader.Create(@"xml\IntegratedData.xml"))
            {
                var serializer = new XmlSerializer(typeof(Reader.ItemCollection));
                itemCollection = serializer.Deserialize(reader) as ItemCollection;
            }
        }

        private Item CurrentItem
            => itemCollection.Items[Index];

        int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                CurrentText = CurrentItem.Text;
            }
        }

        public string CurrentText { get; set; }

        public IEnumerable<string> NameList
            => itemCollection.Items.Select(val => val.Name);

    }
}

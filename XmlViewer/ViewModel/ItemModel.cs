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

        void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ItemCollection itemCollection;


        public ItemModel()
        {
            using (var reader = XmlReader.Create(@"xml\IntegratedData.xml"))
            {
                var serializer = new XmlSerializer(typeof(Reader.ItemCollection));
                itemCollection = serializer.Deserialize(reader) as ItemCollection;
            }

            Index = 1;
        }


        int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;

                _index = value;
                CurrentText = itemCollection[Index].Text;
            }
        }

        private string _currentText;

        public string CurrentText
        {
            get { return _currentText; }
            set
            {
                _currentText = value;
                this.OnPropertyChanged();
            }
        }


        public IEnumerable<string> NameList
            => itemCollection.Items.Select(val => val.Name);

    }
}

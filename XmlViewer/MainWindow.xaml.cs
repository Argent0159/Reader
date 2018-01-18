using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;


namespace XmlViewer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ItemModel itemModel;

        public MainWindow()
        {
            itemModel = new ItemModel();

            InitializeComponent();
            ItemList.ItemsSource = itemModel.NameList;

            ItemCaption.DataContext = itemModel.CurrentText;
        }

        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListBox;

            itemModel.Index = list.SelectedIndex;

        }
    }
}

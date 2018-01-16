using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace Reader
{
    class Program
    {
        static void Main(string[] args)
        {

            var descTablePath = @"txt\idnum2itemdesctable.txt";
            var displayNameTablePath = @"txt\idnum2itemdisplaynametable.txt";
            var cardIllustFilePath = @"txt\num2cardillustnametable.txt";
            var itemIllustFilePath = @"txt\idnum2itemresnametable.txt";


            //データ取得
            var nameTable = GetNameTable(displayNameTablePath,Encoding.GetEncoding("shift-jis"));

            var descTable = GetDescTable(descTablePath);

            var cardTable = GetNameTable(cardIllustFilePath, Encoding.GetEncoding("euc-kr"));

            var iconTable = GetNameTable(itemIllustFilePath, Encoding.GetEncoding("euc-kr"));

            var joinTable = descTable
                .Join(nameTable, d => d.Id, n => n.Id, (desc, name) => new Item(desc, name.Name))
                .ToArray();

            var cardAndIcon = nameTable
                .GroupJoin(
                    cardTable,
                    name => name.Id,
                    card => card.Id,
                    (name, card) => new { name.Id, card })
                .GroupJoin(
                    iconTable,
                    name => name.Id,
                    icon => icon.Id,
                    (name, icon) => new { name.Id, name.card, icon });
                              

            //シリアライズ対象の定義と実行
            var serializeTarget = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>(@"xml\integratedTable.xml",joinTable),
                new KeyValuePair<string, object>(@"xml\idnum2itemdesctable.xml",descTable),
                new KeyValuePair<string, object>(@"xml\num2itemdisplaynametable.xml",nameTable),
                new KeyValuePair<string, object>(@"xml\idnum2itemresnametable.xml",iconTable),
                new KeyValuePair<string, object>(@"xml\num2cardillustnametable.xml",cardTable)

            };
            
            //MultiSerializeXml(serializeTarget);
        }

        private static void MultiSerializeXml(IEnumerable<KeyValuePair<string,object>> pairs)
        {
            foreach (var item in pairs)
            {
                SerializeXml(item.Value, item.Key);
            }
        }

        private static void SerializeXml(object target,string filePath)
        {
            var setting = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(filePath,setting))
            {
                new XmlSerializer(target.GetType()).Serialize(writer, target);
            }
        }

        //IDに紐づけられた1つのデータを持つ列挙を取得する
        static IEnumerable<NameTable> GetNameTable(string filePath,Encoding encoding)
        {
            IEnumerable<NameTable> nameTable;

            if (File.Exists(filePath))
            {
                var readFile = File.ReadLines(filePath, encoding);

                //除外対象："//"から始まる行、空白・除外・Null
                nameTable = readFile
                    .Where(val => !val.StartsWith("//"))
                    .Where(val => !string.IsNullOrEmpty(val))
                    .Where(val => !string.IsNullOrWhiteSpace(val))
                    .Select(val => val.Split('#'))
                    .Select(val => new NameTable(val[0], val[1]))
                    .ToArray();
            }
            else
            {
                nameTable = new NameTable[0];
            }

            return nameTable;
        }

        //アイテム情報を取得する
        static IEnumerable<DescTable> GetDescTable(string filePath)
        {
            IEnumerable<DescTable> list;
            if (File.Exists(filePath))
            {
                //ファイルを読み込み、表示修飾に用いるテキストを削除
                var readFile = File.ReadLines(filePath, Encoding.GetEncoding("shift-jis"))
                    .Select(val => Regex.Replace(val, @"\^\S{6}", ""))
                    .Select(val => Regex.Replace(val, @"\s?_\s*", ""));

                var fileText = string.Join(Environment.NewLine, readFile);

                var textPattern = $@"(\d+)#\s*([\S\s]+?)\s*#";

                var pattern = new Regex(textPattern);

                list = pattern.Matches(fileText).Cast<Match>()
                    .Select(val =>
                    {
                        var id = val.Groups[1].ToString();
                        var text = val.Groups[2].ToString();

                        return new DescTable(id, text);
                    })
                    .ToArray();
            }
            else
            {
                list = new DescTable[0];
            }

            return list;
        }
    }
}

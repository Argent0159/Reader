using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;


namespace Reader
{
    class Program
    {
        static Func<Match, int, int> matchIntParse = (match, index) => int.TryParse(match.Groups[index].ToString(), out int answer) ? answer : 0;
        static Func<Match, int, string> matchToString = (match, index) => match.Groups[index].ToString();

        static void Main(string[] args)
        {

            var descTablePath = @"txt\idnum2itemdesctable.txt";
            var displayNameTablePath = @"txt\idnum2itemdisplaynametable.txt";

            var nameTable = GetNameTable(displayNameTablePath,Encoding.GetEncoding("shift-jis"));

            var descTable = GetDescTable(descTablePath);

            var joinTable = descTable.Join(nameTable, d => d.Id, n => n.Id, (desc, name) => new Item(desc, name.Name)).ToList();

            var serializeTarget = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>(@"xml\integratedTable.xml",joinTable),
                new KeyValuePair<string, object>(@"xml\idnum2itemdesctable.xml",descTable),
                new KeyValuePair<string, object>(@"xml\num2itemdisplaynametable.xml",nameTable)
            };

            MultiSerializeXml(serializeTarget);
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

            using (var writer = XmlWriter.Create(filePath, setting))
            {
                var serializer = new DataContractSerializer(target.GetType());
                serializer.WriteObject(writer, target);
            }
        }

        private static IEnumerable<NameTable> GetNameTable(string filePath,Encoding encoding)
        {
            IList<NameTable> nameTable = new List<NameTable>();


            if (File.Exists(filePath))
            {
                var readFile = File.ReadLines(filePath, encoding);

                //ファイル・フィルタリング

                var filtered = readFile
                    .Where(val => !val.StartsWith("//"))
                    .Where(val => !string.IsNullOrEmpty(val));
                

                Func<string, NameTable> convert = val =>
                 {
                     var splited = val.Split('#');

                     var id = splited[0];
                     var name = !string.IsNullOrWhiteSpace(splited[1]) ? splited[1] : null;

                     return new NameTable(id, name);
                 };

                var splitedFile = filtered.Select(val => val.Split('#'));

                foreach (var item in splitedFile)
                {
                    nameTable.Add(new NameTable(item[0], item[1]));
                }
                
            }

            return nameTable;
        }

        private static IEnumerable<DescTable> GetDescTable(string filePath)
        {
            IList<DescTable> list = new List<DescTable>();
            if (File.Exists(filePath))
            {

                var readFile = File.ReadLines(filePath, Encoding.GetEncoding("shift-jis"));

                var fileText = string.Join(Environment.NewLine, readFile);

                //余分なテキストの削除
                var replaced = Regex.Replace(fileText, @"\^\S{6}", "");
                replaced = Regex.Replace(replaced, @"\s?_\s*", "");

                //正規表現の実行、メソッド「info」はパターンの共通化
                Func<string, string> info = val => $@"({val}\s?[:：]\s?(.+?))?\s*?";

                var pattern = new Regex($@"(\d+)#\s*([\S\s]+?)\s*?{info("系列")}{info("装備")}{info("重量")}#");

                var matches = pattern.Matches(replaced).Cast<Match>();

                foreach (var item in matches)
                {

                    var id = matchIntParse(item, 1);
                    var text = matchToString(item, 2);
                    var category = matchToString(item, 4);
                    var equip = matchToString(item, 6);
                    var weight = matchIntParse(item, 8);

                    list.Add(new DescTable(id, text, category, equip, weight));
                }

            }

            return list;
        }
    }
}

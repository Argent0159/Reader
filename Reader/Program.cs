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
        static Func<Match, int, int> matchIntParse = (match, index) => int.TryParse(match.Groups[index].ToString(), out int answer) ? answer : 0;
        static Func<Match, int, string> matchToString = (match, index) => match.Groups[index].ToString();

        static void Main(string[] args)
        {

            var descTablePath = @"txt\idnum2itemdesctable.txt";
            var displayNameTablePath = @"txt\idnum2itemdisplaynametable.txt";

            //データ取得
            var nameTable = GetNameTable(displayNameTablePath,Encoding.GetEncoding("shift-jis"));

            var descTable = GetDescTable(descTablePath);

            //データ結合
            var joinTable = descTable
                .Join(nameTable, d => d.Id, n => n.Id, (desc, name) => new Item(desc, name.Name))
                .ToArray();


            //シリアライズ対象の定義と実行
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
            IList<DescTable> list = new List<DescTable>();
            if (File.Exists(filePath))
            {
                //ファイルを読み込み、表示修飾に用いるテキストを削除
                var readFile = File.ReadLines(filePath, Encoding.GetEncoding("shift-jis"))
                    .Select(val => Regex.Replace(val, @"\^\S{6}", ""))
                    .Select(val => Regex.Replace(val, @"\s?_\s*", ""));

                var fileText = string.Join(Environment.NewLine, readFile);


                //正規表現の実行、メソッド「info」はパターンの共通化
                Func<string, string> info = val => $@"({val}\s*?[:：]\s*?(\S+?))?\s*?";

                var textPattern = $@"(\d+)#\s*([\S\s]+?)\s*#";

                var pattern = new Regex(textPattern);

                var matches = pattern.Matches(fileText).Cast<Match>();

                foreach (var item in matches)
                {

                    var id = matchIntParse(item, 1);
                    var text = matchToString(item, 2);

                    list.Add(new DescTable(id,text));
                }

            }

            return list;
        }
    }
}

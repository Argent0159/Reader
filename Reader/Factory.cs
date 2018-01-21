using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader
{
    internal static class Factory<T> where T : class, new()
    {
        /// <summary>
        /// Null検証として使用。Nullであるならば引数なしコンストラクタで生成したものを返す。
        /// </summary>
        /// <param name="GenericClass"></param>
        /// <returns></returns>
        internal static T Veritifate(T GenericClass)
        {
            return GenericClass ?? new T();
        }
    }


}

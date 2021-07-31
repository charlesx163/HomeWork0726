using HomeWork0726.Common.AttributeExtensions;
using HomeWork0726.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.DateAccess
{
    /// <summary>
    /// 泛型+反射 做缓存
    /// 对于某个T,当第一次执行过静态构造函数后
    /// 对于相同的类的再次执行的时候，就可以拿到字段的值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericSqlHelper<T> where T:BaseModel
    {
        public static string FindSingleSql = null;
        public static string FindAllSql = null;
        static GenericSqlHelper()
        {
            Type type = typeof(T);
            string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]")); //mdoel字段与数据库实体一致,使用特性
            FindSingleSql = $"select {columnString} from [{type.Name}] where Id=";
            FindAllSql = $"select {columnString} from [{type.Name}]";

        }
    }
}

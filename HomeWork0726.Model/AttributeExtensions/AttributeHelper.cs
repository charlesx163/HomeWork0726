using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Model.AttributeExtensions
{
    public static class AttributeHelper
    {
        public static string GetColumnName(this PropertyInfo prop)
        {
            if(prop.IsDefined(typeof(ColumnAttribute)))
            {
                ColumnAttribute attribute = (ColumnAttribute)prop.GetCustomAttribute(typeof(ColumnAttribute));
                return attribute.GetColumn();
            }
            return prop.Name;
        }
    }
}

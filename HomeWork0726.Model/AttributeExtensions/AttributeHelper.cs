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
        /// <summary>
        /// Get the Attribute value of Property
        /// if the property has the attaribute, get the attaribute values as the column name
        /// else get the value of this field as the colimn name
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
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

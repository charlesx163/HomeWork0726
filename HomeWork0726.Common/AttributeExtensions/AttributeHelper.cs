using HomeWork0726.Common.AttributeExtensions.Mapping;
using HomeWork0726.Common.AttributeExtensions.Validate;
using HomeWork0726.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Common.AttributeExtensions
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
            if (prop.IsDefined(typeof(ColumnAttribute)))
            {
                ColumnAttribute attribute = (ColumnAttribute)prop.GetCustomAttribute(typeof(ColumnAttribute));
                return attribute.GetColumn();
            }
            return prop.Name;
        }

        /// <summary>
        /// validate the attribute inherted from the AbstractValidateAttribute
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool Validate<T>(this T o) where T: BaseModel
        {
            Type type = o.GetType();
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                if (prop.IsDefined(typeof(AbstractValidateAttribute)))
                {
                    object[] attarbutes = prop.GetCustomAttributes(typeof(AbstractValidateAttribute),true);
                    foreach(AbstractValidateAttribute attribute in attarbutes)
                    {
                        if (!attribute.Validate(prop.GetValue(o)))
                        {
                            return false;
                            //throw new Exception($"{prop.Name}'s value is {prop.GetValue(o)}, it is not correct");
                        }
                        return attribute.Validate(prop.GetValue(o));
                    }
                }
            }
            return true;
        }
    }
}

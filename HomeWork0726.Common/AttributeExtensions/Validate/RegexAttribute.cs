using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeWork0726.Common.AttributeExtensions.Validate
{
    public class RegexAttribute : AbstractValidateAttribute
    {
        private string _RegexExpression = null;
        public RegexAttribute(string regex)
        {
            this._RegexExpression = regex;
        }

        public override bool Validate(object value)
        {
            return (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? false : Regex.IsMatch(value.ToString(), _RegexExpression);
        }
    }
}

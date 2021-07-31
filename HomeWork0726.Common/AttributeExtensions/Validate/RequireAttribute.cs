using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Common.AttributeExtensions.Validate
{
    public class RequireAttribute : AbstractValidateAttribute
    {
        public RequireAttribute()
        {
        }

        public override bool Validate(object value)
        {
            return (value == null || string.IsNullOrWhiteSpace(value.ToString())) ? false : true;
        }
    }
}

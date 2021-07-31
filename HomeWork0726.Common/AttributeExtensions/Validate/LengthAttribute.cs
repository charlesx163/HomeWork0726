using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Common.AttributeExtensions.Validate
{
    public class LengthAttribute:AbstractValidateAttribute
    {
        private int _Min;
        private int _Max;
        public LengthAttribute(int min,int max)
        {
            this._Max = max;
            this._Min = min;
        }

        public override bool Validate(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return false;
            int length = value.ToString().Length;
            if (length > _Min && length < _Max)
                return true;
            return false;
        }
    }
}

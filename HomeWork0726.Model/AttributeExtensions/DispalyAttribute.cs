using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Model.AttributeExtensions
{
    public class DispalyAttribute : Attribute
    {
        private string _Name = null;
        
        public DispalyAttribute(string name)
        {
            this._Name = name;
        }

        public string GetDisplayName()
        {
            return this._Name;
        }

    }
}

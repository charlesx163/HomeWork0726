﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Common.AttributeExtensions.Mapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute:Attribute
    {
        public ColumnAttribute(string name)
        {
            this._Name = name;
        }

        private string _Name = null;

        public string GetColumn()
        {
            return this._Name;
        }
    }
}

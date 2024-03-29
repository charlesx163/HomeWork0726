﻿using HomeWork0726.Common.AttributeExtensions.Validate;
using HomeWork0726.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.Model.Entities
{
    public class Company : BaseModel
    {
        [Require]
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreatorId { get; set; }
        public int? LastModifierId { get; set; }
        public DateTime? LastModifyTime { get; set; }
    }
}

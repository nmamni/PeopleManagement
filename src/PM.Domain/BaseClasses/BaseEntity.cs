using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Domain.BaseClasses
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}

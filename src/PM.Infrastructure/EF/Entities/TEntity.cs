using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PM.Infrastructure.EF.Entities
{
    public class TEntity
    {
        public TEntity()
        {
            CreateDate = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeleteDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

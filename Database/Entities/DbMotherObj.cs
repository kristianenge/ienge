using System;
using System.ComponentModel.DataAnnotations;

namespace IEnge.Database.Entities
{
    public abstract class DbMotherObj
    {
        [Key]public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication1.DataModels
{
    public class EntitySequence
    {
        [Key]
        public int EntitySequeceId { get; set; }
        public long SequenceNumber { get; set; }
        public int EntityType { get; set; }
        public DateTime CreateDate { get; set; }
        [ForeignKey("EntityType")]
        public virtual ValueTypeModels ValueType { get; set; }
    }
}
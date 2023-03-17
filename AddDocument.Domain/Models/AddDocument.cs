using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AddDocument
    {
        [Key]
        public Guid correlationid { get; set; }
        public Guid? docid { get; set; }
        public Uri? tempbloburl { get; set; }
        public string? permanenturl { get; set; }
        public string? filename { get; set; }
        public string? flag { get; set; }
        public int? totalpagecount { get; set; }
    }
}

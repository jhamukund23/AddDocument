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
        public Guid CorrelationId { get; set; }
        public Guid? DocId { get; set; }
        public string? TempBlobUrl { get; set; }
        public string? PermanentUrl { get; set; }
        public string? FileName { get; set; }
        public string? Flag { get; set; }      
    }
}

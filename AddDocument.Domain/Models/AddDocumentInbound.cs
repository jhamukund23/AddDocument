using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AddDocumentInbound
    {
        public Guid CorrelationId { get; set; }
        public string? FileName { get; set; }
        public string? FileSize { get; set; }

    }
}

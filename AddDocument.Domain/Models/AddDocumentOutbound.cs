using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AddDocumentOutbound
    {
        public Guid CorrelationId { get; set; }
        public string? SasUrl { get; set; }

    }
}

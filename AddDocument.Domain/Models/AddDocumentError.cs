﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AddDocumentError
    {
        public Guid CorrelationId { get; set; }
        public string? Error { get; set; }
    }
}

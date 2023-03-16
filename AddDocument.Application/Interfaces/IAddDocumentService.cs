using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAddDocumentService
    {
        Task<string> AddDocumentAsync(AddDocument model);
        Task<bool> UpdateAddDocumentAsync(AddDocument model);
        Task<IList<AddDocument>> GetAddDocumentAsync();        
        Task<AddDocument?> GetAddDocumentAsync(Guid id);     
        Task<bool> DeleteAddDocumentAsync(AddDocument model);

    }
}

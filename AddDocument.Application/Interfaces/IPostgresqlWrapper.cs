using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.PostgresqlWrapper
{
    public interface IPostgresqlWrapper
    {
        void AddDocument(AddDocument addDocument);
    }
}

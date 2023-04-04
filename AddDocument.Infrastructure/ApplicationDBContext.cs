using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext() { }
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    { }
    public DbSet<AddDocument>? adddocument { get; set; }   


}

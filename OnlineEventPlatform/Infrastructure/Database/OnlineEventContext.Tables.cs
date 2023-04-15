using Domain.Catalog.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class OnlineEventContext
{
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Position> Positions { get; set; } = null!;
    public DbSet<Speaker> Speakers { get; set; } = null!;
    public DbSet<OnlineEvent> OnlineEvents { get; set; } = null!;
}
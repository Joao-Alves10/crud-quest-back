using Microsoft.EntityFrameworkCore;
using QuestAPI.Entities;

namespace QuestAPI.Contexts
{
    public class QuestContext : DbContext
    {
        public QuestContext(DbContextOptions<QuestContext> options) : base(options)
        {
            
        }

        public DbSet<Pergunta> Pergunta { get; set; }
        public DbSet<Tema> Tema { get; set; }
        public DbSet<Alternativa> Alternativa { get; set; }
    }
}

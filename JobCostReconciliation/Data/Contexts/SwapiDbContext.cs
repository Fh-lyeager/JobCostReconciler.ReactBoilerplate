using JobCostReconciliation.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JobCostReconciliation.Data.Contexts
{
    public class SwapiDbContext : DbContext
    {
        public SwapiDbContext() : base("name=AWS_SWAPI")
        {
            Database.SetInitializer<SwapiDbContext>(null);
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<WorkflowQueue> WorkflowQueueItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            EfMapWorkflowQueueItem(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void EfMapWorkflowQueueItem(DbModelBuilder modelBuilder)
        {
            var item = modelBuilder.Entity<WorkflowQueue>();
            item.ToTable("SWAPI_HTTPLog");
            item.HasKey(k => k.Id);
        }

    }
}

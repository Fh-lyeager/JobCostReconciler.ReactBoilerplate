using JobCostReconciliation.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JobCostReconciliation.Data.Contexts
{
    public class SapphireDbContext : DbContext
    {
        public SapphireDbContext() : base("SapphireDbContext")
        {
            Database.SetInitializer<SapphireDbContext>(null);
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            EfMapWorkflow(modelBuilder);
            EfMapJob(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void EfMapWorkflow(DbModelBuilder modelBuilder)
        {
            var workflow = modelBuilder.Entity<Workflow>();
            workflow.ToTable("WFlows");
            workflow.HasKey(k => k.WFlowRID);
        }

        private static void EfMapJob(DbModelBuilder modelBuilder)
        {
            var job = modelBuilder.Entity<Job>();
            job.ToTable("Jobs");
            job.HasKey(k => k.JobRID);
        }
    }
}

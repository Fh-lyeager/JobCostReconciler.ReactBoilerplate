using JobCostReconciliation.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JobCostReconciliation.Data.Contexts
{
    public class ProcessorDbContext : DbContext
    {
        public ProcessorDbContext() : base("AWS_DIProcessor")
        {
            Database.SetInitializer<ProcessorDbContext>(null);
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<PurchaseOrderLastRun> PurchaseOrderLastRuns { get; set; }
        public DbSet<PurchaseOrderQueue> PurchaseOrderQueueItems { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            EfMapCompany(modelBuilder);
            EfMapPurchaseOrderLastRun(modelBuilder);
            EfMapPurchaseOrderQueueItem(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }

        private static void EfMapCompany(DbModelBuilder modelBuilder)
        {
            var comp = modelBuilder.Entity<Company>();
            comp.HasKey(k => k.CompanyId);
        }

        private static void EfMapPurchaseOrderLastRun(DbModelBuilder modelBuilder)
        {
            var lastRun = modelBuilder.Entity<PurchaseOrderLastRun>();
            lastRun.ToTable("NextRun");
            lastRun.HasKey(k => k.NextRunId);
        }

        private static void EfMapPurchaseOrderQueueItem(DbModelBuilder modelBuilder)
        {
            var item = modelBuilder.Entity<PurchaseOrderQueue>();
            item.ToTable("PO_Queue");
        }
    }
}

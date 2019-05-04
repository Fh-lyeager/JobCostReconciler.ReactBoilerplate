using JobCostReconciliation.DataContext.Sapphire.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JobCostReconciliation.DataContext.Sapphire
{
    public class SapphireDbContext : DbContext
    {
        public SapphireDbContext() : base("SapphireDbContext")
        {
            Database.SetInitializer<SapphireDbContext>(null);
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobPO> JobPos { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<JobScheduleActivity> JobScheduleActivities { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            EfMapActivity(modelBuilder);
            EfMapJob(modelBuilder);
            EfMapJobPo(modelBuilder);
            EfMapJobScheduleActivity(modelBuilder);
            EfMapLot(modelBuilder);
            EfMapCommunity(modelBuilder);
            EfMapVendor(modelBuilder);
            EfMapWorkflow(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void EfMapActivity(DbModelBuilder modelBuilder)
        {
            var act = modelBuilder.Entity<Activity>();
            act.ToTable("Acts");
            act.HasKey(k => k.ActRID);
        }

        private static void EfMapCommunity(DbModelBuilder modelBuilder)
        {
            var com = modelBuilder.Entity<Community>();
            com.ToTable("Communities");
            com.HasKey(k => k.CommunityRID);
        }

        private static void EfMapJob(DbModelBuilder modelBuilder)
        {
            var job = modelBuilder.Entity<Job>();
            job.ToTable("Jobs");
            job.HasKey(k => k.JobRID);
        }

        private static void EfMapJobPo(DbModelBuilder modelBuilder)
        {
            var po = modelBuilder.Entity<JobPO>();
            po.ToTable("JobPOs");
            po.HasKey(k => k.JobPORID);
            po.Property(p => p.AmtTaxable).HasColumnType("Money");
        }

        private static void EfMapJobScheduleActivity(DbModelBuilder modelBuilder)
        {
            var jsa = modelBuilder.Entity<JobScheduleActivity>();
            jsa.ToTable("JobSchActs");
            jsa.HasKey(k => k.JobSchActRID);
        }

        private static void EfMapLot(DbModelBuilder modelBuilder)
        {
            var lot = modelBuilder.Entity<Lot>();
            lot.ToTable("Lots");
            lot.HasKey(k => k.LotRID);
        }

        private static void EfMapVendor(DbModelBuilder modelBuilder)
        {
            var vnd = modelBuilder.Entity<Vendor>();
            vnd.ToTable("Vnds");
            vnd.HasKey(k => k.VndRID);
        }

        private static void EfMapWorkflow(DbModelBuilder modelBuilder)
        {
            var workflow = modelBuilder.Entity<Workflow>();
            workflow.ToTable("WFlows");
            workflow.HasKey(k => k.WFlowRID);
        }
    }
}

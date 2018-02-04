using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using HojasDeVida.EntityFramework;

namespace HojasDeVida.Migrator
{
    [DependsOn(typeof(HojasDeVidaDataModule))]
    public class HojasDeVidaMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<HojasDeVidaDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
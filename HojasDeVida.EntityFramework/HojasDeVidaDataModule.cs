using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using HojasDeVida.EntityFramework;

namespace HojasDeVida
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(HojasDeVidaCoreModule))]
    public class HojasDeVidaDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<HojasDeVidaDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

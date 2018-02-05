using System.Reflection;
using Abp.Modules;
using Abp.Zero;

namespace HojasDeVida
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class HojasDeVidaUtilitiesModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}

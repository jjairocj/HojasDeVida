using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using HojasDeVida.Authorization.Roles;
using HojasDeVida.Authorization.Users;
using HojasDeVida.MultiTenancy;
using HojasDeVida.Trabajadores;

namespace HojasDeVida.EntityFramework
{
    public class HojasDeVidaDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...


        public virtual IDbSet<Trabajador> Trabajador { get; set; }
        public virtual IDbSet<TrabajadorExperienciaLaboral> TrabajadorExperienciaLaboral { get; set; }
        public virtual IDbSet<TrabajadorEducacion> TrabajadorEducacion { get; set; }
        public virtual IDbSet<TrabajadorReferencia> TrabajadorReferencia { get; set; }
        public virtual IDbSet<TrabajadorAptitud> TrabajadorAptitud { get; set; }
        public virtual IDbSet<TrabajadorCualificacion> TrabajadorCualificacion { get; set; }
        public virtual IDbSet<TrabajadorInteres> TrabajadorInteres { get; set; }


        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public HojasDeVidaDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in HojasDeVidaDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of HojasDeVidaDbContext since ABP automatically handles it.
         */
        public HojasDeVidaDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public HojasDeVidaDbContext(DbConnection existingConnection)
         : base(existingConnection, false)
        {

        }

        public HojasDeVidaDbContext(DbConnection existingConnection, bool contextOwnsConnection)
         : base(existingConnection, contextOwnsConnection)
        {

        }
    }
}

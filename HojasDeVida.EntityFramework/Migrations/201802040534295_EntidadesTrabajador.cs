namespace HojasDeVida.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class EntidadesTrabajador : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trabajador",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        FotoPerfil = c.String(),
                        Nombre = c.String(maxLength: 512),
                        Apellidos = c.String(maxLength: 512),
                        NombreCompleto = c.String(maxLength: 1024),
                        Celular = c.String(maxLength: 50),
                        CorreoElectronico = c.String(maxLength: 512),
                        IdentificacionTipo = c.String(maxLength: 512),
                        IdentificacionNumero = c.String(maxLength: 50),
                        Departamento = c.String(maxLength: 512),
                        DepartamentoCodigo = c.String(maxLength: 512),
                        Municipio = c.String(maxLength: 512),
                        MunicipioCodigo = c.String(maxLength: 512),
                        Direccion = c.String(maxLength: 512),
                        Estrato = c.Int(nullable: false),
                        SitioId = c.String(maxLength: 512),
                        EstadoGeolocalizacion = c.String(maxLength: 512),
                        Telefono = c.String(maxLength: 512),
                        FechaNacimiento = c.DateTime(),
                        Sexo = c.String(maxLength: 512),
                        EstadoCivil = c.String(maxLength: 512),
                        NumeroHijosDependientes = c.Int(),
                        NivelEducativo = c.String(maxLength: 512),
                        AspiracionSalarial = c.String(maxLength: 512),
                        MonedaAspiracionSalarial = c.String(maxLength: 512),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Trabajador_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UsuarioId, unique: true);
            
            CreateTable(
                "dbo.TrabajadorAptitud",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        Aptitud = c.String(maxLength: 128),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Trabajador_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorAptitud_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trabajador", t => t.Trabajador_Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Trabajador_Id);
            
            CreateTable(
                "dbo.TrabajadorCualificacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        Descripcion = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Trabajador_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorCualificacion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trabajador", t => t.Trabajador_Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Trabajador_Id);
            
            CreateTable(
                "dbo.TrabajadorEducacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        TipoFormacion = c.String(nullable: false, maxLength: 512),
                        Institucion = c.String(maxLength: 512),
                        Nombre = c.String(nullable: false, maxLength: 512),
                        FechaInicial = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(),
                        Descripcion = c.String(maxLength: 1024),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Trabajador_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorEducacion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trabajador", t => t.Trabajador_Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Trabajador_Id);
            
            CreateTable(
                "dbo.TrabajadorExperienciaLaboral",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        NombreEmpresa = c.String(nullable: false, maxLength: 512),
                        Cargo = c.String(nullable: false, maxLength: 512),
                        Departamento = c.String(maxLength: 512),
                        DepartamentoCodigo = c.String(maxLength: 512),
                        Municipio = c.String(maxLength: 512),
                        MunicipioCodigo = c.String(maxLength: 512),
                        FechaInicial = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(),
                        Descripcion = c.String(maxLength: 1024),
                        NombreJefe = c.String(maxLength: 512),
                        ContactoJefe = c.String(maxLength: 512),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Trabajador_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorExperienciaLaboral_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trabajador", t => t.Trabajador_Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Trabajador_Id);
            
            CreateTable(
                "dbo.TrabajadorInteres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        Descripcion = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Trabajador_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorInteres_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trabajador", t => t.Trabajador_Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Trabajador_Id);
            
            CreateTable(
                "dbo.TrabajadorReferencia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(maxLength: 128),
                        Descripcion = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleterUserId = c.Long(),
                        DeletionTime = c.DateTime(),
                        LastModificationTime = c.DateTime(),
                        LastModifierUserId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                        Trabajador_Id = c.Guid(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorReferencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trabajador", t => t.Trabajador_Id)
                .Index(t => t.UsuarioId)
                .Index(t => t.Trabajador_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrabajadorReferencia", "Trabajador_Id", "dbo.Trabajador");
            DropForeignKey("dbo.TrabajadorInteres", "Trabajador_Id", "dbo.Trabajador");
            DropForeignKey("dbo.TrabajadorExperienciaLaboral", "Trabajador_Id", "dbo.Trabajador");
            DropForeignKey("dbo.TrabajadorEducacion", "Trabajador_Id", "dbo.Trabajador");
            DropForeignKey("dbo.TrabajadorCualificacion", "Trabajador_Id", "dbo.Trabajador");
            DropForeignKey("dbo.TrabajadorAptitud", "Trabajador_Id", "dbo.Trabajador");
            DropIndex("dbo.TrabajadorReferencia", new[] { "Trabajador_Id" });
            DropIndex("dbo.TrabajadorReferencia", new[] { "UsuarioId" });
            DropIndex("dbo.TrabajadorInteres", new[] { "Trabajador_Id" });
            DropIndex("dbo.TrabajadorInteres", new[] { "UsuarioId" });
            DropIndex("dbo.TrabajadorExperienciaLaboral", new[] { "Trabajador_Id" });
            DropIndex("dbo.TrabajadorExperienciaLaboral", new[] { "UsuarioId" });
            DropIndex("dbo.TrabajadorEducacion", new[] { "Trabajador_Id" });
            DropIndex("dbo.TrabajadorEducacion", new[] { "UsuarioId" });
            DropIndex("dbo.TrabajadorCualificacion", new[] { "Trabajador_Id" });
            DropIndex("dbo.TrabajadorCualificacion", new[] { "UsuarioId" });
            DropIndex("dbo.TrabajadorAptitud", new[] { "Trabajador_Id" });
            DropIndex("dbo.TrabajadorAptitud", new[] { "UsuarioId" });
            DropIndex("dbo.Trabajador", new[] { "UsuarioId" });
            DropTable("dbo.TrabajadorReferencia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorReferencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TrabajadorInteres",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorInteres_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TrabajadorExperienciaLaboral",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorExperienciaLaboral_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TrabajadorEducacion",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorEducacion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TrabajadorCualificacion",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorCualificacion_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TrabajadorAptitud",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TrabajadorAptitud_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Trabajador",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Trabajador_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}

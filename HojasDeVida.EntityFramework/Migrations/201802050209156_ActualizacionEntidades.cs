namespace HojasDeVida.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionEntidades : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Trabajador", "AspiracionSalarial", c => c.Int());
            AlterColumn("dbo.TrabajadorCualificacion", "Descripcion", c => c.String(maxLength: 128));
            AlterColumn("dbo.TrabajadorEducacion", "Descripcion", c => c.String());
            AlterColumn("dbo.TrabajadorExperienciaLaboral", "Descripcion", c => c.String());
            AlterColumn("dbo.TrabajadorInteres", "Descripcion", c => c.String(maxLength: 128));
            DropColumn("dbo.Trabajador", "Estrato");
            DropColumn("dbo.Trabajador", "SitioId");
            DropColumn("dbo.Trabajador", "EstadoGeolocalizacion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trabajador", "EstadoGeolocalizacion", c => c.String(maxLength: 512));
            AddColumn("dbo.Trabajador", "SitioId", c => c.String(maxLength: 512));
            AddColumn("dbo.Trabajador", "Estrato", c => c.Int(nullable: false));
            AlterColumn("dbo.TrabajadorInteres", "Descripcion", c => c.String());
            AlterColumn("dbo.TrabajadorExperienciaLaboral", "Descripcion", c => c.String(maxLength: 1024));
            AlterColumn("dbo.TrabajadorEducacion", "Descripcion", c => c.String(maxLength: 1024));
            AlterColumn("dbo.TrabajadorCualificacion", "Descripcion", c => c.String());
            AlterColumn("dbo.Trabajador", "AspiracionSalarial", c => c.String(maxLength: 512));
        }
    }
}

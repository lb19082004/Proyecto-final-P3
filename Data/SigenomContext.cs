using Microsoft.EntityFrameworkCore;
using SIGENOM.Models.Entities;

namespace SIGENOM.Data;

public class SigenomContext : DbContext
{
    public SigenomContext(DbContextOptions<SigenomContext> options) : base(options) { }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<TipoConcepto> TiposConcepto { get; set; }
    public DbSet<ConceptoNomina> ConceptosNomina { get; set; }
    public DbSet<PeriodoNomina> PeriodosNomina { get; set; }
    public DbSet<NominaEmpleado> NominaEmpleados { get; set; }
    public DbSet<DetalleNomina> DetallesNomina { get; set; }
    public DbSet<ComprobantePago> ComprobantesPago { get; set; }
    public DbSet<Bitacora> Bitacora { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>()
            .Property(c => c.SalarioBase).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<Empleado>()
            .Property(e => e.SalarioBase).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<NominaEmpleado>()
            .Property(n => n.SalarioBase).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<NominaEmpleado>()
            .Property(n => n.TotalIngresos).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<NominaEmpleado>()
            .Property(n => n.TotalDeducciones).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<NominaEmpleado>()
            .Property(n => n.SalarioBruto).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<NominaEmpleado>()
            .Property(n => n.SalarioNeto).HasColumnType("decimal(12,2)");
        modelBuilder.Entity<DetalleNomina>()
            .Property(d => d.Monto).HasColumnType("decimal(12,2)");
    }
}
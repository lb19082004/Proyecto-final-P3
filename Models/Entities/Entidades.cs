using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGENOM.Models.Entities;

[Table("Roles")]
public class Rol
{
    [Key] public int RolId { get; set; }
    [Required, MaxLength(50)] public string Nombre { get; set; } = "";
    [MaxLength(200)] public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
    public ICollection<Usuario> Usuarios { get; set; } = [];
}

[Table("Usuarios")]
public class Usuario
{
    [Key] public int UsuarioId { get; set; }
    public int RolId { get; set; }
    [Required, MaxLength(60)] public string NombreUsuario { get; set; } = "";
    [Required, MaxLength(256)] public string PasswordHash { get; set; } = "";
    [Required, MaxLength(120)] public string Email { get; set; } = "";
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    [ForeignKey("RolId")] public Rol? Rol { get; set; }
}

[Table("Departamentos")]
public class Departamento
{
    [Key] public int DepartamentoId { get; set; }
    [Required, MaxLength(100)] public string Nombre { get; set; } = "";
    [MaxLength(300)] public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
    public ICollection<Cargo> Cargos { get; set; } = [];
    public ICollection<Empleado> Empleados { get; set; } = [];
}

[Table("Cargos")]
public class Cargo
{
    [Key] public int CargoId { get; set; }
    public int DepartamentoId { get; set; }
    [Required, MaxLength(100)] public string Nombre { get; set; } = "";
    [MaxLength(300)] public string? Descripcion { get; set; }
    public decimal SalarioBase { get; set; }
    public bool Activo { get; set; } = true;
    [ForeignKey("DepartamentoId")] public Departamento? Departamento { get; set; }
    public ICollection<Empleado> Empleados { get; set; } = [];
}

[Table("Empleados")]
public class Empleado
{
    [Key] public int EmpleadoId { get; set; }
    public int CargoId { get; set; }
    public int DepartamentoId { get; set; }
    [Required, MaxLength(20)] public string Cedula { get; set; } = "";
    [Required, MaxLength(100)] public string Nombres { get; set; } = "";
    [Required, MaxLength(100)] public string Apellidos { get; set; } = "";
    [MaxLength(120)] public string? Email { get; set; }
    [MaxLength(20)] public string? Telefono { get; set; }
    [MaxLength(300)] public string? Direccion { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public decimal SalarioBase { get; set; }
    [MaxLength(20)] public string Estado { get; set; } = "Activo";
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    [ForeignKey("CargoId")] public Cargo? Cargo { get; set; }
    [ForeignKey("DepartamentoId")] public Departamento? Departamento { get; set; }
    public string NombreCompleto => $"{Nombres} {Apellidos}";
}

[Table("TiposConcepto")]
public class TipoConcepto
{
    [Key] public int TipoConceptoId { get; set; }
    [Required, MaxLength(60)] public string Nombre { get; set; } = "";
    [Required, MaxLength(20)] public string Clasificacion { get; set; } = "";
    public ICollection<ConceptoNomina> ConceptosNomina { get; set; } = [];
}

[Table("ConceptosNomina")]
public class ConceptoNomina
{
    [Key] public int ConceptoId { get; set; }
    public int TipoConceptoId { get; set; }
    [Required, MaxLength(100)] public string Nombre { get; set; } = "";
    [MaxLength(300)] public string? Descripcion { get; set; }
    public bool EsFijo { get; set; }
    public bool Activo { get; set; } = true;
    [ForeignKey("TipoConceptoId")] public TipoConcepto? TipoConcepto { get; set; }
}

[Table("PeriodosNomina")]
public class PeriodoNomina
{
    [Key] public int PeriodoId { get; set; }
    public int UsuarioId { get; set; }
    [Required, MaxLength(150)] public string Descripcion { get; set; } = "";
    public DateOnly FechaInicio { get; set; }
    public DateOnly FechaFin { get; set; }
    [MaxLength(20)] public string Estado { get; set; } = "Abierto";
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    [ForeignKey("UsuarioId")] public Usuario? Usuario { get; set; }
    public ICollection<NominaEmpleado> NominaEmpleados { get; set; } = [];
}

[Table("NominaEmpleado")]
public class NominaEmpleado
{
    [Key] public int NominaEmpleadoId { get; set; }
    public int PeriodoId { get; set; }
    public int EmpleadoId { get; set; }
    public decimal SalarioBase { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalDeducciones { get; set; }
    public decimal SalarioBruto { get; set; }
    public decimal SalarioNeto { get; set; }
    [MaxLength(20)] public string Estado { get; set; } = "Pendiente";
    public DateTime? FechaCalculo { get; set; }
    [ForeignKey("PeriodoId")] public PeriodoNomina? Periodo { get; set; }
    [ForeignKey("EmpleadoId")] public Empleado? Empleado { get; set; }
    public ICollection<DetalleNomina> Detalles { get; set; } = [];
    public ComprobantePago? Comprobante { get; set; }
}

[Table("DetalleNomina")]
public class DetalleNomina
{
    [Key] public int DetalleId { get; set; }
    public int NominaEmpleadoId { get; set; }
    public int ConceptoId { get; set; }
    public decimal Monto { get; set; }
    [MaxLength(300)] public string? Observacion { get; set; }
    [ForeignKey("NominaEmpleadoId")] public NominaEmpleado? NominaEmpleado { get; set; }
    [ForeignKey("ConceptoId")] public ConceptoNomina? Concepto { get; set; }
}

[Table("ComprobantesPago")]
public class ComprobantePago
{
    [Key] public int ComprobanteId { get; set; }
    public int NominaEmpleadoId { get; set; }
    [Required, MaxLength(30)] public string NumeroComprobante { get; set; } = "";
    public DateTime FechaEmision { get; set; } = DateTime.Now;
    public bool Anulado { get; set; }
    [ForeignKey("NominaEmpleadoId")] public NominaEmpleado? NominaEmpleado { get; set; }
}

[Table("Bitacora")]
public class Bitacora
{
    [Key] public int BitacoraId { get; set; }
    public int? UsuarioId { get; set; }
    [Required, MaxLength(60)] public string Accion { get; set; } = "";
    [Required, MaxLength(60)] public string Modulo { get; set; } = "";
    [MaxLength(500)] public string? Descripcion { get; set; }
    public DateTime FechaHora { get; set; } = DateTime.Now;
    [MaxLength(45)] public string? IPAddress { get; set; }
    [ForeignKey("UsuarioId")] public Usuario? Usuario { get; set; }
}
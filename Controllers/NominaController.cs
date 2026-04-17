using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGENOM.Data;
using SIGENOM.Models.Entities;
using System.Security.Claims;

namespace SIGENOM.Controllers;

[Authorize]
public class NominaController : Controller
{
    private readonly SigenomContext _db;
    public NominaController(SigenomContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var periodos = await _db.PeriodosNomina
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
        return View(periodos);
    }

    public IActionResult CrearPeriodo() => View();

    [HttpPost]
    public async Task<IActionResult> CrearPeriodo(PeriodoNomina periodo)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        periodo.UsuarioId = usuarioId;
        if (ModelState.IsValid)
        {
            _db.PeriodosNomina.Add(periodo);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Período creado.";
            return RedirectToAction(nameof(Index));
        }
        return View(periodo);
    }

    public async Task<IActionResult> Procesar(int id)
    {
        var periodo = await _db.PeriodosNomina.FindAsync(id);
        if (periodo == null || periodo.Estado != "Abierto")
            return RedirectToAction(nameof(Index));

        var empleados = await _db.Empleados
            .Where(e => e.Estado == "Activo")
            .ToListAsync();

        var conceptosFijos = await _db.ConceptosNomina
            .Include(c => c.TipoConcepto)
            .Where(c => c.EsFijo && c.Activo)
            .ToListAsync();

        foreach (var emp in empleados)
        {
            var yaExiste = await _db.NominaEmpleados
                .AnyAsync(n => n.PeriodoId == id && n.EmpleadoId == emp.EmpleadoId);
            if (yaExiste) continue;

            var nomina = new NominaEmpleado
            {
                PeriodoId = id,
                EmpleadoId = emp.EmpleadoId,
                SalarioBase = emp.SalarioBase,
                Estado = "Pendiente"
            };

            decimal ingresos = emp.SalarioBase;
            decimal deducciones = 0;
            var detalles = new List<DetalleNomina>();

            foreach (var concepto in conceptosFijos)
            {
                decimal monto = 0;
                if (concepto.Nombre.Contains("AFP")) monto = emp.SalarioBase * 0.03m;
                else if (concepto.Nombre.Contains("SFS")) monto = emp.SalarioBase * 0.0304m;
                else continue;

                detalles.Add(new DetalleNomina { ConceptoId = concepto.ConceptoId, Monto = monto });

                if (concepto.TipoConcepto?.Clasificacion == "Deduccion") deducciones += monto;
                else ingresos += monto;
            }

            nomina.TotalIngresos = ingresos;
            nomina.TotalDeducciones = deducciones;
            nomina.SalarioBruto = ingresos;
            nomina.SalarioNeto = ingresos - deducciones;
            nomina.Estado = "Calculado";
            nomina.FechaCalculo = DateTime.Now;
            nomina.Detalles = detalles;

            _db.NominaEmpleados.Add(nomina);
        }

        await _db.SaveChangesAsync();
        TempData["OK"] = "Nómina procesada correctamente.";
        return RedirectToAction(nameof(Detalle), new { id });
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var periodo = await _db.PeriodosNomina.FindAsync(id);
        if (periodo == null) return NotFound();

        var nominas = await _db.NominaEmpleados
            .Include(n => n.Empleado).ThenInclude(e => e!.Departamento)
            .Include(n => n.Empleado).ThenInclude(e => e!.Cargo)
            .Include(n => n.Detalles).ThenInclude(d => d.Concepto)
            .Where(n => n.PeriodoId == id)
            .ToListAsync();

        ViewBag.Periodo = periodo;
        return View(nominas);
    }

    [HttpPost]
    public async Task<IActionResult> Aprobar(int id)
    {
        var periodo = await _db.PeriodosNomina.FindAsync(id);
        if (periodo == null) return NotFound();

        var nominas = await _db.NominaEmpleados
            .Where(n => n.PeriodoId == id && n.Estado == "Calculado")
            .ToListAsync();

        foreach (var n in nominas)
        {
            n.Estado = "Aprobado";
            var comprobante = new ComprobantePago
            {
                NominaEmpleadoId = n.NominaEmpleadoId,
                NumeroComprobante = $"COMP-{id:D4}-{n.EmpleadoId:D4}",
                FechaEmision = DateTime.Now
            };
            _db.ComprobantesPago.Add(comprobante);
        }

        periodo.Estado = "Cerrado";
        await _db.SaveChangesAsync();
        TempData["OK"] = "Nómina aprobada y comprobantes generados.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Comprobante(int id)
    {
        var comp = await _db.ComprobantesPago
            .Include(c => c.NominaEmpleado).ThenInclude(n => n!.Empleado)
                .ThenInclude(e => e!.Departamento)
            .Include(c => c.NominaEmpleado).ThenInclude(n => n!.Empleado)
                .ThenInclude(e => e!.Cargo)
            .Include(c => c.NominaEmpleado).ThenInclude(n => n!.Periodo)
            .Include(c => c.NominaEmpleado).ThenInclude(n => n!.Detalles)
                .ThenInclude(d => d.Concepto).ThenInclude(c => c!.TipoConcepto)
            .FirstOrDefaultAsync(c => c.NominaEmpleadoId == id);

        if (comp == null) return NotFound();
        return View(comp);
    }
}
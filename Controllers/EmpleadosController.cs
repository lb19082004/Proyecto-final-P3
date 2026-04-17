using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGENOM.Data;
using SIGENOM.Models.Entities;

namespace SIGENOM.Controllers;

[Authorize]
public class EmpleadosController : Controller
{
    private readonly SigenomContext _db;
    public EmpleadosController(SigenomContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var empleados = await _db.Empleados
            .Include(e => e.Departamento)
            .Include(e => e.Cargo)
            .ToListAsync();
        return View(empleados);
    }

    public async Task<IActionResult> Crear()
    {
        await CargarSelectLists();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Empleado emp)
    {
        if (await _db.Empleados.AnyAsync(e => e.Cedula == emp.Cedula))
        {
            ModelState.AddModelError("Cedula", "Ya existe un empleado con esa cédula.");
        }
        if (ModelState.IsValid)
        {
            _db.Empleados.Add(emp);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Empleado registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        await CargarSelectLists();
        return View(emp);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var emp = await _db.Empleados.FindAsync(id);
        if (emp == null) return NotFound();
        await CargarSelectLists();
        return View(emp);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Empleado emp)
    {
        if (ModelState.IsValid)
        {
            _db.Empleados.Update(emp);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Empleado actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        await CargarSelectLists();
        return View(emp);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var emp = await _db.Empleados
            .Include(e => e.Departamento)
            .Include(e => e.Cargo)
            .FirstOrDefaultAsync(e => e.EmpleadoId == id);
        if (emp == null) return NotFound();
        return View(emp);
    }

    [HttpPost]
    public async Task<IActionResult> CambiarEstado(int id, string estado)
    {
        var emp = await _db.Empleados.FindAsync(id);
        if (emp != null)
        {
            emp.Estado = estado;
            await _db.SaveChangesAsync();
            TempData["OK"] = $"Estado cambiado a {estado}.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarSelectLists()
    {
        ViewBag.Departamentos = new SelectList(
            await _db.Departamentos.Where(d => d.Activo).ToListAsync(),
            "DepartamentoId", "Nombre");
        ViewBag.Cargos = new SelectList(
            await _db.Cargos.Where(c => c.Activo).ToListAsync(),
            "CargoId", "Nombre");
    }
}
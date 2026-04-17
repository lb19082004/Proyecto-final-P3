using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIGENOM.Data;
using SIGENOM.Models.Entities;

namespace SIGENOM.Controllers;

[Authorize]
public class CargosController : Controller
{
    private readonly SigenomContext _db;
    public CargosController(SigenomContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var lista = await _db.Cargos.Include(c => c.Departamento).ToListAsync();
        return View(lista);
    }

    public async Task<IActionResult> Crear()
    {
        await CargarDepartamentos();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Cargo cargo)
    {
        if (ModelState.IsValid)
        {
            _db.Cargos.Add(cargo);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Cargo creado.";
            return RedirectToAction(nameof(Index));
        }
        await CargarDepartamentos();
        return View(cargo);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var cargo = await _db.Cargos.FindAsync(id);
        if (cargo == null) return NotFound();
        await CargarDepartamentos();
        return View(cargo);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Cargo cargo)
    {
        if (ModelState.IsValid)
        {
            _db.Cargos.Update(cargo);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Cargo actualizado.";
            return RedirectToAction(nameof(Index));
        }
        await CargarDepartamentos();
        return View(cargo);
    }

    private async Task CargarDepartamentos()
    {
        ViewBag.Departamentos = new SelectList(
            await _db.Departamentos.Where(d => d.Activo).ToListAsync(),
            "DepartamentoId", "Nombre");
    }
}
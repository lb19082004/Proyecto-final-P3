using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGENOM.Data;
using SIGENOM.Models.Entities;

namespace SIGENOM.Controllers;

[Authorize]
public class DepartamentosController : Controller
{
    private readonly SigenomContext _db;
    public DepartamentosController(SigenomContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        var lista = await _db.Departamentos.ToListAsync();
        return View(lista);
    }

    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Departamento dep)
    {
        if (ModelState.IsValid)
        {
            _db.Departamentos.Add(dep);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Departamento creado.";
            return RedirectToAction(nameof(Index));
        }
        return View(dep);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var dep = await _db.Departamentos.FindAsync(id);
        if (dep == null) return NotFound();
        return View(dep);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Departamento dep)
    {
        if (ModelState.IsValid)
        {
            _db.Departamentos.Update(dep);
            await _db.SaveChangesAsync();
            TempData["OK"] = "Departamento actualizado.";
            return RedirectToAction(nameof(Index));
        }
        return View(dep);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGENOM.Data;

namespace SIGENOM.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly SigenomContext _db;
    public DashboardController(SigenomContext db) { _db = db; }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalEmpleados = await _db.Empleados.CountAsync(e => e.Estado == "Activo");
        ViewBag.TotalDepartamentos = await _db.Departamentos.CountAsync(d => d.Activo);
        ViewBag.PeriodosAbiertos = await _db.PeriodosNomina.CountAsync(p => p.Estado == "Abierto");
        return View();
    }
}
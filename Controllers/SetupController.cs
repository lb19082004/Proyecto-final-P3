using Microsoft.AspNetCore.Mvc;
using SIGENOM.Data;
using Microsoft.EntityFrameworkCore;

namespace SIGENOM.Controllers;

public class SetupController : Controller
{
    private readonly SigenomContext _db;
    public SetupController(SigenomContext db) { _db = db; }

    public async Task<IActionResult> FixAdmin()
    {
        var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == "admin");
        if (user != null)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@2025");
            await _db.SaveChangesAsync();
            return Content("✅ Password actualizado. Ya puedes hacer login.");
        }
        return Content("❌ Usuario admin no encontrado.");
    }
}
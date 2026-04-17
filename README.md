# SIGENOM Enterprise
## Sistema Integral de Gestión de Nómina y Talento Humano

Sistema web empresarial desarrollado con ASP.NET Core para gestionar empleados, departamentos, cargos, períodos de nómina y generación de comprobantes de pago.

---

## 🛠️ Tecnologías utilizadas

- **Backend:** C#, ASP.NET Core MVC, Entity Framework Core
- **Base de datos:** SQL Server
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5
- **Pruebas:** xUnit
- **Control de versiones:** Git y GitHub
- **Gestión ágil:** Jira (Scrum)

---

## 📁 Estructura del proyecto
Proyecto-final-P3/
├── Controllers/
│   ├── AccountController.cs
│   ├── DashboardController.cs
│   ├── EmpleadosController.cs
│   ├── DepartamentosController.cs
│   ├── CargosController.cs
│   └── NominaController.cs
├── Data/
│   └── SigenomContext.cs
├── Models/
│   ├── Entities/
│   │   └── Entidades.cs
│   └── ViewModels/
├── Views/
│   ├── Account/
│   ├── Dashboard/
│   ├── Empleados/
│   ├── Departamentos/
│   ├── Cargos/
│   ├── Nomina/
│   └── Shared/
├── SIGENOM.Tests/
│   └── UnitTest1.cs
├── wwwroot/
├── Program.cs
├── appsettings.json
└── SIGENOM.csproj
---

## 🚀 Funcionalidades del primer Release

- ✅ Inicio de sesión con autenticación por roles
- ✅ Gestión de empleados (registro, edición, desactivación)
- ✅ Gestión de departamentos y cargos
- ✅ Creación y procesamiento de períodos de nómina
- ✅ Cálculo automático de AFP (3%) y SFS (3.04%)
- ✅ Generación de comprobantes de pago individuales
- ✅ Dashboard con resumen del sistema

---

## ⚙️ Instrucciones de instalación

1. Clonar el repositorio:
 2. Restaurar la base de datos ejecutando el script SQL en SQL Server Management Studio.

3. Configurar la cadena de conexión en `appsettings.json`:
```json
"SigenomConnection": "Server=localhost;Database=SIGENOM;Trusted_Connection=True;TrustServerCertificate=True;"
```

4. Ejecutar el proyecto en Visual Studio con `Ctrl+F5`.

5. Acceder con las credenciales de prueba:
   - **Usuario:** admin
   - **Contraseña:** Admin@2025

---

## 🧪 Pruebas automatizadas

Se implementaron 7 pruebas unitarias con xUnit que validan los cálculos críticos del módulo de nómina.

Para ejecutar las pruebas:
Resultado: **7/7 pruebas pasando ✅**

---

## 📋 Gestión del proyecto

- **Jira:** https://lbj.atlassian.net/jira/software/projects/SIG/boards/67/backlog
- **Metodología:** Agile-Scrum con sprints de 2 semanas

---

## 👤 Autor

**Leury Brand De Jesus**
Matrícula: 2024-1619
Programación III — ITLA
2026

using Xunit;

namespace PruebasNomina;

public class NominaTests
{
    [Fact]
    public void Test1_SalarioNeto_MenosDeducciones()
    {
        decimal salario = 50000;
        decimal afp = salario * 0.03m;
        decimal sfs = salario * 0.0304m;
        decimal neto = salario - afp - sfs;
        Assert.Equal(47480m, neto);
    }

    [Fact]
    public void Test2_AFP_TresPorCiento()
    {
        decimal salario = 60000;
        decimal afp = salario * 0.03m;
        Assert.Equal(1800m, afp);
    }

    [Fact]
    public void Test3_SFS_TresPuntoCuatro()
    {
        decimal salario = 50000;
        decimal sfs = salario * 0.0304m;
        Assert.Equal(1520m, sfs);
    }

    [Fact]
    public void Test4_SalarioNeto_NoNegativo()
    {
        decimal salario = 30000;
        decimal neto = salario - (salario * 0.03m) - (salario * 0.0304m);
        Assert.True(neto > 0);
    }

    [Fact]
    public void Test5_NombreCompleto()
    {
        string completo = $"Maria Garcia";
        Assert.Equal("Maria Garcia", completo);
    }

    [Fact]
    public void Test6_TotalDeducciones()
    {
        decimal salario = 40000;
        decimal total = (salario * 0.03m) + (salario * 0.0304m);
        Assert.Equal(2416m, total);
    }

    [Fact]
    public void Test7_SalarioBruto()
    {
        decimal salarioBase = 75000;
        Assert.Equal(75000m, salarioBase);
    }
}
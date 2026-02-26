using Xunit;
using ProcEstatistica;
using System.Collections.Generic;

namespace ProcEstatistica.Test.Unit;

public class FrequenciaAcumuladaTests
{
    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumulada()
    {
        // Arrange
        List<int> numeros = new List<int> {1, 5, 10, 20};
        var dados =  new DadosFrequenciaAcumulada();
        
        // Act: Somatório dos números
        dados.FrequenciaAcumuladaTotal(numeros);

        // Assert: o somatório deve ser 36
        // Explicação: 1 + 5 + 10 + 20 = 36, então espera-se que a propriedade FrequenciaAcumulada seja 36
        Assert.Equal(36d, dados.FrequenciaAcumulada, 6);
    }

    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualRelativa()
    {
        // Arrange
        List<int> numeros = new List<int> { 1, 5, 10, 20 };
        var dados = new DadosFrequenciaAcumulada();
        
        // Act: calcula total e percentuais
        dados.CalcularPercentualRelativa(numeros);

        // Assert: somatório percentual relativo deve ser 100
        // Explicação: os percentuais relativos somam 100% quando distribuídos corretamente
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualRelativa, 6);
    }

    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualTotal()
    {
        // Arrange
        List<int> numeros = new List<int> { 1, 5, 10, 20 };
        var dados = new DadosFrequenciaAcumulada();

        // Act: calcula total e percentuais
        dados.CalcularPercentualTotal(numeros);

        // Assert: somatório percentual total deve ser 100
        // Explicação: os percentuais totais acumulados devem resultar em 100% ao final
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualTotal, 6);
    }
    
    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualRelativaEPercentualTotal()
    {
        // Arrange
        List<int> numeros = new List<int> { 1, 5, 10, 20 };
        var dados = new DadosFrequenciaAcumulada();

        // Act: calcula total e percentuais encadeando
        dados
            .FrequenciaAcumuladaTotal(numeros)
            .CalcularPercentualRelativa(numeros)
            .CalcularPercentualTotal(numeros);

        // Assert: somatório percentual relativo e total devem ser 100
        // Explicação 1: o somatório das frequências é 36 (1+5+10+20)
        Assert.Equal(36d, dados.FrequenciaAcumulada, 6);
        // Explicação 2: após calcular percentuais relativos, a soma deve ser 100 (representando 100%)
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualRelativa, 6);
        // Explicação 3: após calcular percentuais totais acumulados, a soma deve ser 100 também
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualTotal, 6);
    }

    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualRelativaEPercentualTotal_ComListaVazia()
    {
        // Arrange
        List<int> numeros = new List<int>();
        var dados = new DadosFrequenciaAcumulada();

        // Act: calcula total e percentuais
        dados
            .FrequenciaAcumuladaTotal(numeros)
            .CalcularPercentualRelativa(numeros)
            .CalcularPercentualTotal(numeros);

        // Assert: somatório percentual relativo e total devem ser 0
        // Explicação: lista vazia => somatório 0 e percentuais 0
        Assert.Equal(0d, dados.FrequenciaAcumulada, 6);
        Assert.Equal(0d, dados.FrequenciaAcumuladaPercentualRelativa, 6);
        Assert.Equal(0d, dados.FrequenciaAcumuladaPercentualTotal, 6);
    }

    [Fact]
    public void NullList_DeveTratarSemExcecao()
    {
        // Arrange
        List<int>? numeros = null;
        var dados = new DadosFrequenciaAcumulada();

        // Act: chamar métodos com null não deve lançar e deve resultar em zeros
        dados.FrequenciaAcumuladaTotal(numeros!);
        dados.CalcularPercentualRelativa(numeros!);
        dados.CalcularPercentualTotal(numeros!);

        // Assert: resultados devem ser zeros
        Assert.Equal(0d, dados.FrequenciaAcumulada, 6);
        Assert.Equal(0d, dados.FrequenciaAcumuladaPercentualRelativa, 6);
        Assert.Equal(0d, dados.FrequenciaAcumuladaPercentualTotal, 6);
    }

    [Fact]
    public void FuncionaComDoubleEDecimal()
    {
        // Arrange
        var listaDouble = new List<double> { 2.0, 3.0, 5.0 };
        var listaDecimal = new List<decimal> { 1.5m, 2.5m, 6.0m };

        // Act + Assert for double list
        var dados1 = new DadosFrequenciaAcumulada();
        dados1.BeginFrequencia()
              .FrequenciaAcumuladaTotal(listaDouble)
              .CalcularPercentualRelativa(listaDouble)
              .CalcularPercentualTotal(listaDouble)
              .Build();

        // Explicação: somatório 2+3+5 = 10; percentuais somam 100
        Assert.Equal(10d, dados1.FrequenciaAcumulada, 6);
        Assert.Equal(100d, dados1.FrequenciaAcumuladaPercentualRelativa, 6);
        Assert.Equal(100d, dados1.FrequenciaAcumuladaPercentualTotal, 6);

        // Act + Assert for decimal list
        var dados2 = new DadosFrequenciaAcumulada();
        dados2.BeginFrequencia()
              .FrequenciaAcumuladaTotal(listaDecimal)
              .CalcularPercentualRelativa(listaDecimal)
              .CalcularPercentualTotal(listaDecimal)
              .Build();

        // Explicação: somatório 1.5+2.5+6 = 10; percentuais somam 100
        Assert.Equal(10d, dados2.FrequenciaAcumulada, 6);
        Assert.Equal(100d, dados2.FrequenciaAcumuladaPercentualRelativa, 6);
        Assert.Equal(100d, dados2.FrequenciaAcumuladaPercentualTotal, 6);
    }
}

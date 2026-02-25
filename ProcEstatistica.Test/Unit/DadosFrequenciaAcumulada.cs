using Xunit;
using ProcEstatistica;
using System.Collections.Generic;

namespace ProcEstatistica.Test;

public class DadosFrequenciaAcumuladaTests
{
    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumulada()
    {
        //Arrange
        List<int> numeros = new List<int> {1, 5, 10, 20};
        var dados =  new ProcEstatistica.DadosFrequenciaAcumulada();
        
        //Act: Somatório dos números
        dados.FrequenciaAcumuladaTotal(numeros);

        //Assert: o somatório deve ser 36
        Assert.Equal(36, dados.FrequenciaAcumulada);
    }

    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualRelativa()
    {
        //Arrange
        List<int> numeros = new List<int> { 1, 5, 10, 20 };
        var dados = new DadosFrequenciaAcumulada();
        
        //Act: calcula total e percentuais
        dados
            .CalcularPercentualRelativa(numeros);
            

        //Assert: somatório percentual relativo e total devem ser 100
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualRelativa, 3);
    }

    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualTotal()
    {
        //Arrange
        List<int> numeros = new List<int> { 1, 5, 10, 20 };
        var dados = new DadosFrequenciaAcumulada();

        //Act: calcula total e percentuais
        dados
            .CalcularPercentualTotal(numeros);


        //Assert: somatório percentual relativo e total devem ser 100
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualTotal, 3);
    }
    
    [Fact]
    public void FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualRelativaEPercentualTotal()
    {
        //Arrange
        List<int> numeros = new List<int> { 1, 5, 10, 20 };
        var dados = new DadosFrequenciaAcumulada();

        //Act: calcula total e percentuais
        dados
            .FrequenciaAcumuladaTotal(numeros)
            .CalcularPercentualRelativa(numeros)
            .CalcularPercentualTotal(numeros);

        //Assert: somatório percentual relativo e total devem ser 100
        Assert.Equal(36, dados.FrequenciaAcumulada);
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualRelativa, 3);
        Assert.Equal(100d, dados.FrequenciaAcumuladaPercentualTotal, 3);
    }

    [Fact]
    public void
        FrequenciaAcumuladaTotal_DeveRetornarFrequenciaAcumuladaPercentualRelativaEPercentualTotal_ComListaVazia()
    {
        //Arrange
        List<int> numeros = new List<int>();
        var dados = new DadosFrequenciaAcumulada();

        //Act: calcula total e percentuais
        dados
            .FrequenciaAcumuladaTotal(numeros)
            .CalcularPercentualRelativa(numeros)
            .CalcularPercentualTotal(numeros);

        //Assert: somatório percentual relativo e total devem ser 0
        Assert.Equal(0, dados.FrequenciaAcumulada);
        Assert.Equal(0d, dados.FrequenciaAcumuladaPercentualRelativa, 3);
        Assert.Equal(0d, dados.FrequenciaAcumuladaPercentualTotal, 3);
    }
}
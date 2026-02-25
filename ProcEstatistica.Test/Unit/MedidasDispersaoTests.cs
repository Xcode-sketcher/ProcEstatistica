using Xunit;
using ProcEstatistica;
using System.Collections.Generic;

namespace ProcEstatistica.Test;

public class MedidasDispersaoTests
{
    // ========== Testes de Amplitude ==========

    [Fact]
    public void Amplitude_DeveCalcularCorretamente_ComInt()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 1, 5, 10, 20 };

        dados.Amplitude(lista);

        Assert.Equal(19, dados.AmplitudeTotal); // 20 - 1 = 19
    }

    [Fact]
    public void Amplitude_DeveCalcularCorretamente_ComFloat()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<float> { 1.5f, 5.25f, 10.75f, 20.0f };

        dados.Amplitude(lista);

        // 20.0 - 1.5 = 18.5 -> Convert.ToInt32(18.5) => 18
        Assert.Equal(18, dados.AmplitudeTotal);
    }

    [Fact]
    public void Amplitude_DeveCalcularCorretamente_ComDouble()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<double> { -2.5, 0.0, 7.3 };

        dados.Amplitude(lista);

        // 7.3 - (-2.5) = 9.8 -> Convert.ToInt32(9.8) => 10
        Assert.Equal(10, dados.AmplitudeTotal);
    }

    [Fact]
    public void Amplitude_DeveCalcularCorretamente_ComDecimal()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<decimal> { 2.1m, 3.9m };

        dados.Amplitude(lista);

        // 3.9 - 2.1 = 1.8 -> Convert.ToInt32(1.8) => 2
        Assert.Equal(2, dados.AmplitudeTotal);
    }

    [Fact]
    public void Amplitude_ComListaVazia_DeveRetornarZero()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int>();

        dados.Amplitude(lista);

        Assert.Equal(0, dados.AmplitudeTotal);
    }

    [Fact]
    public void Amplitude_ComUmElemento_DeveRetornarZero()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 42 };

        dados.Amplitude(lista);

        Assert.Equal(0, dados.AmplitudeTotal); // Max - Min = 42 - 42 = 0
    }

    [Fact]
    public void Amplitude_ComValoresNegativos_DeveCalcularCorretamente()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { -10, -5, 0, 5, 10 };

        dados.Amplitude(lista);

        Assert.Equal(20, dados.AmplitudeTotal); // 10 - (-10) = 20
    }

    // ========== Testes de Media e Desvio via Builder ==========

    [Fact]
    public void MediaDesvio_ViaBuilder_TransfereMediaECalculaCorretamente()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 1, 3, 5 };

        
        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        // Média = (1+3+5)/3 = 3.0
        Assert.Equal(3.0, dados.Media, 6);
        
        // Desvio Médio = (|1-3| + |3-3| + |5-3|)/3 = (2 + 0 + 2)/3 = 1.333333...
        Assert.Equal(1.33333333333333, dados.DesvioMedio, 6);
        
        // Variância Populacional = ((1-3)² + (3-3)² + (5-3)²)/3 = (4 + 0 + 4)/3 = 2.6666667
        Assert.Equal(2.66666666666667, dados.Variancia, 6);
        
        // Desvio Padrão = sqrt(2.6666667) ≈ 1.6329931...
        Assert.Equal(System.Math.Sqrt(2.66666666666667), dados.DesvioPadrao, 6);
    }

    [Fact]
    public void MediaDesvio_ViaBuilder_ComValoresNegativos()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<double> { -5.0, -2.0, 3.0 };

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        // Média = (-5 + (-2) + 3)/3 = -4/3 ≈ -1.333333...
        Assert.Equal(-1.33333333333333, dados.Media, 6);
        
        // Desvio Médio = (|-5 - (-1.333)| + |-2 - (-1.333)| + |3 - (-1.333)|)/3
        //             = (3.6667 + 0.6667 + 4.3333)/3 ≈ 2.8888888...
        Assert.Equal(2.88888888888889, dados.DesvioMedio, 6);
    }

    [Fact]
    public void MediaDesvio_Explicitos_FuncionamIgualmente()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<double> { 1.0, 3.0, 5.0 };

        // Método explícito (não via builder)
        dados.Media(lista);
        dados.Desvio(lista, dados.Media);

        Assert.Equal(3.0, dados.Media, 6);
        Assert.Equal(1.33333333333333, dados.DesvioMedio, 6);
        Assert.Equal(2.66666666666667, dados.Variancia, 6);
    }

    [Fact]
    public void MediaDesvio_ComListaVazia_DeveRetornarZeros()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int>();

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        Assert.Equal(0, dados.Media, 6);
        Assert.Equal(0, dados.DesvioMedio, 6);
        Assert.Equal(0, dados.Variancia, 6);
        Assert.Equal(0, dados.DesvioPadrao, 6);
    }

    [Fact]
    public void MediaDesvio_ComUmElemento()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 42 };

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        Assert.Equal(42.0, dados.Media, 6);
        // Desvio médio = |42 - 42|/1 = 0
        Assert.Equal(0, dados.DesvioMedio, 6);
        // Variância = (42 - 42)²/1 = 0
        Assert.Equal(0, dados.Variancia, 6);
        // Desvio padrão = sqrt(0) = 0
        Assert.Equal(0, dados.DesvioPadrao, 6);
    }

    // ========== Testes de Amplitude com Media/Desvio (Builder) ==========

    [Fact]
    public void Builder_PermiteAmplitudeAposMidiaEDesvio()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 2, 4, 6 };

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista)
             .Amplitude(lista);

        Assert.Equal(4.0, dados.Media, 6);  // (2+4+6)/3 = 4
        Assert.Equal(4, dados.AmplitudeTotal);  // 6 - 2 = 4
    }

    [Fact]
    public void Builder_PermiteAmplitudeSemDesvio()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 10, 20, 30 };

        dados.BeginMedidas()
             .Media(lista)
             .Amplitude(lista);

        Assert.Equal(20.0, dados.Media, 6);  // (10+20+30)/3 = 20
        Assert.Equal(20, dados.AmplitudeTotal);  // 30 - 10 = 20
    }

    [Fact]
    public void Builder_Build_RetornaObjeto()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 5, 10, 15 };

        var resultado = dados.BeginMedidas()
                             .Media(lista)
                             .Build();

        Assert.Equal(10.0, resultado.Media, 6);  // (5+10+15)/3 = 10
        Assert.Same(dados, resultado);  // Retorna o mesmo objeto
    }

    // ========== Testes de Precisão com Valores Decimais ==========

    [Fact]
    public void MediaDesvio_ComDecimal_MantemPrecisao()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<decimal> { 1.5m, 2.5m, 3.5m };

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        // Média = (1.5 + 2.5 + 3.5)/3 = 7.5/3 = 2.5
        Assert.Equal(2.5, dados.Media, 6);
        
        // Desvio Médio = (|1.5-2.5| + |2.5-2.5| + |3.5-2.5|)/3 = (1 + 0 + 1)/3 ≈ 0.6666667
        Assert.Equal(0.66666666666667, dados.DesvioMedio, 6);
    }

    [Fact]
    public void MediaDesvio_ComGrandesValores()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<int> { 1000, 2000, 3000 };

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        Assert.Equal(2000.0, dados.Media, 6);  // (1000+2000+3000)/3 = 2000
        // Desvio Médio = (|1000-2000| + |2000-2000| + |3000-2000|)/3 = (1000 + 0 + 1000)/3 ≈ 666.6667
        Assert.Equal(666.66666666666667, dados.DesvioMedio, 6);
    }

    // ========== Testes com Tipos Diferentes ==========

    [Fact]
    public void MediaDesvio_ComFloat()
    {
        var dados = new MedidasDispersaoDados();
        var lista = new List<float> { 1.1f, 2.2f, 3.3f };

        dados.BeginMedidas()
             .Media(lista)
             .Desvio(lista);

        // Média ≈ 2.2 (com margem de erro de float)
        Assert.InRange(dados.Media, 2.1, 2.3);
        Assert.True(dados.DesvioMedio > 0, "Desvio médio deve ser positivo");
    }

    [Fact]
    public void Amplitude_ComMisturaraTiposNumericos_ComInt()
    {
        var dados1 = new MedidasDispersaoDados();
        var lista1 = new List<int> { 5, 15 };
        dados1.Amplitude(lista1);
        Assert.Equal(10, dados1.AmplitudeTotal);

        var dados2 = new MedidasDispersaoDados();
        var lista2 = new List<double> { 5.0, 15.0 };
        dados2.Amplitude(lista2);
        Assert.Equal(10, dados2.AmplitudeTotal);
    }
}

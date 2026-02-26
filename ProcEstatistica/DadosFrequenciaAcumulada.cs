using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcEstatistica;

public class DadosFrequenciaAcumulada
{
    public double FrequenciaAcumulada { get; set; }
    public double FrequenciaAcumuladaPercentualRelativa { get; set; }
    public double FrequenciaAcumuladaPercentualTotal { get; set; }
}

public static class FrequenciaAcumuladaExtensions
{
    public static DadosFrequenciaAcumulada FrequenciaAcumuladaTotal<T>(this DadosFrequenciaAcumulada dados, List<T> numeros)
        where T : struct, IComparable<T>, IConvertible
    {
        if (dados is null) throw new ArgumentNullException(nameof(dados));

        if (numeros is null || numeros.Count == 0)
        {
            dados.FrequenciaAcumulada = 0;
            return dados;
        }
        
        
        dados.FrequenciaAcumulada = numeros.Select(x => Convert.ToDouble(x)).Sum();
        return dados;
    }

    public static DadosFrequenciaAcumulada CalcularPercentualRelativa<T>(this DadosFrequenciaAcumulada dados, List<T> numeros)
        where T : struct, IComparable<T>, IConvertible
    {
        if (dados is null) throw new ArgumentNullException(nameof(dados));

        
        if (dados.FrequenciaAcumulada == 0)
            dados.FrequenciaAcumuladaTotal(numeros);

        if (dados.FrequenciaAcumulada == 0 || numeros is null || numeros.Count == 0)
            return dados;

        
        dados.FrequenciaAcumuladaPercentualRelativa = 0.0;

        foreach (var num in numeros)
        {
            var valor = Convert.ToDouble(num);
            dados.FrequenciaAcumuladaPercentualRelativa += (valor / dados.FrequenciaAcumulada) * 100.0;
        }

        return dados;
    }

    public static DadosFrequenciaAcumulada CalcularPercentualTotal<T>(this DadosFrequenciaAcumulada dados, List<T> numeros)
        where T : struct, IComparable<T>, IConvertible
    {
        if (dados is null) throw new ArgumentNullException(nameof(dados));

        if (dados.FrequenciaAcumulada == 0)
            dados.FrequenciaAcumuladaTotal(numeros);

        if (dados.FrequenciaAcumulada == 0 || numeros is null || numeros.Count == 0)
            return dados;
        
        
        dados.FrequenciaAcumuladaPercentualTotal = 0.0;

        foreach (var num in numeros)
        {
            var valor = Convert.ToDouble(num);
            dados.FrequenciaAcumuladaPercentualTotal += (valor / dados.FrequenciaAcumulada) * 100.0;
        }

        return dados;
    }

    
    public static IFrequenciaStart BeginFrequencia(this DadosFrequenciaAcumulada dados) => new FrequenciaBuilder(dados);
}

public interface IFrequenciaStart
{
    IFrequenciaAfterTotal FrequenciaAcumuladaTotal<T>(List<T> numeros) where T : struct, IComparable<T>, IConvertible;
}

public interface IFrequenciaAfterTotal
{
    IFrequenciaAfterTotal CalcularPercentualRelativa<T>(List<T> numeros) where T : struct, IComparable<T>, IConvertible;
    IFrequenciaAfterTotal CalcularPercentualTotal<T>(List<T> numeros) where T : struct, IComparable<T>, IConvertible;
    DadosFrequenciaAcumulada Build();
}


internal class FrequenciaBuilder : IFrequenciaStart, IFrequenciaAfterTotal
{
    private readonly DadosFrequenciaAcumulada _dados;

    public FrequenciaBuilder(DadosFrequenciaAcumulada dados)
    {
        _dados = dados ?? throw new ArgumentNullException(nameof(dados));
    }

    public IFrequenciaAfterTotal FrequenciaAcumuladaTotal<T>(List<T> numeros) where T : struct, IComparable<T>, IConvertible
    {
        _dados.FrequenciaAcumuladaTotal(numeros);
        return this;
    }

    public IFrequenciaAfterTotal CalcularPercentualRelativa<T>(List<T> numeros) where T : struct, IComparable<T>, IConvertible
    {
        _dados.CalcularPercentualRelativa(numeros);
        return this;
    }

    public IFrequenciaAfterTotal CalcularPercentualTotal<T>(List<T> numeros) where T : struct, IComparable<T>, IConvertible
    {
        _dados.CalcularPercentualTotal(numeros);
        return this;
    }

    public DadosFrequenciaAcumulada Build() => _dados;
}
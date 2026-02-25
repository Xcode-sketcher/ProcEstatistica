using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcEstatistica;

public class MedidasDispersaoDados
{
    public double Media { get; set; }
    public int AmplitudeTotal { get; set; }
    public double DesvioMedio { get; set; }
    public double Variancia { get; set; }
    public double DesvioPadrao { get; set; }
}

public static class MedidasDispersao
{
    public static MedidasDispersaoDados Media<T>(this MedidasDispersaoDados dados, List<T> lista)
        where T : struct, IComparable<T>, IConvertible
    {
        if (lista == null || lista.Count == 0)
        {
            dados.Media = 0;
            return dados;
        }
        
        var listaDouble = lista.Select(x => Convert.ToDouble(x)).ToList();
        var media = listaDouble.Average();
        dados.Media = media;
        return dados;
    }
    
    public static MedidasDispersaoDados Desvio<T>(this MedidasDispersaoDados dados, List<T> lista, double media)
        where T : struct, IComparable<T>, IConvertible
    {
        if (lista == null || lista.Count == 0)
        {
            dados.DesvioMedio = 0;
            dados.Variancia = 0;
            dados.DesvioPadrao = 0;
            return dados;
        }

        int n = lista.Count;
        double somaAbs = 0;
        double somaQuad = 0;

        foreach (var item in lista)
        {
            var val = Convert.ToDouble(item);
            var dif = val - media;
            somaAbs += Math.Abs(dif);
            somaQuad += dif * dif;
        }

        dados.DesvioMedio = somaAbs / n;
        dados.Variancia = somaQuad / n;  
        dados.DesvioPadrao = Math.Sqrt(dados.Variancia);
        return dados;
    }

    public static MedidasDispersaoDados Amplitude<T>(this MedidasDispersaoDados dados, List<T> lista)
        where T : struct, IComparable<T>, IConvertible
    {
        if (lista is null || lista.Count == 0)
        {
            dados.AmplitudeTotal = 0;
            return dados;
        }

        var max = lista.Max();
        var min = lista.Min();

        var diff = Convert.ToDouble(max) - Convert.ToDouble(min);
        dados.AmplitudeTotal = Convert.ToInt32(diff);
        return dados;
    }

    

    
    public static IMediasStart BeginMedidas(this MedidasDispersaoDados dados) => new MedidasBuilder(dados);

}


public interface IMediasStart
{
    IMediasAfterMedia Media<T>(List<T> lista) where T : struct, IComparable<T>, IConvertible;
}

public interface IMediasAfterMedia
{
    
    MedidasDispersaoDados Desvio<T>(List<T> lista) where T : struct, IComparable<T>, IConvertible;
    
    MedidasDispersaoDados Amplitude<T>(List<T> lista) where T : struct, IComparable<T>, IConvertible;
    
    MedidasDispersaoDados Build();
}


internal class MedidasBuilder : IMediasStart, IMediasAfterMedia
{
    private readonly MedidasDispersaoDados _dados;
    private double _media;

    public MedidasBuilder(MedidasDispersaoDados dados)
    {
        _dados = dados ?? throw new ArgumentNullException(nameof(dados));
    }

    public IMediasAfterMedia Media<T>(List<T> lista) where T : struct, IComparable<T>, IConvertible
    {
        if (lista == null || lista.Count == 0)
        {
            _media = 0;
            _dados.Media = 0;
            return this;
        }

        var listaDouble = lista.Select(x => Convert.ToDouble(x)).ToList();
        _media = listaDouble.Average();
        _dados.Media = _media;
        return this;
    }

    public MedidasDispersaoDados Desvio<T>(List<T> lista) where T : struct, IComparable<T>, IConvertible
    {
        _dados.Desvio(lista, _media);
        return _dados;
    }

    public MedidasDispersaoDados Amplitude<T>(List<T> lista) where T : struct, IComparable<T>, IConvertible
    {
        return _dados.Amplitude(lista);
    }

    public MedidasDispersaoDados Build() => _dados;
}
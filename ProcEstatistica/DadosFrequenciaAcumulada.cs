namespace ProcEstatistica;

public class DadosFrequenciaAcumulada
{
    public int FrequenciaAcumulada { get; set; }
    public double FrequenciaAcumuladaPercentualRelativa { get; set; }
    public double FrequenciaAcumuladaPercentualTotal { get; set; }
}

public static class FrequenciaAcumuladaExtensions
{
    public static DadosFrequenciaAcumulada FrequenciaAcumuladaTotal(this DadosFrequenciaAcumulada dados, List<int> numeros)
    {
        foreach (var num in numeros)
        {
            dados.FrequenciaAcumulada += num;
        }
        return dados;
    }

    public static DadosFrequenciaAcumulada CalcularPercentualRelativa(this DadosFrequenciaAcumulada dados, List<int> numeros)
    {
        // Se a frequência acumulada ainda for zero, calcula o total primeiro para evitar divisão por zero
        if (dados.FrequenciaAcumulada == 0)
        {
            dados.FrequenciaAcumuladaTotal(numeros);
        }

        // Se ainda for zero, retorna os dados sem calcular os percentuais para evitar divisão por zero
        if (dados.FrequenciaAcumulada == 0)
            return dados;

        foreach (var num in numeros)
        {
            dados.FrequenciaAcumuladaPercentualRelativa += ((double)num / dados.FrequenciaAcumulada) * 100;
        }
        return dados; 
    }

    public static DadosFrequenciaAcumulada CalcularPercentualTotal(this DadosFrequenciaAcumulada dados, List<int> numeros)
    {
        // Garantir que a frequência acumulada seja calculada antes de calcular os percentuais totais para evitar divisão por zero
        if (dados.FrequenciaAcumulada == 0)
        {
            dados.FrequenciaAcumuladaTotal(numeros);
        }
        
        // Se ainda for zero, retorna os dados sem calcular os percentuais para evitar divisão por zero
        if (dados.FrequenciaAcumulada == 0)
            return dados;

        foreach (var num in numeros)
        {
            dados.FrequenciaAcumuladaPercentualTotal += ((double)num / dados.FrequenciaAcumulada) * 100;
        }
        return dados;
    }
}
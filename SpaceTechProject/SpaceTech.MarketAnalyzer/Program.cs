// (c) 2026 Guillermo Roger Hernandez Chandia - ADS

static void ExportarParaExcel()
{
    Console.WriteLine("\n[SISTEMA]: Gerando relatório .CSV para análise externa...");
    
    if (File.Exists(ArquivoLog))
    {
        // Criamos o cabeçalho do arquivo CSV
        string csvHeader = "Data;Modelo;Preco;ComissaoEstimada\n";
        string csvContent = csvHeader;

        var linhas = File.ReadAllLines(ArquivoLog);
        foreach (var linha in linhas)
        {
            // Lógica para limpar os dados e formatar para o Excel
            // Substituímos os separadores do TXT por ponto e vírgula (;)
            string formatada = linha.Replace(" | ", ";").Replace("R$", "");
            csvContent += formatada + "\n";
        }

        File.WriteAllText("RelatorioVendas.csv", csvContent);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">>> SUCESSO! Arquivo 'RelatorioVendas.csv' pronto para o Excel.");
        Console.ResetColor();
    }
    else { Console.WriteLine("Sem dados para exportar."); }
    Console.ReadKey();
}

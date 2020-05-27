using System;
using System.Collections.Generic;
using System.Linq;

namespace DistruibuiContratoCarteiraAtendente
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteWelcome();

            var contratos = new List<Contrato>();
            var atendentes = new List<Atendente>();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Digite aqui: ");
                Console.ResetColor();

                var comandoDigitado = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(comandoDigitado))
                    Console.WriteLine("Digite um comandos existentes");
                else
                {
                    comandoDigitado = comandoDigitado.ToLower();

                    switch (comandoDigitado)
                    {
                        case "d": ExecutaCenarioDefault(); break;
                        case "c": WriteWelcome(clear: true); break;
                        case "q": Console.WriteLine("Saindo do programa"); return;
                        case "exec": Exec(contratos, atendentes); break;
                        default: AddComando(comandoDigitado, contratos, atendentes); break;
                    }
                }
                
            }
        }

        static void AddComando(string comandoDigitado, List<Contrato> contratos, List<Atendente> atendentes)
        {
            try
            {
                var tipoAdd = comandoDigitado.Split("-")[0];
                var empreendimento = Convert.ToInt32(comandoDigitado.Split("-")[1]);

                if (tipoAdd == "c") // Adicionando contratos
                {
                    var qtdComEscritura = Convert.ToInt32(comandoDigitado.Split("-")[2]);
                    var qtdSemEscritura = Convert.ToInt32(comandoDigitado.Split("-")[3]);

                    contratos.AddRange(GeraContratos(empreendimento, qtdComEscritura, true));
                    contratos.AddRange(GeraContratos(empreendimento, qtdSemEscritura, false));

                }
                else if (tipoAdd == "a") // Adicionando atendentes
                {
                    var qtdAtendentes = Convert.ToInt32(comandoDigitado.Split("-")[2]);
                    atendentes.AddRange(GeraAtendentes(empreendimento, qtdAtendentes));
                }
                else
                    throw new Exception("Entrada inválida, favor digitar no padrão do exemplo sugerido");

            }
            catch (Exception)
            {
                Console.WriteLine("Entrada inválida, favor digitar no padrão do exemplo sugerido");
            }
        }

        static void WriteWelcome(bool clear = false)
        {
            if (clear)
                Console.Clear();
           
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*** Bem vindo a simulação de distribuição de contratos para os atendentes! ***\n\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" * COMANDOS EXISTENTES *");

            Console.ResetColor();
            Console.WriteLine(" * Para add contrato digite C-Empreendimento-QtdComEscritura-QtdSemEscritura (separando-os por \"-\"). Exemplo: C-4-23-17");
            Console.WriteLine(" * Para add atendente digite A-Empreendimento-Quantidade (separando-os por \"-\"). Exemplo: A-4-6");
            Console.WriteLine(" * [exec] - para executar a simulação com os dados preenchidos]");
            Console.WriteLine(" * [d] - para executar a simulação default]");
            Console.WriteLine(" * [c] - limpar simulador");
            Console.WriteLine(" * [q] - quit]\n");
        }

        static void Exec(List<Contrato> contratos, List<Atendente> atendentes)
        {
            Console.WriteLine("Executando Simulação ...\n");

            // TOTAIS
            var totalContratosComEscritura = contratos.Count(x => x.Escritura);
            var totalContratosSemEscritura = contratos.Count(x => !x.Escritura);
            var totalContratos = totalContratosComEscritura + totalContratosSemEscritura;
            var totalAtendentes = atendentes.Count;

            Console.WriteLine("Total [GERAL] de contratos: " + totalContratos);
            Console.WriteLine("Total [COM] escritura.....: " + totalContratosComEscritura);
            Console.WriteLine("Total [SEM] escritura.....: " + totalContratosSemEscritura + "\n");

            // MÉDIAS
            var mediaComEscritura = totalContratosComEscritura / totalAtendentes;
            var mediaSemEscritura = totalContratosSemEscritura / totalAtendentes;
            var mediaContratosPorAtendente = totalContratos / totalAtendentes;

            if (mediaComEscritura == 0 && totalContratosComEscritura > 1)
                mediaComEscritura = 1;

            if (mediaSemEscritura == 0 && totalContratosSemEscritura > 1)
                mediaSemEscritura = 1;

            if (mediaContratosPorAtendente == 0 && totalContratosComEscritura + totalContratosSemEscritura > 1)
                mediaContratosPorAtendente = 1;

            Console.WriteLine("MÉDIA [GERAL] [contrato/atendente]: " + mediaContratosPorAtendente);
            Console.WriteLine("MÉDIA [COM] escritura [contrato/atendente]: " + mediaComEscritura);
            Console.WriteLine("MÉDIA [SEM] escritura [contrato/atendente]: " + mediaSemEscritura + "\n");

            // DISTRIBUIÇÕES
            var contadorDistribuicao = 1;

            while (contratos.Any(x => !x.Distrubuido))
            {
                Console.WriteLine("Distribuição Nº " + contadorDistribuicao + "\n");

                var contratosRestanteComEscritura = RealizaDistribuicao(contratos, atendentes, true, mediaComEscritura, contadorDistribuicao, mediaContratosPorAtendente);
                var contratosRestanteSemEscritura = RealizaDistribuicao(contratos, atendentes, false, mediaSemEscritura, contadorDistribuicao, mediaContratosPorAtendente);

                contratos = contratosRestanteComEscritura.Concat(contratosRestanteSemEscritura).ToList();

                mediaComEscritura = contratos.Count(x => x.Escritura) / totalAtendentes;

                if (mediaComEscritura == 0 && contratosRestanteComEscritura.Count > 1)
                    mediaComEscritura = 1;

                mediaSemEscritura = contratos.Count(x => !x.Escritura) / totalAtendentes;

                if (mediaSemEscritura == 0 && contratosRestanteSemEscritura.Count > 1)
                    mediaSemEscritura = 1;

                contadorDistribuicao++;
            }

            MontaResumo(atendentes);

            WriteWelcome();
        }

        static void ExecutaCenarioDefault()
        {
            Console.WriteLine("Executando ExecutaCenario1 ...\n");

            var atendentes = new List<Atendente> {
                new Atendente(4),
                new Atendente(5),
                new Atendente(6),
                new Atendente(7),
                new Atendente(8)
            };

            var contratos = GeraContratos(4, 15, true);
            contratos.AddRange(GeraContratos(4, 6, false));

            contratos.AddRange(GeraContratos(5, 3, true));
            contratos.AddRange(GeraContratos(5, 9, false));

            contratos.AddRange(GeraContratos(6, 0, true));
            contratos.AddRange(GeraContratos(6, 20, false));

            contratos.AddRange(GeraContratos(7, 7, true));
            contratos.AddRange(GeraContratos(7, 1, false));

            contratos.AddRange(GeraContratos(8, 2, true));
            contratos.AddRange(GeraContratos(8, 17, false));

            Exec(contratos, atendentes);
        }

        static List<Contrato> RealizaDistribuicao(List<Contrato> contratosGeral, List<Atendente> atendentes, bool flagEscritura, int media, int contadorDistribuicao, int mediaContratosPorAtendente)
        {
            Console.WriteLine($"{(flagEscritura ? "COM" : "SEM")} ESCRITURA\n");

            var contratos = contratosGeral.Where(x => x.Escritura == flagEscritura).ToList();
            var totalEmprendimentoDistintos = contratos.GroupBy(x => x.Empreendimento).Count();

            var empreendimentosParaIgnorar = new List<int>();
            var empreendimentoComMaisCasos = ProcuraEmpreendimentoComMaisCasos(contratos, flagEscritura, empreendimentosParaIgnorar);

            if (contratos.Count > 0)
                while (true)
                {
                    if (contadorDistribuicao > 1 && contratos.All(x => x.Distrubuido))
                        break;
                    else if (atendentes.All(x => x.TotalContratos(flagEscritura, contadorDistribuicao) >= media))
                        break;

                    if (empreendimentosParaIgnorar.Count == totalEmprendimentoDistintos)
                        empreendimentoComMaisCasos = atendentes.First(x => x.TotalContratos(flagEscritura, contadorDistribuicao) < media).Empreendimento;
                    else
                        empreendimentoComMaisCasos = ProcuraEmpreendimentoComMaisCasos(contratos, flagEscritura, empreendimentosParaIgnorar);

                    if (!empreendimentoComMaisCasos.HasValue && contadorDistribuicao > 1)
                    {
                        foreach (var contrato in contratos.Where(x => !x.Distrubuido))
                        {
                            var atendentesSemAMediaTotal = atendentes.Where(x => x.TotalContratos() < mediaContratosPorAtendente);

                            if (atendentesSemAMediaTotal == null || !atendentesSemAMediaTotal.Any())
                            {
                                // setando o contrato como distribuido
                                contratos.First(x => x.Id == contrato.Id).SetDistribuido(contadorDistribuicao);

                                // atribuindo o contrato para qualquer atendente do empreendimento do contrato
                                atendentes.First(x => x.Empreendimento == contrato.Empreendimento).Contratos.Add(contrato);

                                Console.WriteLine($"Atendente:{atendentes.First(x => x.Empreendimento == contrato.Empreendimento).Empreendimento} ---> contrato:{contrato.Empreendimento}");

                                continue;
                            }

                            foreach (var atendente in atendentesSemAMediaTotal.OrderBy(x => x.Empreendimento == contrato.Empreendimento))
                            {
                                // setando o contrato como distribuido
                                contratos.First(x => x.Id == contrato.Id).SetDistribuido(contadorDistribuicao);

                                // atribuindo o contrato para o atendente
                                atendentes.First(x => x.Id == atendente.Id).Contratos.Add(contrato);

                                Console.WriteLine($"Atendente:{atendente.Empreendimento} ---> contrato:{contrato.Empreendimento}");

                                break;
                            }
                        }

                        continue;
                    }

                    // percorrer os atendentes do empreendimento com mais casos atual
                    foreach (var atendente in atendentes.Where(x => x.Empreendimento == empreendimentoComMaisCasos))
                    {
                        if (atendente.Contratos.Any(x => x.Escritura == flagEscritura && x.ContadorDistribuicao == contadorDistribuicao))
                            continue;

                        if (atendente.TotalContratos() == mediaContratosPorAtendente)
                            continue;

                        for (int i = 0; i < media; i++)
                        {
                            var contrato = contratos.FirstOrDefault(x => x.Empreendimento == empreendimentoComMaisCasos && !x.Distrubuido);

                            if (contrato == null)
                            {
                                var empreendimentoComMaisCasosAux = ProcuraEmpreendimentoComMaisCasos(contratos, flagEscritura, new List<int>());

                                var contratoDeOutroEmpreendimento = contratos.FirstOrDefault(x => x.Empreendimento == empreendimentoComMaisCasosAux && !x.Distrubuido);

                                if (contratoDeOutroEmpreendimento == null)
                                {
                                    Console.WriteLine("Falha na regra: não foi encontrado contrato de outro empreendimento para distribuir ao atendente que precisa");
                                    return null;
                                }

                                contrato = contratoDeOutroEmpreendimento;
                            }

                            // setando o contrato como distribuido
                            contratos.First(x => x.Id == contrato.Id).SetDistribuido(contadorDistribuicao);

                            // atribuindo o contrato para o atendente
                            atendentes.First(x => x.Id == atendente.Id).Contratos.Add(contrato);

                            Console.WriteLine($"Atendente:{atendente.Empreendimento} ---> contrato:{contrato.Empreendimento}");
                        }
                    }

                    empreendimentosParaIgnorar.Add(empreendimentoComMaisCasos.Value);
                    Console.WriteLine("\n");
                }

            return contratos.Where(x => !x.Distrubuido && x.Escritura == flagEscritura).ToList();
        }

        static void MontaResumo(List<Atendente> atendentes)
        {
            Console.WriteLine("\n ***** INICIO RESUMO ******\n");

            foreach (var atendente in atendentes)
            {
                Console.WriteLine($"Atendente:{atendente.Empreendimento} --> {atendente.Contratos.Count} contratos, " +
                    $"sendo {atendente.Contratos.Count(x => x.Escritura)} COM e {atendente.Contratos.Count(x => !x.Escritura)} SEM");
            }

            Console.WriteLine("\n ***** FIM RESUMO ******\n\n");
        }

        static int? ProcuraEmpreendimentoComMaisCasos(List<Contrato> contratos, bool escritura, List<int> empreendimentosParaIgnorar)
        {
            return contratos.Where(x => !empreendimentosParaIgnorar.Contains(x.Empreendimento) && !x.Distrubuido && x.Escritura == escritura)
                            ?.GroupBy(x => x.Empreendimento)
                            ?.ToList()
                            ?.OrderByDescending(x => x.Count())
                            ?.FirstOrDefault()
                            ?.Key;
        }

        static List<Contrato> GeraContratos(int empreendimento, int quantidade, bool escritura)
        {
            var contratos = new List<Contrato>();

            for (int i = 0; i < quantidade; i++)
                contratos.Add(new Contrato(empreendimento, escritura));

            return contratos;
        }

        static List<Atendente> GeraAtendentes(int empreendimento, int quantidade)
        {
            var atendentes = new List<Atendente>();

            for (int i = 0; i < quantidade; i++)
                atendentes.Add(new Atendente(empreendimento));

            return atendentes;
        }
    }
}

using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CargaSiscori.Models;
using CsvHelper.Configuration;
using System.Globalization;
using CargaSiscori.ViewModels;
using CargaSiscori.Context;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Transactions;
using Npgsql;

namespace CargaSiscori
{
    public class Process
    {
        protected readonly BWContext DB;
        static string PastaOrigem = Configuration.GetConfiguration("pastaOrigem");
        static string PastaDestino = Configuration.GetConfiguration("pastaDestino");

        public static bool ProcessarArquivos()
        {
            var status = true;

            if (!VerificaPastasParaImportacao())
                return false;

            var arquivosParaImportacao = ListarArquivosParaImportacao();

            if (arquivosParaImportacao == null)
                status = false;

            foreach (var arquivo in arquivosParaImportacao)
            {
                var importacoesDoCapituloMensal = LerArquivoCSV(arquivo);
                Adicionar(importacoesDoCapituloMensal);
             }

            return status;
        }

        private static IList<Siscori> LerArquivoCSV(string arquivo)
        {
            List<Siscori> importacoesDoCapitulo = new List<Siscori>();

            ZipFile.ExtractToDirectory(arquivo, PastaOrigem);

            var arquivoCsv = arquivo.Replace(".zip", ".CSV");


            if (!File.Exists(arquivoCsv))
                return null;

            try
            {
                using (var reader = new StreamReader(arquivoCsv, Encoding.GetEncoding("iso-8859-1")))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.Delimiter = "@";
                    csv.Configuration.BadDataFound = null;
                    csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.Trim().ToUpper();
                    csv.Configuration.IgnoreQuotes = true;

                    //csv.Configuration.HeaderValidated = null;
                    //csv.Configuration.MissingFieldFound = null;

                    var records = csv.GetRecords<SiscoriViewModel>();

                    foreach (var record in records)
                    {
                        importacoesDoCapitulo.Add(new Siscori
                        {
                            AnoMes = record.AnoMes.Trim(),
                            DescricaoDoProduto = record.DescricaoDoProduto.Trim(), //.Length > 8000 ? record.DescricaoDoProduto.Substring(1, 7999).ToString() : record.DescricaoDoProduto,
                            Incoterm = record.Incoterm.Trim(),
                            NatInformacao = record.NatInformacao.Trim(),
                            NcmCodigo = record.NcmCodigo.Trim(),
                            NcmDescricao = record.NcmDescricao.Trim(),
                            Ordem = record.Ordem.Trim(),
                            PaisAquisicaoCodigo = record.PaisAquisicaoCodigo.Trim(),
                            PaisDeAquisicao = record.PaisDeAquisicao.Trim(),
                            PaisDeOrigem = record.PaisDeOrigem.Trim(),
                            PaisOrigemCodigo = record.PaisOrigemCodigo.Trim(),
                            PesoLiquido = record.PesoLiquido.Trim(),
                            QuantidadeComercial = record.QuantidadeComercial.Trim(),
                            QuantidadeEstatistica = record.QuantidadeEstatistica.Trim(),
                            SituacaoDoDespacho = record.SituacaoDoDespacho.Trim(),
                            TotalunidadeProdutoDolar = record.TotalunidadeProdutoDolar.Trim(),
                            UnidadeComercial = record.UnidadeComercial.Trim(),
                            UnidadeDeMedida = record.UnidadeDeMedida.Trim(),
                            UnidadeDesembaraco = record.UnidadeDesembaraco.Trim(),
                            UnidadeDesembarque = record.UnidadeDesembarque.Trim(),
                            UnidEstat = record.UnidEstat.Trim(),
                            ValorSeguroDolar = record.ValorSeguroDolar.Trim(),
                            ValorUnidadeProdutoDolar = record.ValorUnidadeProdutoDolar.Trim(),
                            VolumeDolar = record.VolumeDolar.Trim(),
                            VoumeFrenteDolar = record.VoumeFrenteDolar.Trim()
                        });
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Ocorreu um erro: {0}", e.Message));
            }
            var arquivoAtual = arquivo.Split("\\").Last();

            File.Delete(arquivoCsv);
            File.Move(arquivo, string.Format("{0}{1}{2}", PastaDestino, "\\", arquivoAtual));

            return importacoesDoCapitulo;
        }

        private static List<string> ListarArquivosParaImportacao()
        {
            if (!VerificaPastasParaImportacao())
                return new List<string>();

            ExtrairArquivosAnuais();

            var arquivosParaImportacao = Directory.GetFiles(PastaOrigem, "*.zip").ToList();

            return arquivosParaImportacao;
        }

        private static void ExtrairArquivosAnuais()
        {
            var arquivosParaImportacaoAnual = Directory.GetFiles(PastaOrigem, "*.zip")
                            .ToList()
                            .Where(filter => filter.Split("\\")
                            .Last()
                            .Length > 14);

            foreach (var item in arquivosParaImportacaoAnual)
            {
                ZipFile.ExtractToDirectory(item, PastaOrigem);
                File.Delete(item);
            }
        }

        private static bool VerificaPastasParaImportacao()
        {
            var status = true;

            if (!Directory.Exists(PastaDestino))
                Directory.CreateDirectory(PastaDestino);

            if (!Directory.Exists(PastaOrigem))
            {
                Directory.CreateDirectory(PastaOrigem);
                Console.WriteLine(string.Format("Foi criada a pasta para importação dos arquivos .csv em: {0}.", PastaOrigem));
                status = false;
            }

            return status;
        }       

        private static void Adicionar(IList<Siscori> entity)
        {
            var status = string.Empty;

            using (var db = new BWContext())
            {
                try
                {
                    var inicio = DateTime.Now;
                    //Console.WriteLine("inicio: {0}", inicio);
                    //Console.WriteLine("Capitulo {0} data {1}", entity[0].NcmCodigo.Substring(0, 2), entity[0].AnoMes);
                    //Console.WriteLine("toral de registros: {0}", entity.Count);

                    db.BulkInsert(entity, options => options.AutoMapOutputDirection = false);

                    //var fim = DateTime.Now;
                    //Console.WriteLine("fim: {0}", fim);
                    //Console.WriteLine("Duração: {0}", fim.Subtract(inicio));

                   // Console.WriteLine("  ");


                    //Console.WriteLine("{0}:Capitulo {1} data {2} importados {3} arquivos.", DateTime.Now, entity[0].NcmCodigo, entity[0].AnoMes, entity.Count);
                    status = "Processado";
                    System.Threading.Thread.Sleep(2000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ocorreu um erro: {0} no Capitulo {1} data {2}", e.Message, entity[0].NcmCodigo.Substring(0, 2), entity[0].AnoMes);
                    Console.WriteLine();
                    //var arq = string.Format("C:\\Temp\\Siscori\\Novos\\CAPI{0}{1}.csv", entity[0].NcmCodigo.Substring(0, 2), entity[0].AnoMes.Substring(2, 4));
                    //File.Delete(arq);
                    status = e.Message;
                }
                finally
                {
                    Log.LogGravacao(new LogEntity
                    {
                        AnoMes = entity[0].AnoMes,
                        Capitulo = entity[0].NcmCodigo.Substring(0, 2),
                        Linhas = entity.Count,
                        Status = status,
                        Data = DateTime.Now
                    });

                    db.Dispose();
                }

            }
        }        
    }
}


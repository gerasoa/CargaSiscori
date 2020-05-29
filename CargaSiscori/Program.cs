using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CargaSiscori.Models;
using CsvHelper.Configuration;
using System.Globalization;
using CargaSiscori.ViewModels;

namespace CargaSiscori
{
    class Program
    {
      
        static void Main(string[] args)
        {
            ProcessarArquivos();

            Console.ReadLine();
        }

        private static bool ProcessarArquivos()
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

            using (var reader = new StreamReader(arquivo, Encoding.GetEncoding("iso-8859-1")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = "@";
                csv.Configuration.BadDataFound = null;
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.Trim().ToUpper();
                csv.Configuration.IgnoreQuotes = true;

                var records = csv.GetRecords<SiscoriViewModel>();

                if (records == null)
                    return null;

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
            }
            return importacoesDoCapitulo;
        }

        private static string[] ListarArquivosParaImportacao()
        {
            if (VerificaPastasParaImportacao())
            {
                return Directory.GetFiles(@"C:\Temp\Siscori\Novos", "*.csv");
            }

            return null;
        }

        private static bool VerificaPastasParaImportacao()
        {
            var status = true;

            var novosArquivos = @"C:\Temp\Siscori\Novos";
            var processados = @"C:\Temp\Siscori\Processados";

            if (!Directory.Exists(processados))
                Directory.CreateDirectory(processados);

            if (!Directory.Exists(novosArquivos))
            {
                Directory.CreateDirectory(novosArquivos);
                Console.WriteLine(string.Format("Foi criada a pasta para importação dos arquivos .csv em: {0}.", novosArquivos));
                status = false;
            }

            return status;
        }

        private static void Adicionar(IList<Siscori> entity)
        {
            //DB.BulkInsert(entity);
        }
    }
}

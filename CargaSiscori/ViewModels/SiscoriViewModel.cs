using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargaSiscori.ViewModels
{
    public class SiscoriViewModel
    {
        [Name("NUMERO DE ORDEM")]
        public string Ordem { get; set; }

        [Name("ANOMES")]
        public string AnoMes { get; set; }

        [Name("COD.NCM")]
        public string NcmCodigo { get; set; }

        [Name("DESCRICAO DO CODIGO NCM")]
        public string NcmDescricao { get; set; }

        [Name("PAIS")]
        public string PaisOrigemCodigo { get; set; }

        [Name("PAIS DE ORIGEM")]
        public string PaisDeOrigem { get; set; }

        [Name("PAIS")]
        public string PaisAquisicaoCodigo { get; set; }

        [Name("PAIS DE AQUISICAO")]
        public string PaisDeAquisicao { get; set; }

        [Name("UND.ESTAT.")]
        public string UnidEstat { get; set; }

        [Name("UNIDADE DE MEDIDA")]
        public string UnidadeDeMedida { get; set; }

        [Name("UNIDADE COMERC.")]
        public string UnidadeComercial { get; set; }

        [Name("DESCRICAO DO PRODUTO")]
        public string DescricaoDoProduto { get; set; }

        [Name("QTDE ESTATISTICA")]
        public string QuantidadeEstatistica { get; set; }

        [Name("PESO LIQUIDO")]
        public string PesoLiquido { get; set; }

        [Name("VMLE DOLAR")]
        public string VolumeDolar { get; set; }

        [Name("VL FRETE DOLAR")]
        public string VoumeFrenteDolar { get; set; }

        [Name("VL SEGURO DOLAR")]
        public string ValorSeguroDolar { get; set; }

        [Name("VALOR UN.PROD.DOLAR")]
        public string ValorUnidadeProdutoDolar { get; set; }

        [Name("QTD COMERCIAL.")]
        public string QuantidadeComercial { get; set; }

        [Name("TOT.UN.PROD.DOLAR")]
        public string TotalunidadeProdutoDolar { get; set; }

        [Name("UNIDADE DESEMBARQUE")]
        public string UnidadeDesembarque { get; set; }

        [Name("UNIDADE DESEMBARACO")]
        public string UnidadeDesembaraco { get; set; }

        [Name("INCOTERM")]
        public string Incoterm { get; set; }

        [Name("NAT.INFORMACAO")]
        public string NatInformacao { get; set; }

        [Name("SITUACAO DO DESPACHO")]
        public string SituacaoDoDespacho { get; set; }
    }
}

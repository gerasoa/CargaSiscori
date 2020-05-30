using System;
using System.Collections.Generic;
using System.Text;

namespace CargaSiscori.Models
{
    public class LogEntity : Entity
    {
        public int Linhas { get; set; }
        public string AnoMes { get; set; }
        public string Capitulo { get; set; }
        public string Status { get; set; }
        public DateTime Data { get; set; }
    }
}

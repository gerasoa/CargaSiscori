using CargaSiscori.Context;
using CargaSiscori.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CargaSiscori
{
    public class Log
    {
        public static void LogGravacao(LogEntity log)
        {
            using (var db = new BWContext())
            {
                try
                {
                    db.Add(log);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ocorreu um erro ao logar a gravacao Siscori: {0} - {1}", log.Capitulo, log.AnoMes);
                    Console.WriteLine("Erro: ", e.Message);
                    Console.WriteLine("");
                       }
                finally
                {
                    db.Dispose();
                }
            }
        }        
    }
}

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
                    throw new Exception(e.Message); 
                }                
            }
        }        
    }
}

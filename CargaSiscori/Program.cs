﻿using CsvHelper;
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
using Microsoft.Extensions.Configuration;

namespace CargaSiscori
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Process process = new Process();
            var res = Process.ProcessarArquivos();
            Console.WriteLine("Finalizado sem error: {0}", res);
            Console.ReadLine();
        }  
    }
}

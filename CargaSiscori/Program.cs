using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CargaSiscori
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportarCSV();

            Console.WriteLine("Hello World!");
        }


    
        private static void ImportarCSV()
        {
            var arquivos = ObterArquivos();
        }

        private static IList<string> ObterArquivos()
        {
            throw new NotImplementedException();
        }

    }
}

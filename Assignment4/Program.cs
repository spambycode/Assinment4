using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Assignment4
{
    class Program
    {
        private static StreamReader eventReader;
        private static StreamWriter logWritter;
        private static CustomerPrQ customer;

        static void Main(string[] args)
        {
            customer    = new CustomerPrQ();
            eventReader = new StreamReader("event.csv");
            logWritter  = new StreamWriter("log.txt", false);

            HandelEvents();

        }

        //---------------------------------------------------------------
        /// <summary>
        /// Handles the events of the file
        /// </summary>
        static void HandelEvents()
        {
            int status = -1;

            while(ReadLine())
            {
               
            }
        }

        private static bool ReadLine()
        {
            string lineRead;

            while(eventReader.EndOfStream != true)
            {
                lineRead = eventReader.ReadLine();
                var lineSplit = lineRead.Split(',');

                if (lineSplit.Length >= 2)
                    return true;

            }

            return false;
        }

    }
}

/*Author: George Karaszi
 * Assignment 4 Heap Tree's
 * 
 * File's Accessed: (in) Events.csv    -Store event information
 *                  (out) log.txt      -Verbose information about the program
 *                  
 * Description: Reads the events file and processes the information by passing
 * commands to the customerPqr structure. That handles the day to day task of 
 * the store and its heap tree.
 * 
 * 
 */


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
            ;
            eventReader = new StreamReader("event.csv");
            logWritter  = new StreamWriter("log.txt", false);
            customer    = new CustomerPrQ(logWritter);
            customer.SetupPrQ();

            HandelEvents();

        }

        //---------------------------------------------------------------
        /// <summary>
        /// Handles the events of the file
        /// </summary>
        static void HandelEvents()
        {
            List <string>lineEvent;

            while ((lineEvent = ReadLine()) != null)
            {
               switch(lineEvent[0].ToUpper())
               {
                   case "OPENSTORE":
                       customer.SetupPrQ();
                       break;
                   case "CLOSESTORE":
                       customer.EmptyOutPrQ();
                       break;
                   case "NEWCUSTOMER":
                       lineEvent.RemoveAt(0);
                       customer.InsertCustomerFromPrQ(lineEvent.ToArray());
                       break;
                   case "SERVECUSTOMER":
                       customer.RemoveCustomerFromPrQ();
                       break;
                   default:
                       break;
               }
            }
        }

        //------------------------------------------------------------------------
        /// <summary>
        /// Reads a line in the event's file.
        /// </summary>
        /// <returns>The command in a list format</returns>
        private static List<string> ReadLine()
        {
            string lineRead;
            
            while(eventReader.EndOfStream != true)
            {
                lineRead = eventReader.ReadLine();
                string []lineSplit = lineRead.Split(',');

                return new List<string>(lineSplit);

            }

            return null;
        }

    }
}

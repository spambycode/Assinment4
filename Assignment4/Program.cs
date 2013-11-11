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
            eventReader = new StreamReader("events.txt");
            logWritter  = new StreamWriter("log.txt", false);
            customer    = new CustomerPrQ(logWritter);

            HandelEvents();


            logWritter.Close();
        }

        //---------------------------------------------------------------
        /// <summary>
        /// Handles the events of the file
        /// </summary>
        static void HandelEvents()
        {
            List<string> lineEvent;

            logWritter.WriteLine("**Crowd Organizer App starting");

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
                   case "SERVEACUSTOMER":
                       customer.RemoveCustomerFromPrQ();
                       break;
                   default:
                       break;
               }
            }


            logWritter.WriteLine("**Crowd Organizer App stopping");
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
                var lineSplit = new List<string> (lineRead.Split(','));

                return lineSplit;
            }

            return null;
        }

    }
}

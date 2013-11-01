/*Author: George Karaszi
 * Assignment 4 Heap Tree's
 * 
 * File's Accessed: (in) LineAt6Am.csv -Customer Raw Data
 *                  (out) log.txt      -Verbose information about the program
 *                  
 * Description: A heap tree structure based on priority values. Each customer to 
 * the shop is assigned a priority value and is placed according, in ascending order.
 * The priority values are determined by a persons age, VIP status, and employee status 
 * in the company.The lower the customers priority value is, the higher the customer 
 * is placed in the stack.
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
    class CustomerPrQ
    {
        private struct Data { public string name; public int priorityValue;};
        private const int MaxN = 50;
        private int N;
        private object[] _HeapTree;
        private StreamReader customerReader;
        private StreamWriter logFile;
        private string[] customerRawData;

        /// <summary>
        /// Starting point for the heap tree
        /// </summary>
        /// <param name="logFile"></param>
        public CustomerPrQ(StreamWriter logFile)
        {
            customerReader = new StreamReader("LineAt6Am.csv");
            _HeapTree = new object[MaxN];
            this.logFile = logFile;
            N = 0;
        }

        //------------------------------------------------------------
        /// <summary>
        /// Reads and processes the customer file
        /// </summary>
        public void SetupPrQ()
        {

            string lineRead;

            while (customerReader.EndOfStream != true)
            {
                lineRead = customerReader.ReadLine();
                var lineSplit = lineRead.Split(',');

                if (lineSplit.Length >= 2)
                {
                    HeapInsert(lineSplit);
                }
            }

            Console.WriteLine("**initial heap built containing " + N + " nodes");

        }

        //-------------------------------------------------------------
        public void EmptyOutPrQ()
        {
            while (HeapIsEmpty() == false)
            {
                var customer = HeapDelete();
                Console.WriteLine(string.Format("SERVE CUSTOMER: {0} ({1})",
                                   customer.name, customer.priorityValue));
            }
        }

        //------------------------------------------------------------------------------
        /// <summary>
        /// Inserts a customer from a command
        /// </summary>
        public void InsertCustomerFromPrQ(string  []customerData)
        {
            if (HeapIsFull() == false)
            {
                var customer = HeapInsert(customerData);
                Console.WriteLine(string.Format("NEW CUSTOMER: {0} ({1})", 
                                  customer.name, customer.priorityValue));
            }

        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Removes a single customer from the top stack.
        /// </summary>
        public void RemoveCustomerFromPrQ()
        {
            if (HeapIsEmpty() == false)
            {
                var customer = HeapDelete();
                Console.WriteLine(string.Format("SERVE CUSTOMER: {0} ({1})",
                                   customer.name, customer.priorityValue));
            }
        }


        //-----------------------Private Methods--------------------------
        /// <summary>
        /// Inserts into the the heap tree in a standard walking up method
        /// </summary>
        /// <param name="CustomerData">Rawdata about the customer</param>
        /// <returns>Inserted Customer data</returns>
        private Data HeapInsert(string[] CustomerData)
        {
            int parentI = 0;
            int i       = N;
            Data customerTemp = new Data();
            customerTemp.name = CustomerData[0];
            customerTemp.priorityValue = DeterminePriorityValue(CustomerData);

            if (N == 0)
            {
                _HeapTree[N] = customerTemp;
            }
            else
            {
                //Shift heap tree, to place the smallest value in a walking up fashion.
                while (i >= 0)
                {
                    //Calculating parent
                    parentI = (i - 1) / 2;

                    //There is a special case when ever the parent is zero (root)
                    if (parentI > 0)
                    {
                        //Check if parent is higher value than child
                        if (((Data)_HeapTree[parentI]).priorityValue > customerTemp.priorityValue)
                        {
                            //Swap parent and to its child's location
                            _HeapTree[i] = _HeapTree[parentI];
                            i = parentI;
                        }
                        else //found the correct location t place the new node
                        {
                            _HeapTree[i] = customerTemp;
                            break;
                        }
                    }
                    else
                    {
                        if (((Data)_HeapTree[parentI]).priorityValue > customerTemp.priorityValue)
                        {
                            _HeapTree[i] = _HeapTree[0];
                            _HeapTree[0] = customerTemp;
                        }
                        else
                        {
                            _HeapTree[i] = customerTemp;
                        }
                        break;
                    }

                }

            }
            

            ++N;

            Console.WriteLine("New Customer: " + customerTemp.name +
                              "(" + customerTemp.priorityValue + ")");

            return customerTemp;
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Deletes the root customer from the heap tree. Then walks down, 
        /// repositioning the tree.
        /// </summary>
        /// <returns>Removed customer</returns>
        private Data HeapDelete()
        {
            int i = 0;
            int parentI = 0;
            Data customerDeleted = (Data)_HeapTree[i];

            while ((i = SubOfSmallChild(parentI)) != -1)
            {
                _HeapTree[parentI] = _HeapTree[i];
                parentI = i;

            }

            --N;
            return customerDeleted;
        }

        //-----------------------------------------------------------------------
        /// <summary>
        /// Calculates the priority value of the the incoming customer
        /// </summary>
        /// <param name="CustomerData">Raw data of the customer</param>
        /// <returns>Priority value of the customer</returns>
        private int DeterminePriorityValue(string[] CustomerData)
        {
            int initialValue = 101;
            int age;

            foreach (string s in CustomerData)
            {
                if (Int32.TryParse(s, out age) == true)
                {
                    if (age >= 65)
                    {
                        initialValue -= 15;

                        if (age >= 80)
                        {
                            initialValue -= 15;
                        }
                    }
                }
                else
                {
                    switch (s.ToUpper())
                    {
                        case "EMPLOYEE":
                            initialValue -= 25;
                            break;
                        case "OWNER":
                            initialValue -= 80;
                            break;
                        case "VIP":
                            initialValue -= 5;
                            break;
                        case "SUPERVIP":
                            initialValue -= 10;
                            break;
                        default:
                            break;
                    }
                }
            }

            return initialValue;
        }

        //--------------------------------------------------------------------
        /// <summary>
        /// Determines if heap is full
        /// </summary>
        /// <returns>Status of heap</returns>
        private bool HeapIsFull()
        {
            if (N == MaxN)
            {
                Console.WriteLine("**Heap tree is full");
                return true;
            }

            return false;
        }

        //-----------------------------------------------------------------------
        /// <summary>
        /// Status of heap
        /// </summary>
        /// <returns>empty status of heap</returns>
        private bool HeapIsEmpty()
        {
            if (N == 0)
            {
                Console.WriteLine("**heap is now empty");
                return true;
            }

            return false;
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Calculates the child sub of the lowest priority
        /// </summary>
        /// <param name="index">Starting and checking point of parent</param>
        /// <returns>index of smallest child</returns>
        private int SubOfSmallChild(int index)
        {
            int LChild = (index * 2) + 1;
            int RChild = (index * 2) + 2;

            if (_HeapTree[LChild] != null && _HeapTree[RChild] == null)
                return LChild;
            else
            {
                if (_HeapTree[LChild] == null && _HeapTree[RChild] != null)
                    return RChild;
                else
                {
                    if (((Data)_HeapTree[LChild]).priorityValue > ((Data)_HeapTree[RChild]).priorityValue)
                        return RChild;
                    else
                        return LChild;
                }
            }

        }
    }
}

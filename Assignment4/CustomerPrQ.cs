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
        private string []customerRawData;

        /// <summary>
        /// Starting point for the heap tree
        /// </summary>
        /// <param name="logFile"></param>
        public CustomerPrQ(StreamWriter logFile)
        {
            customerReader = new StreamReader("LineAt6Am.csv");
            this.logFile = logFile;
            N = 0;
        }

        //------------------------------------------------------------
        /// <summary>
        /// Reads and processes the customer file
        /// </summary>
        public void SetupPrQ()
        {
            
            while(ReadLine())
            {
                HeapInsert();
            }

            Console.WriteLine("**initial heap built containing " + N + " nodes");

        }

        //-------------------------------------------------------------
        public void EmptyOutPrQ()
        {
            while(HeapIsEmpty() == false)
            {
               var customer =  HeapDelete();
               Console.WriteLine(string.Format("SERVE CUSTOMER: {0} ({1})", 
                                  customer.name, customer.priorityValue));
            }
        }

        public void InsertCustomerFromPrQ()
        {

        }

        public void RemoveCustomerFromPrQ()
        {
            if (HeapIsEmpty() == false)
            {
                var customer = HeapDelete();
                Console.WriteLine(string.Format("SERVE CUSTOMER: {0} ({1})",
                                   customer.name, customer.priorityValue));
            }
            else
            {
                Console.WriteLine("**heap now empty");
            }
        }


        //-----------------------Private Methods--------------------------
        /// <summary>
        /// Inserts into the the heap tree in a standard walking up method
        /// </summary>
        /// <returns></returns>
        private bool HeapInsert()
        {
            int parentI                = 0;
            Data customerTemp          = new Data();
            customerTemp.name          = customerRawData[0];
            customerTemp.priorityValue = DeterminePriorityValue();

            if(N == 0)
            {
               _HeapTree[N] = customerTemp; 
            }
            else
            {
                //Shift heap tree, to place the smallest value in the a walking up fastsion.
                for(int i = N; i >= 0;)
                {
                    //Calcualting parent
                    parentI = (i-1)/2;

                    //There is a speical case when ever the parent is zero (root)
                    if(parentI > 0)
                    {
                        //Check if parent is higher value than child
                        if(((Data)_HeapTree[parentI]).priorityValue > customerTemp.priorityValue)
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

            N = N + 1;
            return true;
        }

        //----------------------------------------------------------------------
        /// <summary>
        /// Deletes the root customer from the heap tree. Then walks down, 
        /// reposisitioning the tree.
        /// </summary>
        /// <returns>Removed customer</returns>
        private Data HeapDelete()
        {
            int i = 0;
            int parentI = 0;
            Data customerDeleted = (Data)_HeapTree[i];

            while((i = SubOfSmallChild(parentI)) != -1)
            {
                _HeapTree[parentI] = _HeapTree[i];
                parentI = i;

            }

            --N;
            return customerDeleted;
        }

        //-----------------------------------------------------------------------
        /// <summary>
        /// Calculates the priority value of the the incomming customer
        /// </summary>
        /// <returns>Priority value of the customer</returns>
        private int DeterminePriorityValue()
        {
            int initialValue = 101;
            int age;

            foreach(string s in customerRawData)
            {
                if(Int32.TryParse(s, out age) == true)
                {
                    if(age >= 65)
                    {
                        initialValue -= 15;

                        if(age >= 80)
                        {
                            initialValue -= 15;
                        }
                    }
                }
                else
                {
                    switch(s.ToUpper())
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
        /// Determins if heap is full
        /// </summary>
        /// <returns>Status of heap</returns>
        private bool HeapIsFull()
        {
            if (N == MaxN)
                return true;

            return false;
        }

        //-----------------------------------------------------------------------
        /// <summary>
        /// Status of heap
        /// </summary>
        /// <returns>empty stastus of heap</returns>
        private bool HeapIsEmpty()
        {
            if (N == 0)
                return true;

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
            int LChild = (index*2)+1;
            int RChild = (index*2)+2;

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

        //-----------------------------------------------------------------------
        /// <summary>
        /// Reads each line the of customer file
        /// </summary>
        /// <returns>Status of the EOF</returns>

        private bool ReadLine()
        {
            string lineRead;

            while (customerReader.EndOfStream != true)
            {
                lineRead = customerReader.ReadLine();
                var lineSplit = lineRead.Split(',');

                if (lineSplit.Length >= 2)
                {
                    customerRawData = lineSplit;
                    return true;
                }
            }

            return false;
        }

    }
}

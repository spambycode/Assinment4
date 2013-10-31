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
        private struct Data { public string name; public int priorityValue};
        private const int MaxN = 50;
        private int N;
        private Data[] _HeapTree;
        private StreamReader customerReader;
        private StreamWriter logFile;
        private string []customerRawData;

        public CustomerPrQ(StreamWriter logFile)
        {
            customerReader = new StreamReader("LineAt6Am.csv");
            this.logFile = logFile;
            N = 0;
        }


        public void SetupPrQ()
        {
            
            while(ReadLine())
            {
                HeapInsert();
            }
        }

        public void EmptyPrQ()
        {

        }

        public void InsertCustomerFromPrQ()
        {

        }

        public void RemoveCustomerFromPrQ()
        {

        }


        //-----------------------Private Methods--------------------------

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
                        if(_HeapTree[parentI].priorityValue > customerTemp.priorityValue)
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
                        if(_HeapTree[0].priorityValue > customerTemp.priorityValue)
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

        private void HeapDelete()
        {

        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace LPP_App
{
    class BitArrayHandler
    {
        public BitArrayHandler()
        {

        }

        //reverse a bitarray
        public void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        public int GetNrOfOccurrencesOfTheElementTrue(BitArray bitArray)
        {
            int i = 0;
            foreach (bool b in bitArray)
            {
                if (b == true)
                {
                    i++;
                }
            }
            return i;
        }
    }
}

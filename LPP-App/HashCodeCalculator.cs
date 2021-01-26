using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPP_App
{
    class HashCodeCalculator
    {        
        public string GetHashCode(List<int> binaryResult)
        {
            string hashCode = string.Empty;
            string groupOfDigits = string.Empty;

            for (int i = 0; i < binaryResult.Count; i++)
            {
                groupOfDigits += binaryResult[i].ToString();

                if (groupOfDigits.Length % 4 == 0 && groupOfDigits.Length >= 4) 
                {
                    hashCode += Convert.ToString(Convert.ToInt32(groupOfDigits, 2), 16).ToUpper();
                    groupOfDigits = string.Empty;
                }

                if (binaryResult.Count < 4) 
                {
                    if (groupOfDigits.Length % binaryResult.Count == 0 && i > 0)
                    {
                        hashCode += Convert.ToString(Convert.ToInt32(groupOfDigits, 2), 16).ToUpper();
                        groupOfDigits = string.Empty;
                    }
                }
            }

            return hashCode;
        }
    }
}

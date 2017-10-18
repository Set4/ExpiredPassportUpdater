using ExpiredPassportUpdater.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater.UtilityClasses
{
    class Parser
    {
        public static ExpiredPassports[] ToExpiredPassports(string[] dataLines)
        {
            try
            {
 
                ExpiredPassports[] items = new ExpiredPassports[dataLines.Length];
                for (int i = 0; i < dataLines.Count(); i++)
                {
                    var a = dataLines[i].Split(',');
                    items[i] = new ExpiredPassports
                    {
                        Id = i,
                        Serial = a[0],
                        Number = a[1],
                        ExcludeDate = null
                    };
                }

                return items;
            }
            catch(Exception ex)
            {
                throw new Exception("Parse ToExpiredPassports error", ex);
            }
        }
    }
}

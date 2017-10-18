using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpiredPassportUpdater.Model
{
    class ExpiredPassports
    {
        public long Id { get; set; }
        public string Serial { get; set; }
        public string Number { get; set; }
        public DateTime? ExcludeDate { get; set; }
    }
}

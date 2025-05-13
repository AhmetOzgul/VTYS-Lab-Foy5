using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foy5Wpf.Models
{
    public class tBolum
    {
        [Key]
        public int bolumID { get; set; }
        public string bolumAd { get; set; }
        public int fakulteID { get; set; }
    }
}

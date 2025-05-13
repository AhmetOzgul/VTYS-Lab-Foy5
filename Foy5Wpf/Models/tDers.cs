using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foy5Wpf.Models
{
    public class tDers
    {
        [Key]
        public int dersID { get; set; }
        public string dersAd { get; set; }
    }
}

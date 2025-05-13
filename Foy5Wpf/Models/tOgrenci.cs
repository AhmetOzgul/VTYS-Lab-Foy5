using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foy5Wpf.Models
{
    public class tOgrenci
    {
        [Key]
        public int ogrenciID { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public int bolumID { get; set; }
    }
}

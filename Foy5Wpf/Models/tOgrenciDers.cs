using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foy5Wpf.Models
{
    public class tOgrenciDers
    {
        [Key]
        public int ID { get; set; }     
        public int ogrenciID { get; set; }
        public int dersID { get; set; }
        public int yil { get; set; }
        public string yariyil { get; set; }
        public int? vize { get; set; }
        public int? final { get; set; }
    }
}

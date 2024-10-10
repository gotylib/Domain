using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Base_Controll
{
    public class Domain
    {
        [Key]
        public long Id { get; set; }
        public string? Name {  get; set; }
        public string? Registrar { get; set; }
        public DateTime? DateOfRegistration { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string? Status { get; set; }
        public double? Price { get; set; }  


    }
}

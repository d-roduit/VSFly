using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreApp2021
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

    }
}

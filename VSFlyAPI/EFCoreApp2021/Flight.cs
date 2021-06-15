using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * 
 *
 **/
namespace EFCoreApp2021
{
    public class Flight
    {
        #region Constructor
        public Flight() { }
        #endregion

        #region Getter/Setter
        [Key]
        public int FlightNo { get; set; }
        [StringLength(50),MinLength(3)]
        public String Departure { get; set; }
        [StringLength(50), MinLength(3)]
        public String Destination { get; set; }
        public DateTime Date { get; set; }
        public double BasePrice { get; set; }
        [Required]
        public short? Seats { get; set; }
        //Link to the pilot which is an employee which is a person
        [ForeignKey("PilotId")]
        public virtual Pilot Pilot { get; set; }
        public int PilotId { get; set; }
        public virtual ICollection<Booking> BookingSet { get; set; }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DoctorAp.Models
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryId { get; set; }

        public int OrderNumber { get; set; }
        //-----------------------Medical aid---------PatientID-----------------------------------------------------
        /* Probably not adding this 
         public int? Id { get; set; }
         public ItemsLead ItemsLead { get; set; }

         public int? PatientID { get; set; }
         public PatientInfo PatientInfo { get; set; }
        */



        //-------------------Adress-----------------------------------------------------------------------------
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }

        //======================================================================================================

        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        public string Progress { get; set; }
        /*


        public string CurrentLocation { get; set; }


        public bool isDelivered { get; set; }



        public DateTime Date { get; set; } = DateTime.Now;
        // public virtual Sale sale { get; set; }
        */

    }
}

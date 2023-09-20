namespace DoctorAp.Models
{
    public class Driver
    {

     
            public int Id { get; set; }
            public string OrderNumber { get; set; }
            public string DriverName { get; set; }
            public bool Delivered { get; set; } // Added property to track delivery status
        }
    

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeHomeExercise4System.ViewModels
{
    public class MemberEditView
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Certification { get; set; }
        public int VehicleCount { get; set; }
        public int CarID { get; set; }
        public int TempID { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Ownership { get; set; }
        public string State { get; set; }
        public string Class { get; set; }
        public bool RemoveFlag { get; set; }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class MemberEdit
    {
        [Inject]
        private ILogger<MemberEdit> Logger { get; set; }

        [Inject]
        MemberEditServices MemberServices { get; set; }

        public List<CertificationsView> certifications { get; set; }
        public List<CarClassListView> CarClasses { get; set; }
        private Member? Member { get; set; }
        public List<string> OwnershipDescriptions { get; set; } = new List<string>();
        public List<string> CarStates { get; set; } = new List<string>();

        //public MemberListView Member { get; set; }

        [Parameter]
        public string CarClassName { get; set; }
        [Parameter]
        public int CarClassID { get; set; }

        //Member properties
        [Parameter]
        public int MemberID { get; set; } = 0;

        [Parameter]
        public string FirstName { get; set; }

        [Parameter]
        public string LastName { get; set; }

        [Parameter]
        public string Address { get; set; }

        [Parameter]
        public string City { get; set; }

        [Parameter]
        public string PostalCode { get; set; }

        [Parameter]
        public string Phone { get; set; }

        [Parameter]
        public DateTime BirthDate { get; set; }

        [Parameter]
        public string Email { get; set; }

        [Parameter]
        public int VehicleCount { get; set; } = 0;

        //CarList properties
        [Parameter]
        public int CarID { get; set; }

        [Parameter]
        public int TempID { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public string SerialNumber { get; set; }

        [Parameter]
        public string Ownership { get; set; }

        [Parameter]
        public string State { get; set; }

        [Parameter]
        public string CarClass { get; set; }

        [Parameter]
        public bool RemoveFlag { get; set; }

        public List<Car> Cars { get; set; } = new List<Car>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //get certifications
            certifications = MemberServices.GetCertificationsList();
            //get car classes
            CarClasses = MemberServices.GetCarClasses();
            //Get car states
            CarStates = new List<string>
            {
                "Wrecked",
                "In Shop",
                "Certified",
                "Unknown"
            };
            //get car ownership descriptions
            OwnershipDescriptions = new List<string>
            {
                "Rental",
                "Owner"
            };

            //Display member info
            DisplayMemberInfo(MemberID);
        }

        /// <summary>
        /// Display the fetched member info if OnEdit or display an empty form if OnNew
        /// </summary>
        /// <param name="memberId"></param>
        private void DisplayMemberInfo(int memberId)
        {
            if (memberId > 0)
            {
                Member = MemberServices.GetMemberById(memberId);

                if (Member != null)
                {
                    // Set the properties based on the fetched member
                    FirstName = Member.FirstName;
                    LastName = Member.LastName;
                    Address = Member.Address;
                    City = Member.City;
                    PostalCode = Member.PostalCode;
                    Phone = Member.Phone;
                    Email = Member.EmailAddress;
                    BirthDate = Member.BirthDate;
                    VehicleCount = Member.Cars.Count;
                    Cars = Member.Cars.ToList(); 
                    
                    
                }
            }
            else
            {
                FirstName = string.Empty;
                LastName = string.Empty;
                Address = string.Empty;
                City = string.Empty;
                PostalCode = string.Empty;
                Phone = string.Empty;
                Email = string.Empty;
                BirthDate = DateTime.Now;
                
            }
           
        }

    
    }
}

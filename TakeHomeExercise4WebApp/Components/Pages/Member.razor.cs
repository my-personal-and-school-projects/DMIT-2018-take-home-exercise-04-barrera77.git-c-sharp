using Microsoft.AspNetCore.Components;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class Member
    {

        [Inject]
        MemberServices MemberServices { get; set; }

        public List<Certification> Certifications { get; set; }
        public List<CarClassListView> CarClasses { get; set; }
        public List<string> OwnershipDescriptions { get; set; }
        public List<string> CarStates {  get; set; }

        [Parameter]
        public string CertificationLevel { get; set; }

        [Parameter]
        public string CertificationDescription { get; set; }

        [Parameter]
        public string  CarClassName { get; set; }
        [Parameter]
        public int CarClassID { get; set; }

        /// <summary>
        /// Fetch the certifications to populate the dropdown list
        /// </summary>
        /// <returns>list of Certifications</returns>
        protected override Task OnInitializedAsync()
        {
            //get certifications
            Certifications = MemberServices.Certifications_List();
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

            return base.OnInitializedAsync();
        }
    }
}

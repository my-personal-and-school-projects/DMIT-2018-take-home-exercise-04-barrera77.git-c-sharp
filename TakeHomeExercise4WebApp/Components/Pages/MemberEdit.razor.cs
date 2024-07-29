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
        private NavigationManager _navManager { get; set; }

        [Inject]
        MemberEditServices MemberServices { get; set; }

        public List<CertificationsView> certifications { get; set; }
        public List<CarClassListView> CarClasses { get; set; }
        private MemberEditView Member { get; set; }
        public List<string> OwnershipDescriptions { get; set; } = new List<string>();
        public List<string> CarStates { get; set; } = new List<string>();
                
        //Member properties
        [Parameter]
        public int MemberID { get; set; } = 0;       


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
                Logger.LogWarning($"Member Car States: {string.Join(", ", Member.CarList.Select(c => c.State))}");
                
            }
            else
            {
                Member = new MemberEditView();
                //Member.FirstName = string.Empty;
                //Member.LastName = string.Empty;
                //Member.Address = string.Empty;
                //Member.City = string.Empty;
                //Member.PostalCode = string.Empty;
                //Member.Phone = string.Empty;
                //Member.Email = string.Empty;
                //Member.BirthDate = DateTime.Now;

            }
        } 
        
        private void OnCancel()
        {
            Member = new MemberEditView();
            _navManager.NavigateTo("/MemberSearch");
        }

        private void OnSave()
        {
           
            _navManager.NavigateTo("/MemberSearch");
        }

    }
}

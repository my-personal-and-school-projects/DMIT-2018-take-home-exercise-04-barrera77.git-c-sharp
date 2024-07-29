
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class MemberSearch
    {
        [Inject]

        private ILogger<MemberSearch> Logger { get; set; }

        [Inject]
        MemberListServices? MemberListServices { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        private List<MemberListView> MemberList { get; set;}

        [Parameter]
        public string FirstName { get; set; }

        private string ErrorMessage { get; set; }

        private string SearchParam { get; set; }


        private async Task SearchMembers()
        {
            try
            {
                if (string.IsNullOrEmpty(SearchParam) || !char.IsLetter(SearchParam[0]))
                {
                    throw new ArgumentException($"{SearchParam} is not valid lastName");
                }

                MemberList = await Task.Run(() => MemberListServices.GetMembersList(SearchParam));
                ErrorMessage = null;

            }
            catch (ArgumentException ex)
            {
                ErrorMessage = $"Validation Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private void OnNew(int memberId)
        {
            _navManager.NavigateTo($"Member/{memberId}");
        }

        private void OnEdit(int memberId)
        {
            Logger.LogInformation($"MemberID: {memberId}");
            _navManager.NavigateTo($"Member/{memberId}");
        }

        //Set pagination
        protected PaginationState Pagination = new PaginationState { ItemsPerPage = 10 };
    }
   
}

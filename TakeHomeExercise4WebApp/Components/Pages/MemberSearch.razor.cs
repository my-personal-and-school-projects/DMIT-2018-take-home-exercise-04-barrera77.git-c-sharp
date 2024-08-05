
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class MemberSearch
    {
        private string feedbackMessage;
        private string errorMessage;

        [Inject]
        private ILogger<MemberSearch> Logger { get; set; }

        [Inject]
        MemberListServices? MemberListServices { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        private List<MemberListView> MemberList { get; set;}

        [Parameter]
        public string FirstName { get; set; }


        private string SearchParam { get; set; }
        private List<string> errorDetails = new List<string>();

        private async Task SearchMembers()
        {
            try
            {
                errorDetails.Clear();
                errorMessage = string.Empty;
                feedbackMessage = string.Empty;
                MemberList.Clear();

                 MemberList = await Task.Run(() => MemberListServices.GetMembersList(SearchParam));
                errorMessage = null;

            }
            catch (AggregateException ex)
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage += Environment.NewLine;
                }
                errorMessage += "Unable to search for customer.";
                foreach (Exception error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (Exception ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
        }

        private void OnNew(int memberId)
        {
            _navManager.NavigateTo($"Member/{memberId}");
        }

        private void OnEdit(int memberId)
        {
            _navManager.NavigateTo($"Member/{memberId}");
        }

        //Set pagination
        protected PaginationState Pagination = new PaginationState { ItemsPerPage = 10 };
    }
   
}

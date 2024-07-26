
using Microsoft.AspNetCore.Components;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class MemberSearch
    {
        [Inject]
        MemberListServices MemberListServices { get; set; }

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

        //Set pagination
        //protected PaginationState Pagination = new PaginationState { ItemsPerPage = 10 };
    }
}

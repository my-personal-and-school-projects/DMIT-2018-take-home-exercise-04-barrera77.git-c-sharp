using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.ViewModels;
using MudBlazor;
using static MudBlazor.Icons;
using BlazorDialog;


namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class MemberEdit
    {
        #region Properties
        [Inject]
        private ILogger<MemberEdit> Logger { get; set; }

        [Inject]
        protected IDialogService DialogService { get; set; }

        [Inject]
        protected IBlazorDialogService BlazorDialogService { get; set; }

        [Inject]
        private NavigationManager _navManager { get; set; }

        [Inject]
        MemberEditServices MemberServices { get; set; }

        //Member properties
        [Parameter]
        public int MemberID { get; set; } = 0;

        #endregion

        #region Feedback and Error Messaging

        private string feedbackMessage;
        private string errorMessage;

        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);

        private List<string> errorDetails = new List<string>();

        #endregion

        #region Fields

        public List<CertificationsView> certifications { get; set; }
        public List<CarClassListView> CarClasses { get; set; }
        public CarListView newCar { get; set; } = new CarListView();
        public MemberEditView Member { get; set; }
        public List<string> OwnershipDescriptions { get; set; } = new List<string>();
        public List<string> CarStates { get; set; } = new List<string>();

        #endregion

        #region Validation

        private EditContext editContext;
        private ValidationMessageStore messageStore;

        #endregion


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

           try
            {
                editContext = new EditContext(newCar);

                //get certifications
                certifications = MemberServices.GetCertificationsList();
                //get car classes
                CarClasses = MemberServices.GetCarClasses();
                //Get car states
                CarStates = new List<string>
                {
                    "Wrecked",
                    "InShop",
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
            catch (AggregateException ex)
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage += Environment.NewLine;
                }

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

        private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            messageStore?.Clear();

            if (String.IsNullOrWhiteSpace(newCar.Description))
            {
                messageStore.Add(() => newCar.Description, "Description is required!");
            }

            if (String.IsNullOrWhiteSpace(newCar.SerialNumber))
            {
                messageStore.Add(() => newCar.SerialNumber, "SerialNumber is required!");
            }

            if (String.IsNullOrWhiteSpace(newCar.Ownership))
            {
                messageStore.Add(() => newCar.Ownership, "Ownership is required!");
            }

            if (String.IsNullOrWhiteSpace(newCar.State))
            {
                messageStore.Add(() => newCar.State, "Car state is required!");
            }

            if (newCar.Class == 0)
            {
                messageStore.Add(() => newCar.Class, "Car class is required!");
            }
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
            }
            else
            {
                Member = new MemberEditView();
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
             
        private async Task OnRemoveVehicle(int carId)
        {
            string bodyText = $"Are you sure you want to remove the car from the list?";

            string dialogResult = await BlazorDialogService.ShowComponentAsDialog<string>(
                new ComponentAsDialogOptions(typeof(SimpleComponentDialog))
                {
                    Size = DialogSize.Normal,
                    Parameters = new()
                        {
                            { nameof(SimpleComponentDialog.Input), "Remove Car" },
                            { nameof(SimpleComponentDialog.BodyText), bodyText }
                        }
                });

            if (dialogResult == "Ok")
            {
                if (carId != 0)
                {
                    try
                    {
                        MemberServices.RemoveCarFromView(carId);
                                              
                        // refresh or update the car list
                        Member = MemberServices.GetMemberById(MemberID);
                    }
                    catch (Exception ex)
                    {
                        errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
                    }
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private void OnAddvehicle(int memberId)
        {
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;
           
            try
            {
                newCar = MemberServices.SaveCar(newCar, memberId);
               
                //Update the car list
                Member = MemberServices.GetMemberById(MemberID);

                //Reset Form
                ResetNewCarFormFields();

                feedbackMessage = "Vehicle added succesfully";               

            }
            catch (AggregateException ex)
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage += Environment.NewLine;
                }

                errorMessage += "Unable to add a vehicle!";
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

        private void ResetNewCarFormFields()
        {           
            newCar.Description = string.Empty;
            newCar.SerialNumber = string.Empty;
            newCar.Ownership = string.Empty;
            newCar.State = "0";
            newCar.Class = 0;
        }
    }
}

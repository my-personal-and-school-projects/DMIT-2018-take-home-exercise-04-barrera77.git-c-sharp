using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TakeHomeExercise4System.BLL;
using TakeHomeExercise4System.ViewModels;
using MudBlazor;
using static MudBlazor.Icons;
using BlazorDialog;
using Microsoft.AspNetCore.Components.QuickGrid;
using TakeHomeExercise4System.Entities;


namespace TakeHomeExercise4WebApp.Components.Pages
{
    public partial class MemberEdit : ComponentBase
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
        public MemberEditView Member { get; set; } = new MemberEditView();
        public List<string> OwnershipDescriptions { get; set; } = new List<string>();
        public List<string> CarStates { get; set; } = new List<string>();

        private bool isNewMember = false;

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
                //Disbale Add Car button if new member
                if(MemberID == 0)
                {
                    isNewMember = true;
                }
                
                editContext = new EditContext(Member);

                editContext.OnValidationRequested += EditContext_OnValidationRequested;

                messageStore = new ValidationMessageStore(editContext);

                editContext.OnFieldChanged += EditContext_OnFieldChanged;

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

                await InvokeAsync(StateHasChanged);


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


        private void EditContext_OnFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            editContext.Validate();
        }

        private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
        {
            messageStore?.Clear();

            if (string.IsNullOrWhiteSpace(Member.FirstName))
            {
                messageStore.Add(() => Member.FirstName, "First Name is required!");
            }
            
            if (string.IsNullOrWhiteSpace(Member.LastName))
            {
                messageStore.Add(() => Member.LastName, "Last Name is required!");
            }

            if (string.IsNullOrWhiteSpace(Member.Address))
            {
                messageStore.Add(() => Member.Address, "Address is required!");
            }

            if (string.IsNullOrWhiteSpace(Member.City))
            {
                messageStore.Add(() => Member.City, "City is required!");
            }

            if (string.IsNullOrWhiteSpace(Member.PostalCode))
            {
                messageStore.Add(() => Member.PostalCode, "Postal Codeis required!");
            }

            if (string.IsNullOrWhiteSpace(Member.Phone))
            {
                messageStore.Add(() => Member.Phone, "Phone is required!");
            }

            if (string.IsNullOrWhiteSpace(Member.Email))
            {
                messageStore.Add(() => Member.Email, "Email is required!");
            }

            if (string.IsNullOrWhiteSpace(newCar.Description))
            {
                messageStore.Add(() => newCar.Description, "Description is required!");
            }

            if (string.IsNullOrWhiteSpace(newCar.SerialNumber))
            {
                messageStore.Add(() => newCar.SerialNumber, "SerialNumber is required!");
            }

            if (string.IsNullOrWhiteSpace(newCar.Ownership))
            {
                messageStore.Add(() => newCar.Ownership, "Ownership is required!");
            }

            if (string.IsNullOrWhiteSpace(newCar.State))
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

        /// <summary>
        /// Cancel any action from the Member Edit Page and go back to the member search page
        /// </summary>
        private void OnCancel()
        {
            Member = new MemberEditView();
            _navManager.NavigateTo("/MemberSearch");
        }

        private void OnSave()
        {
            _navManager.NavigateTo("/MemberSearch");
        }

        /// <summary>
        /// Removes a vehicle from the current member's car list and updates the count
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Ads a vehicle to the list for the current member and updates the count
        /// </summary>
        /// <param name="memberId"></param>
        private void OnAddvehicle(int memberId)
        {
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;
           
            try
            {
                newCar = MemberServices.EditCar(newCar, memberId);
               
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

        /// <summary>
        /// Saves or edits the member and his car list
        /// </summary>
        /// <param name="memberView"></param>
        private void OnSaveOrEditMember(MemberEditView memberView)
        {
            errorDetails.Clear();
            errorMessage = string.Empty;
            feedbackMessage = string.Empty;

            try
            {

                // Check if new member or an existing one
                bool isNewMember = memberView.MemberID == 0;

                // Perform validation for new members
                if (isNewMember)
                {
                    // Validate car fields for new members
                    if (string.IsNullOrWhiteSpace(newCar.Description) ||
                        string.IsNullOrWhiteSpace(newCar.SerialNumber) ||
                        string.IsNullOrWhiteSpace(newCar.Ownership) ||
                        string.IsNullOrWhiteSpace(newCar.State) ||
                        newCar.Class == 0)
                    {
                        errorMessage = "Please fill out all car fields. A new member needs to have at least one car.";
                        return; // Exit the method if validation fails
                    }
                }

                if (isNewMember)
                {
                    // Create a new member
                    Member = MemberServices.EditMember(memberView);

                    // Add the new car if it’s a new member
                    MemberServices.EditCar(newCar, Member.MemberID);

                    // Update the car list
                    Member = MemberServices.GetMemberById(memberView.MemberID);

                    feedbackMessage = "Data was successfully saved!";
                    ResetNewCarFormFields();
                }
                else
                {
                    // Edit existing member
                    Member = MemberServices.EditMember(memberView);

                    // Update the car list
                    Member = MemberServices.GetMemberById(memberView.MemberID);
                    if (Member.CarList.Count == 0)
                    {
                        // Add car if there are no cars for an existing member
                        MemberServices.EditCar(newCar, memberView.MemberID);
                        Member = MemberServices.GetMemberById(memberView.MemberID);

                        ResetNewCarFormFields();
                    }
                    else
                    {
                        // Update existing cars
                        foreach (var car in Member.CarList)
                        {
                            MemberServices.EditCar(car, memberView.MemberID);
                        }
                    }

                    feedbackMessage = "Data was successfully saved!";
                
                }
            }

            catch (AggregateException ex)
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage += Environment.NewLine;
                }
                errorMessage += "Unable to add or edit member!";
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


        /// <summary>
        /// Resets the values for the new member car form
        /// </summary>
        private void ResetNewCarFormFields()
        {           
            newCar.Description = string.Empty;
            newCar.SerialNumber = string.Empty;
            newCar.Ownership = string.Empty;
            newCar.State = "0";
            newCar.Class = 0;
        }

        //Set pagination
        protected PaginationState Pagination = new PaginationState { ItemsPerPage = 3 };
    }
}

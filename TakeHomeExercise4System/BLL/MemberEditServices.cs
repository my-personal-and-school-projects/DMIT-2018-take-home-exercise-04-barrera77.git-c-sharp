using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;
using static MudBlazor.Icons;

namespace TakeHomeExercise4System.BLL
{
    public class MemberEditServices
    {

        private readonly ERace2024R1Context _context;

        internal MemberEditServices( ERace2024R1Context context )
        {
            _context = context;
        }

        #region Validation
        private string feedbackMessage;
        private string errorMessage;
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);
        #endregion

        [Inject]
        private ILogger<MemberEditServices> Logger { get; set; }

        //Create an array to store the errors
        List<Exception> errorList = new List<Exception>();

        /// <summary>
        /// Get a list of the membercertification for the dropdown list
        /// </summary>
        /// <returns></returns>
        public List<CertificationsView> GetCertificationsList()
        {
            return _context.Certifications
                .Select(c => new CertificationsView
                {
                    CertificationLevel = c.CertificationLevel,
                    Description = c.Description,
                    RemoveFromViewFlag = c.RemoveFromViewFlag,
                })
                .ToList();
        }


        /// <summary>
        /// Get a list of the car classes for the dropdown list
        /// </summary>
        /// <returns></returns>
        public List<CarClassListView> GetCarClasses() 
        {
            return _context.CarClasses
                .Select(c => new CarClassListView
                {
                    CarClassID = c.CarClassId,
                    CarClassName = c.CarClassName,
                })
                .OrderBy(c => c.CarClassName)                
                .ToList();
        }

        /// <summary>
        /// Get the information for the MemberEdit Page
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public MemberEditView? GetMemberById(int memberId) 
        {
            if (memberId == 0)
            {
                throw new ArgumentException("Please provide a valid customer ID");
            }

            return _context.Members
                .Where(m => m.MemberId == memberId)
                .Select(m => new MemberEditView
                {
                    MemberID = m.MemberId,
                    FirstName = m.FirstName, 
                    LastName = m.LastName,
                    Address = m.Address,
                    City = m.City,
                    PostalCode = m.PostalCode,
                    Phone = m.Phone,
                    Email = m.EmailAddress,
                    BirthDate = m.BirthDate,
                    Certification = m.CertificationLevel,
                    VehicleCount = m.Cars.Count(),
                    CarList = _context.Cars
                            .Where(c => c.MemberId == memberId && c.RemoveFromViewFlag == false)
                            .OrderByDescending(c => c.CarClassId)
                            .Select(c => new CarListView
                            {
                                CarID = c.CarId,
                                TempID = c.CarId,
                                SerialNumber = c.SerialNumber,
                                Ownership = c.Ownership,
                                Class = c.CarClassId,
                                State = c.State,
                                Description = c.Description,
                                MemberID = c.MemberId,
                                RemoveFromViewFlag = c.RemoveFromViewFlag,  
                            }).ToList()
                })
                .FirstOrDefault();           
        }

        /// <summary>
        /// Generic method to update any entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateEntity<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity cannot be null", new ArgumentNullException());
            }
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes an existing car from the view
        /// </summary>
        /// <param name="car"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveCarFromView(int carId)
        {
            if(carId == 0)
            {
                throw new ArgumentNullException("CarId cannot be null or 0", new ArgumentNullException());
            }

            var car = _context.Cars
                .FirstOrDefault(c => c.CarId == carId);

            if (car == null)
            {
                throw new InvalidOperationException("Car not found");
            }
            car.RemoveFromViewFlag = true;
            UpdateEntity(car);
        }

       public CarListView SaveCar(CarListView carView, int memberID)
        {
            #region Business Logic and Parameter Exceptions

            errorList.Clear();
            
            if (carView == null)
            {
                throw new ArgumentNullException("No vehicle was supplied!");
            }

            ValidateNewCarFields(carView);

            if(errorList.Count > 0)
            {
                throw new AggregateException
                    ("Please check error message(s): ", errorList);
            }

            #endregion
          
                Car car = new Car();

            car.Description = carView.Description;
            car.SerialNumber = carView.SerialNumber;
            car.Ownership = carView.Ownership;
            car.State = carView.State;
            car.CarClassId = carView.Class;
            car.MemberId = memberID;

            if (errorList.Count > 0)
            {
                _context.ChangeTracker.Clear();

                throw new AggregateException
                    ("Please check error message(s): ", errorList);
            }
            else
            {             
                _context.Cars.Add(car);
                _context.SaveChanges();
            }

            carView.CarID = car.CarId;
            return carView;
        }

        public MemberEditView EditMember(MemberEditView memberEdit)
        {


            return memberEdit;
        }

        private void UpdateCar(Car car)
        {
            if (car == null)
            {
                throw new ArgumentNullException("Car cannot be null", new ArgumentNullException());
            }

            var currentCar = _context.Cars.Find(car.CarId);
            
            if (currentCar == null)
            {
                throw new ArgumentException("Car not found", new ArgumentException());
            }
          
        }

        private void ValidateNewCarFields(CarListView car)
        {
            if (String.IsNullOrWhiteSpace(car.Description))
            {
                errorList.Add(new Exception("Description is required!"));
            }

            if (String.IsNullOrWhiteSpace(car.SerialNumber))
            {
                errorList.Add(new Exception("SerialNumber is required!"));
            }

            if (String.IsNullOrWhiteSpace(car.Ownership))
            {
                errorList.Add(new Exception("Ownership is required!"));
            }

            if (String.IsNullOrWhiteSpace(car.State))
            {
                errorList.Add(new Exception("Car state is required!"));
            }

            if (car.Class == 0)
            {
                errorList.Add(new Exception("Car class is required!"));
            }
        }

        private void ValidateEditMemberFields(MemberEditView member)
        {
            if(String.IsNullOrWhiteSpace(member.FirstName))
            {
                errorList.Add(new Exception("First name is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.LastName))
            {
                errorList.Add(new Exception("Last name is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.Address))
            {
                errorList.Add(new Exception("Address is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.City))
            {
                errorList.Add(new Exception("City is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.PostalCode))
            {
                errorList.Add(new Exception("PostalCode is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.Phone))
            {
                errorList.Add(new Exception("Phone is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.Email))
            {
                errorList.Add(new Exception("Email is required!"));
            }

            if (String.IsNullOrWhiteSpace(member.Certification))
            {
                errorList.Add(new Exception("Certification is required!"));
            }

            if (!IsValidAge(member.BirthDate))
            {
                errorList.Add(new Exception("Member should be at least 18 and less than 100 years old "));
            }

            List<CarListView> carList = member.CarList;


            //Validate CarListViw fields
            //ValidateNewCarFields(carList);

        }

        /// <summary>
        /// Checks for a valid date, if the member is 18+ < 100
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns>Age validation</returns>
        private bool IsValidAge(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18 && age <= 100;
        }



        //public Member? GetMemberById(int memberId)
        //{
        //    if (memberId == 0)
        //    {
        //        throw new ArgumentException("Please provide a valid customer ID");
        //    }

        //    var member = _context.Members
        //    .Where(m => m.MemberId == memberId)
        //    .Include(m => m.Cars)
        //    .FirstOrDefault();

        //    // If member is found, order the cars by CarId in descending order
        //    if (member != null)
        //    {
        //        member.Cars = member.Cars.OrderByDescending(c => c.CarId).ToList();
        //    }

        //    return member;
        //}

    }


}

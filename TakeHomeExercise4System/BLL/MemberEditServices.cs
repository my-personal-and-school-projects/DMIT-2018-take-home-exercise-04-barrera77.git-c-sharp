using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4System.BLL
{
    public class MemberEditServices
    {

        private readonly ERace2024R1Context _context;

        internal MemberEditServices( ERace2024R1Context context )
        {
            _context = context;
        }

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

        //TODO: find out if this is needed
        public List<OwnershipListView> GetOwnership()
        {
            return _context.Cars
                .Select(c => new OwnershipListView
                {
                    Ownership = c.Ownership
                })
                .ToList();
        }

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

        public Member? GetMemberById(int memberId)
        {
            if (memberId == 0)
            {
                throw new ArgumentException("Please provide a valid customer ID");
            }

            var member = _context.Members
            .Where(m => m.MemberId == memberId)
            .Include(m => m.Cars)
            .FirstOrDefault();

            // If member is found, order the cars by CarId in descending order
            if (member != null)
            {
                member.Cars = member.Cars.OrderByDescending(c => c.CarId).ToList();
            }

            return member;
        }

       

        public List<CarListView> GetCarsList(int memberId)
        {
            return _context.Cars
                 .Where(c => c.MemberId == memberId)
                 .Include(c => c.CarClass)
                 .Select(c => new CarListView
                 {
                     CarID = c.CarId,
                     TempID = c.CarId,
                     Description = c.Description,
                     SerialNumber = c.SerialNumber,
                     Ownership = c.Ownership,
                     State = c.State,
                     Class = c.CarClass.Description,
                     RemoveFromViewFlag = c.RemoveFromViewFlag,
                 }).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;

namespace TakeHomeExercise4System.BLL
{
    public class MemberServices
    {

        private readonly ERace2024R1Context _context;

        internal MemberServices( ERace2024R1Context context )
        {
            _context = context;
        }

        public List<Certification> Certifications_List()
        {
            return _context.Certifications.ToList();
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
    }
}

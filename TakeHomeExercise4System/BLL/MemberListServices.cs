using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;
using static MudBlazor.CategoryTypes;


namespace TakeHomeExercise4System.BLL
{
    public class MemberListServices
    {
        private readonly ERace2024R1Context _context;

        internal MemberListServices (ERace2024R1Context context)
        {
            _context = context;
        }

        public List<MemberListView> GetMembersList(string searchParam)
        {
            if (string.IsNullOrWhiteSpace(searchParam) || searchParam == "")
            {
                return _context.Members
                    .OrderBy(m => m.LastName)
                    .Select(m => new MemberListView
                    {
                        MemberID = m.MemberId,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        City = m.City,
                        Phone = m.Phone,
                        Email = m.EmailAddress,
                        Certification = _context.Certifications
                            .Where(c => c.CertificationLevel == m.CertificationLevel)
                            .Select(c => c.Description)
                            .FirstOrDefault(),
                        VehicleCount = _context.Cars
                            .Where(c => c.MemberId == m.MemberId)
                            .Select(c => c.CarId).Count()
                    })
                    .ToList();
            }
            else
            {
                return _context.Members
                    .Where(m => m.LastName.Contains(searchParam))
                    .OrderBy(m => m.LastName)
                    .Select(m => new MemberListView
                    {
                        MemberID = m.MemberId,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        City = m.City,
                        Phone = m.Phone,
                        Email = m.EmailAddress,
                        Certification = _context.Certifications
                            .Where(c => c.CertificationLevel == m.CertificationLevel)
                            .Select(c => c.Description)
                            .FirstOrDefault(),
                        VehicleCount = _context.Cars
                            .Where(c => c.MemberId == m.MemberId)
                            .Select(c => c.CarId).Count()
                    })
                    .ToList();
            }
        }
    }
}

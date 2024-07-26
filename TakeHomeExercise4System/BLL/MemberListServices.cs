﻿using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
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
            if (string.IsNullOrWhiteSpace(searchParam) || !char.IsLetter(searchParam[0]))
            {
                throw new ArgumentException($"{searchParam} is not valid lastName");
            }
            return _context.Members
                .Where(m => m.LastName.StartsWith(searchParam))
                .Select(m => new MemberListView
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
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

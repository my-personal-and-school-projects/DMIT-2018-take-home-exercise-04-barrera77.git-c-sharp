using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.ViewModels;


namespace TakeHomeExercise4System.BLL
{
    public class MemberListServices
    {
        private readonly ERace2024R1Context _context;

        internal MemberListServices (ERace2024R1Context context)
        {
            _context = context;
        }

        public List<MemberListView> GetMembersList()
        {
            return _context.Members

        }
    }
}

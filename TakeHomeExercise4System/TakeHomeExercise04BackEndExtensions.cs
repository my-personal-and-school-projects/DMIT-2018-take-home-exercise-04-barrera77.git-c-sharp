using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TakeHomeExercise4System.Entities;
using TakeHomeExercise4System.BLL;

namespace TakeHomeExercise4System
{
    public static class TakeHomeExercise04BackEndExtensions
    {

        public static void TakeHomeExercise4BackEndDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<ERace2024R1Context>(options);

            services.AddTransient<MemberListServices>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<ERace2024R1Context>();
                return new MemberListServices(context!);
            });

            services.AddTransient<MemberEditServices>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<ERace2024R1Context>();
                return new MemberEditServices(context!);
            });
        }
    }
}

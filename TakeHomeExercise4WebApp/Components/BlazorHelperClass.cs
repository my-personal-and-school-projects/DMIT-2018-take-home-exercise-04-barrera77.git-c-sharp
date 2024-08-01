namespace TakeHomeExercise4WebApp.Components
{
    public class BlazorHelperClass
    {
        public static Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}

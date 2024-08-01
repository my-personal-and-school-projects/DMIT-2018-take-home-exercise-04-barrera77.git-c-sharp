namespace TakeHomeExercise4WebApp.Components
{
    {
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}

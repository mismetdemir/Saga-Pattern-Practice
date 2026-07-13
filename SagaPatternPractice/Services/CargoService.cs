namespace SagaPatternPractice.Services
{
    public class CargoService
    {
        public async Task<bool> PrepareCargoAsync(bool failed)
        {
            return !failed;
        }
    }
}

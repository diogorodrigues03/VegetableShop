using VegetableShop.Console.Services;

namespace VegetableShop.Console.Interfaces
{
    public interface IExceptionHandler
    {
        ExitCodes Handle(Exception exception);
    }
}

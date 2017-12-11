using System.Collections.Generic;

namespace Lykke.blue.Api.Models
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; }

        public Dictionary<string, List<string>> ModelErrors { get; }

        private ErrorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ModelErrors = new Dictionary<string, List<string>>();
        }

        public static ErrorResponse Create(string message)
        {
            return new ErrorResponse(message);
        }
    }
}

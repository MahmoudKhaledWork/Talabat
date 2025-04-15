
namespace TalabatAPIs.Errors
{
    public class ApiResponse
    {
        public int StautsCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse( int stautsCode , string? message = null)
        {
            StautsCode = stautsCode;
            Message = message ?? GetDefaultMessageForStatusCode(stautsCode);
        }
        private string? GetDefaultMessageForStatusCode(int stautsCode)
        {
            return stautsCode switch
            {
                400 => "BadRequest",
                401 => "You Are Not Authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                 _  => null
            };
        }
    }
}

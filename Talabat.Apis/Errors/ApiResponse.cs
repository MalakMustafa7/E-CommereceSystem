
namespace Talabat.Apis.Errors
{
    public class ApiResponse
    {
        public int StatusCode{ get; set; }
        public string? Messege { get; set; }

        public ApiResponse(int statuscode , string? messege=null) {
               StatusCode = statuscode ;
                Messege = messege?? GetDefaultMessegeForStatusCode(StatusCode) ;
        }

        private string? GetDefaultMessegeForStatusCode(int? statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401=> "You Are Not Authorized",
                404=> "Resourse Not Found",
                500=> "Internal Server Error",
                _=> null
            };
        }
    }
}

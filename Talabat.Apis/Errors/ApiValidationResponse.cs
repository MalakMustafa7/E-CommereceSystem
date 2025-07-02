namespace Talabat.Apis.Errors
{
    public class ApiValidationResponse : ApiResponse
    {
        public IEnumerable<String> Errors { get; set; }

        public ApiValidationResponse() : base(400)
        {
            Errors = new List<String>();
        }

    }
}

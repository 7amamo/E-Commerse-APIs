namespace E_Commerce_Project.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {

        public IEnumerable<string> Errors { get; set; } 

        public ApiValidationErrorResponse() : base(400)
        {
            Errors = new List<string>();
        }

    }
}

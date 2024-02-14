namespace OnlineSaleSiteAuth.Application
{

    public class ServiceResponse
    {
        public ServiceResponse()
        {
            IsSuccessful = true;
            ErrorMessage = string.Empty;
        }

        public ServiceResponse(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = string.Empty;
        }

        public ServiceResponse(bool isSuccessful, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }

        public bool IsSuccessful { get; set; }
    }

    public class ServiceResponse<T>
    {
        public ServiceResponse(T data)
        {
            Data = data;
            IsSuccessful = true;
            ErrorMessage = string.Empty;
        }
        public ServiceResponse(T data, bool isSuccessful)
        {
            Data = data;
            IsSuccessful = isSuccessful;
            ErrorMessage = string.Empty;
        }

        public ServiceResponse(T data, bool isSuccessful, string errorMessage)
        {
            Data = data;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
        }

        public T Data { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSuccessful { get; set; }
    }
}

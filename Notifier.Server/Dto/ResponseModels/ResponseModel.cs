namespace Notifier.Server.Dto.ResponseModels
{
    public class ResponseModel
    {
        public int Code { get; private set; }
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public object? Data { get; private set; }
        public void SetSuccess(string message = "")
        {
            Success = true;
            Message = message;
            Code = 200;
        }

        public void SetFailed(string message = "", object data = null)
        {
            Success = false;
            Message = message;
            Code = 400;
            if (data != null)
            {
                Data = data;
            }
        }
    }

    public class ResponseModelFactory
    {
        public static ResponseModel GetResponseModel => new ResponseModel();
    }
}

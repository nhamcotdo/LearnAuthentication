namespace LearnAuthentication.LearnAuthentication.Common.Schemas
{
    public class ResponseInfo
    {
        public int Code { get; set; }

        public string Data { get; set; }

        public string Message { get; set; }

        public ResponseInfo()
        {
            Code = StatusCodes.Status200OK;
            Data = string.Empty;
            Message = string.Empty;
        }
    }
}
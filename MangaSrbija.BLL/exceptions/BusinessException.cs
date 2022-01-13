namespace MangaSrbija.BLL.exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException() : base() { }

        public BusinessException(string message, int StatusCode) : base(message)
        {
            this.StatusCode = StatusCode;
        }

        public int StatusCode { get; set; }

    }
}

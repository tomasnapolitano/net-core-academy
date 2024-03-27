using System.Globalization;

namespace Utils.Middleware
{
    public class UnavailableServiceException : Exception
    {
        public UnavailableServiceException() : base()
        {
        }
        public UnavailableServiceException(string message) : base(message)
        {
        }
        public UnavailableServiceException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}

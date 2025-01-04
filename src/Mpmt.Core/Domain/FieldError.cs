namespace Mpmt.Core.Domain
{
    public class FieldError
    {
        private string _field;
        private string _message;

        public string Field
        {
            get => _field ?? string.Empty;
            set => _field = value;
        }

        public string Message
        {
            get => _message;
            set => _message = value;
        }
    }
}

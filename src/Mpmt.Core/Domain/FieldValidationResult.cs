namespace Mpmt.Core.Domain
{
    public class FieldValidationResult
    {
        public void AddError(FieldError error) => Errors.Add(error);

        public void AddErrors(params FieldError[] errors) => Errors.AddRange(errors);

        public bool Success => !Errors.Any();

        public List<FieldError> Errors { get; private set; } = new();
    }
}

using FluentValidation.Results;


namespace BugTicketingSystem.BL.Exceptions
{
    public class BLValidationException : Exception
    {
        public List<ValidationFailure> Error { get; }
        public BLValidationException(List<ValidationFailure> errors)
        {


            Error = errors;
        }
    }
}

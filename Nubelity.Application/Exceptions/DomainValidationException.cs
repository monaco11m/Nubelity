

namespace Nubelity.Application.Exceptions
{
    public class DomainValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public DomainValidationException(IEnumerable<string> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }

        public DomainValidationException(string error)
            : this(new[] { error })
        {
        }

        public DomainValidationException(params string[] errors)
            : this((IEnumerable<string>)errors)
        {
        }
    }

}

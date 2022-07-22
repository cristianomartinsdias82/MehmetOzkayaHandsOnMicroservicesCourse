using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        private readonly Dictionary<string, string[]> _errors;

        public ValidationException() : base("One or more validation errors occurred.")
        {
            _errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            _errors = failures
                     .GroupBy(err => err.PropertyName, err => err.ErrorMessage)
                     .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IReadOnlyDictionary<string, string[]> Errors => _errors;
    }
}

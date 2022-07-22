using System.Collections.Generic;
using System.Linq;

namespace Solution.SharedKernel
{
    public struct OperationResult
    {
        private readonly IList<FailureDetail> _failureDetails;

        public OperationResult(string message, int code) : this(message, code, default) { }

        public OperationResult(string message, int code, IEnumerable<FailureDetail> failureDetails)
        {
            Message = message;
            Code = code;

            _failureDetails = default;
            if (failureDetails?.Any() ?? false)
                _failureDetails = failureDetails.ToList();
        }

        public void AddFailureDetail(FailureDetail failureDetail)
            => _failureDetails.Add(failureDetail);

        public bool IsSuccessful
        {
            get { return _failureDetails?.Any() ?? true; }
        }

        public string Message { get; }
        public int Code { get; }

        public IEnumerable<FailureDetail> FailureDetails => _failureDetails;
    }

    public struct FailureDetail
    {
        public string Field { get; init; }
        public string Details { get; init; }
    }
}

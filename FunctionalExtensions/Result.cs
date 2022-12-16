using System.Diagnostics;

namespace FunctionalExtensions
{
    public struct Result
    {
        private static readonly Result OkResult = new Result(isFailure: false, null);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic _logic;

        public bool IsFailure => _logic.IsFailure;

        public bool IsSuccess => _logic.IsSuccess;

        public string Error => _logic.Error;

        [DebuggerStepThrough]
        private Result(bool isFailure, string error)
        {
            _logic = new ResultCommonLogic(isFailure, error);
        }

        [DebuggerStepThrough]
        public static Result Ok()
        {
            return OkResult;
        }

        [DebuggerStepThrough]
        public static Result Fail(string error)
        {
            return new Result(isFailure: true, error);
        }

        [DebuggerStepThrough]
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(isFailure: false, value, null);
        }

        [DebuggerStepThrough]
        public static Result<T> Fail<T>(string error)
        {
            return new Result<T>(isFailure: true, default(T), error);
        }

        [DebuggerStepThrough]
        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            for (int i = 0; i < results.Length; i++)
            {
                Result result = results[i];
                if (result.IsFailure)
                {
                    return Fail(result.Error);
                }
            }

            return Ok();
        }

        [DebuggerStepThrough]
        public static Result Combine(string errorMessagesSeparator, params Result[] results)
        {
            List<Result> source = Enumerable.ToList(Enumerable.Where(results, (Result x) => x.IsFailure));
            if (!Enumerable.Any(source))
            {
                return Ok();
            }

            string error = string.Join(errorMessagesSeparator, Enumerable.ToArray(Enumerable.Select(source, (Result x) => x.Error)));
            return Fail(error);
        }

        [DebuggerStepThrough]
        public static Result Combine(params Result[] results)
        {
            return Combine(", ", results);
        }

        [DebuggerStepThrough]
        public static Result Combine<T>(params Result<T>[] results)
        {
            return Combine(", ", results);
        }

        [DebuggerStepThrough]
        public static Result Combine<T>(string errorMessagesSeparator, params Result<T>[] results)
        {
            Result[] results2 = Enumerable.ToArray(Enumerable.Select((IEnumerable<Result<T>>)results, (Func<Result<T>, Result>)((Result<T> result) => result)));
            return Combine(errorMessagesSeparator, results2);
        }
    }


    public struct Result<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic _logic;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly T _value;

        public bool IsFailure => _logic.IsFailure;

        public bool IsSuccess => _logic.IsSuccess;

        public string Error => _logic.Error;

        public T Value
        {
            [DebuggerStepThrough]
            get
            {
                if (!IsSuccess)
                {
                    throw new InvalidOperationException("There is no value for failure.");
                }

                return _value;
            }
        }

        [DebuggerStepThrough]
        internal Result(bool isFailure, T value, string error)
        {
            if (!isFailure && value == null)
            {
                throw new ArgumentNullException("value");
            }

            _logic = new ResultCommonLogic(isFailure, error);
            _value = value;
        }

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Result.Ok();
            }

            return Result.Fail(result.Error);
        }
    }

    internal sealed class ResultCommonLogic
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _error;

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public string Error
        {
            [DebuggerStepThrough]
            get
            {
                if (IsSuccess)
                {
                    throw new InvalidOperationException("There is no error message for success.");
                }

                return _error;
            }
        }

        [DebuggerStepThrough]
        public ResultCommonLogic(bool isFailure, string error)
        {
            if (isFailure)
            {
                if (string.IsNullOrEmpty(error))
                {
                    throw new ArgumentNullException("error", "There must be error message for failure.");
                }
            }
            else if (error != null)
            {
                throw new ArgumentException("There should be no error message for success.", "error");
            }

            IsFailure = isFailure;
            _error = error;
        }
    }
}
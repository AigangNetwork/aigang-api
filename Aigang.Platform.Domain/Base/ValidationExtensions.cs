using System.Collections.Generic;

namespace Aigang.Platform.Domain.Base
{
    public static class ValidationExtensions
    {
        public static void AddError(this IList<ValidationFailure> list, string errorReason)
        {
            list.Add(new ValidationFailure { Reason = errorReason });
        }

        public static void AddError(this IList<ValidationFailure> list, string errorReason, string errorMessage)
        {
            list.Add(new ValidationFailure { Reason = errorReason, Message = errorMessage });
        }
    }
}

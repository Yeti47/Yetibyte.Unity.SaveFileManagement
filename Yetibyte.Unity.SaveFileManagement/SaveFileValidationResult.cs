using System;
using System.Collections.Generic;
using System.Text;

namespace Yetibyte.Unity.SaveFileManagement {
    public class SaveFileValidationResult {

        public static readonly SaveFileValidationResult Valid = new SaveFileValidationResult();

        public virtual string ErrorMessage { get; }

        public virtual bool IsValid => string.IsNullOrWhiteSpace(ErrorMessage);

        public SaveFileValidationResult(string errorMessage = null) {

            ErrorMessage = errorMessage;

        }

    }
}

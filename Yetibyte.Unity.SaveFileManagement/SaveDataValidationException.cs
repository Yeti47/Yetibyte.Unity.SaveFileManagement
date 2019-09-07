using System;

namespace Yetibyte.Unity.SaveFileManagement {
    public class SaveDataValidationException : SaveDataException {

        private const string DEFAULT_ERR_MSG = "The save data is corrupt or invalid.";

        public SaveDataValidationException(string message, Exception innerException = null) : base(message, innerException) {

        }

        public SaveDataValidationException(Exception innerException) : base(DEFAULT_ERR_MSG, innerException) {

        }

    }

}

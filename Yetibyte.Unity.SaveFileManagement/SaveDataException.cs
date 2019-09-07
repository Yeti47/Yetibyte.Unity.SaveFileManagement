using System;
using System.Collections.Generic;
using System.Text;

namespace Yetibyte.Unity.SaveFileManagement {

    public class SaveDataException : Exception {

        private const string DEFAULT_ERR_MSG = "An error occurred while processing the save data.";

        public SaveDataException(string message, Exception innerException = null) : base(message, innerException) {

        }

        public SaveDataException(Exception innerException) : base(DEFAULT_ERR_MSG, innerException) {

        }

    }

}

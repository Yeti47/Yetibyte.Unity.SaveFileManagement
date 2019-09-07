using System;
using System.Collections.Generic;
using System.Text;

namespace Yetibyte.Unity.SaveFileManagement {
    public interface ISaveData {

        SaveFileValidationResult Validate(SaveDataValidationContext context);
    }
}

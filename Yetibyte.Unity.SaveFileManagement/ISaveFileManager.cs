using System.Collections.Generic;

namespace Yetibyte.Unity.SaveFileManagement {
    public interface ISaveFileManager {
        bool AllowOverwrite { get; set; }
        int ExistingSaveFileCount { get; }
        string FileExtension { get; set; }
        string SaveFileDirectory { get; set; }

        IEnumerable<string> EnumerateSaveFiles();
    }
}
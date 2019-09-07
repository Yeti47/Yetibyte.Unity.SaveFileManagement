using System;

namespace Yetibyte.Unity.SaveFileManagement {
    public interface ISaveable<T> where T : ISaveData {

        T CreateSaveData();

        bool LoadSaveData(T saveData);
        

    }

}

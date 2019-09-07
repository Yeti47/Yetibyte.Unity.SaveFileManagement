using System;
using System.Collections.Generic;
using System.Text;

namespace Yetibyte.Unity.SaveFileManagement.Events {

    public class SaveDataEventArgs<T> : EventArgs where T : ISaveData {

        #region Props

        public T SaveData { get; }

        public bool Cancel { get; set; } = false;

        #endregion

        #region Ctors

        public SaveDataEventArgs(T saveData) {

            SaveData = saveData;

        }

        #endregion

    }
}

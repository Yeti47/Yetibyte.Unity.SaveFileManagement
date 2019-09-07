using System;

namespace Yetibyte.Unity.SaveFileManagement {
    public class NamedSaveFileManager<T> : SaveFileManager<T> where T : ISaveData {

        #region Constants

        public const string DEFAULT_FILE_NAME = "savedata";

        #endregion

        #region Ctors
        public NamedSaveFileManager(string saveFileDirectory) : base(saveFileDirectory) {
        }

        #endregion

        #region Methods

        public bool Save(ISaveable<T> saveable, string fileName = DEFAULT_FILE_NAME) {

            if (saveable == null)
                throw new ArgumentNullException(nameof(saveable));

            return Save(saveable.CreateSaveData(), fileName);
        }

        public bool Save(T saveData, string fileName = DEFAULT_FILE_NAME) {

            fileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : DEFAULT_FILE_NAME;

            try {
                return SaveToFile(saveData, fileName);
            }
            catch (SaveDataValidationException ex) {
                throw ex;
            }
            catch (SaveDataException ex) {
                throw ex;
            }

        }

        public T Load(string fileName = DEFAULT_FILE_NAME) {

            fileName = !string.IsNullOrWhiteSpace(fileName) ? fileName : DEFAULT_FILE_NAME;

            try {
                return LoadFromFile(fileName);
            }
            catch (SaveDataValidationException ex) {
                throw ex;
            }
            catch (SaveDataException ex) {
                throw ex;
            }

        }

        public bool LoadInto(ISaveable<T> saveable, string fileName) {

            try {
                return LoadIntoSaveable(saveable, !string.IsNullOrWhiteSpace(fileName) ? fileName : DEFAULT_FILE_NAME);

            }
            catch (SaveDataValidationException ex) {
                throw ex;
            }
            catch (SaveDataException ex) {
                throw ex;
            }

        }

        #endregion
    }
}

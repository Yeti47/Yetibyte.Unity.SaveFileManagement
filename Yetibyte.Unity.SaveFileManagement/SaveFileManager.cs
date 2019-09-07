using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Text;
using Yetibyte.Unity.SaveFileManagement.Events;
using Yetibyte.Unity.SaveFileManagement.Util;

namespace Yetibyte.Unity.SaveFileManagement {

    public abstract class SaveFileManager<T> : ISaveFileManager where T : ISaveData {

        #region Constants

        public const string DEFAULT_FILE_EXTENSION = "sav";

        #endregion

        #region Fields

        private string _saveFileDirectory;
        private string _fileExtension;

        #endregion

        #region Props

        public bool AllowOverwrite { get; set; } = true;

        public string SaveFileDirectory {
            get => _saveFileDirectory.Trim(' ').TrimEnd(Path.DirectorySeparatorChar);
            set => _saveFileDirectory = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentNullException(nameof(value), "A valid save file directory must be provided");
        }

        public string FileExtension {
            get => !string.IsNullOrWhiteSpace(_fileExtension) ? _fileExtension.TrimStart('.') : DEFAULT_FILE_EXTENSION;
            set => _fileExtension = !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentNullException(nameof(value), "The file extension cannot be null or blank.");
        }

        public int ExistingSaveFileCount => EnumerateSaveFiles()?.Count() ?? 0;

        #endregion

        #region Events

        public event EventHandler<SaveDataEventArgs<T>> SavingData;
        public event EventHandler<SaveDataEventArgs<T>> DataSaved;

        public event EventHandler<SaveDataEventArgs<T>> LoadingData;
        public event EventHandler<SaveDataEventArgs<T>> DataLoaded;


        #endregion

        #region Ctors

        protected SaveFileManager(string saveFileDirectory) {

            SaveFileDirectory = saveFileDirectory;

        }

        #endregion

        #region Methods

        public virtual IEnumerable<string> EnumerateSaveFiles() {

            try {
                return Directory.EnumerateFiles(SaveFileDirectory, "*." + FileExtension, SearchOption.TopDirectoryOnly);

            }
            catch (Exception ex) {
                throw ex;
            }
        }

        protected virtual bool SaveToFile(ISaveable<T> saveable, string fileName) {

            if (saveable == null)
                throw new ArgumentNullException(nameof(saveable));

            return SaveToFile(saveable.CreateSaveData(), fileName);

        }

        protected virtual bool SaveToFile(T saveData, string fileName) {

            if (saveData == null)
                throw new ArgumentNullException(nameof(saveData));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (!ReflectionUtil.HasAttribute<SerializableAttribute>(saveData.GetType()))
                throw new SaveDataException($"The type implementing the {nameof(ISaveData)} interface must be marked as serializable.");

            SaveFileValidationResult validationResult = saveData.Validate(SaveDataValidationContext.Saving);

            if (!validationResult.IsValid)
                throw new SaveDataValidationException(validationResult.ErrorMessage);

            BinaryFormatter formatter = new BinaryFormatter();

            string filePath = CreateSaveFilePath(fileName);

            if (File.Exists(filePath) && !AllowOverwrite)
                return false;

            try {

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create)) {

                    formatter.Serialize(fileStream, saveData);

                }


            }
            catch (Exception ex) {
                throw new SaveDataException(ex.Message, ex);
            }

            return true;

        }

        protected virtual string CreateSaveFilePath(string fileName) => $"{SaveFileDirectory}{Path.DirectorySeparatorChar}{fileName}.{FileExtension}";

        protected virtual T LoadFromFile(string fileName) {

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            string filePath = CreateSaveFilePath(fileName);

            if (!File.Exists(filePath))
                return default(T);

            BinaryFormatter formatter = new BinaryFormatter();

            T saveData = default(T);

            try {

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open)) {

                    saveData = (T)formatter.Deserialize(fileStream);

                }

            }
            catch (Exception ex) {
                throw new SaveDataException(ex.Message, ex);
            }

            var validationResult = saveData.Validate(SaveDataValidationContext.Loading);

            if (!validationResult.IsValid)
                throw new SaveDataValidationException(validationResult.ErrorMessage);

            return saveData;

        }

        protected virtual bool LoadIntoSaveable(ISaveable<T> saveable, string fileName) {

            if (saveable == null)
                throw new ArgumentNullException(nameof(saveable));

            T saveData = LoadFromFile(fileName);

            return saveData != null && saveable.LoadSaveData(saveData);

        }

        #endregion


    }
}

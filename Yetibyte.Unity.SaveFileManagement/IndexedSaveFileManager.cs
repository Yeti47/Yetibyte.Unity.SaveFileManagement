using System;
using System.Collections.Generic;
using System.Text;

namespace Yetibyte.Unity.SaveFileManagement {

    public class IndexedSaveFileManager<T> : SaveFileManager<T> where T : ISaveData {

        #region Const

        public const int DEFAULT_MIN_INDEX = 1;
        public const int DEFAULT_MAX_INDEX = 99;

        public const string DEFAULT_BASE_FILE_NAME = "savefile";

        #endregion

        #region Fields

        private int _minIndex;
        private int _maxIndex;
        private string _baseFileName;

        #endregion

        #region Props

        public int MinIndex {
            get => _minIndex;
            set => _minIndex = Math.Max(0, Math.Min(value, _maxIndex));
        }

        public int MaxIndex {
            get => _maxIndex;
            set => _maxIndex = Math.Max(MinIndex, value);
        }

        public string BaseFileName {
            get => string.IsNullOrWhiteSpace(_baseFileName) ? DEFAULT_BASE_FILE_NAME : _baseFileName;
            set => _baseFileName = value;
        }

        #endregion

        #region Ctors

        public IndexedSaveFileManager(string saveFileDirectory, int minIndex, int maxIndex, string baseFileName = DEFAULT_BASE_FILE_NAME) : base(saveFileDirectory) {

            // Remember to set the MaxIndex first. Otherwise the constraints in the setters will prevent
            // the values from being set
            MaxIndex = maxIndex;
            MinIndex = minIndex;

            BaseFileName = baseFileName;

        }
        public IndexedSaveFileManager(string saveFileDirectory, string baseFileName = DEFAULT_BASE_FILE_NAME) : this(saveFileDirectory, DEFAULT_MIN_INDEX, DEFAULT_MAX_INDEX, baseFileName) {

        }

        #endregion

        #region Methods

        public bool Save(ISaveable<T> saveable, int index) {

            if (saveable == null)
                throw new ArgumentNullException(nameof(saveable));

            return Save(saveable.CreateSaveData(), index);
        }

        public bool Save(T saveData, int index) {

            if (saveData == null)
                throw new ArgumentNullException(nameof(saveData));

            if (index < _minIndex || index > _maxIndex)
                throw new ArgumentOutOfRangeException(nameof(index), $"The index must be between {_minIndex} and {_maxIndex}.");

            try {
                return SaveToFile(saveData, GetFileName(index));
            }
            catch(SaveDataValidationException ex) {
                throw ex;
            }
            catch(SaveDataException ex) {
                throw ex;
            }

        }

        public T Load(int index) {

            try {
                return LoadFromFile(GetFileName(index));
            }
            catch (SaveDataValidationException ex) {
                throw ex;
            }
            catch (SaveDataException ex) {
                throw ex;
            }

        }

        private string GetFileName(int index) => $"{BaseFileName}_{index}";

        public bool LoadInto(ISaveable<T> saveable, int index) {

            try {
                return LoadIntoSaveable(saveable, GetFileName(index));

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

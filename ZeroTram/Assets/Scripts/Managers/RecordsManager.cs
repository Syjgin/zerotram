using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class RecordsManager
    {
        private static RecordsManager _instance;

        private const string KeysString = "RecordKeys";
        private const string UsernameString = "Username";
        private const string DefaultUsername = "Безымянный кондуктор";
        private const char Delimiter = ',';
        private string _currentUserName;

        public static RecordsManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RecordsManager();
            }
            return _instance;
        }

        private Dictionary<string, int> _records; 

        private RecordsManager()
        {
            _records = new Dictionary<string, int>();
            string recordKeys = PlayerPrefs.GetString(KeysString);
            if (!string.IsNullOrEmpty(recordKeys))
            {
                string[] keys = recordKeys.Split(Delimiter);
                foreach (var key in keys)
                {
                    int recordValue = PlayerPrefs.GetInt(key);
                    _records.Add(key, recordValue);
                }
            }
            _currentUserName = PlayerPrefs.GetString(UsernameString, DefaultUsername);
        }

        public bool IsUsernameWasSet()
        {
            return _currentUserName != DefaultUsername;
        }

        public string GetCurrentUserName()
        {
            return _currentUserName;
        }

        public void SetCurrentUserName(string currentUserName)
        {
            if (string.IsNullOrEmpty(currentUserName))
            {
                _currentUserName = DefaultUsername;
                return;
            }
            _currentUserName = currentUserName.Replace(Delimiter.ToString(), "");
            PlayerPrefs.SetString(UsernameString, _currentUserName);
        }

        public void AddRecord(int record)
        {
            if (_records.ContainsKey(_currentUserName))
            {
                int prevRec = _records[_currentUserName];
                if (record > prevRec)
                {
                    WriteRecord(record);
                }
            } else
                WriteRecord(record);
        }

        private void WriteRecord(int record)
        {
            if (_records.ContainsKey(_currentUserName))
                _records.Remove(_currentUserName);
            _records.Add(_currentUserName, record);
            PlayerPrefs.SetInt(_currentUserName, record);
            string keys = string.Empty;
            int index = 0;
            foreach (var rec in _records)
            {
                keys += rec.Key;
                if (index != _records.Count - 1)
                {
                    keys += Delimiter;
                }
                index++;
            }
            PlayerPrefs.SetString(KeysString, keys);
        }

        public Dictionary<string, int> GetSortedRecords()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            int minValue = int.MaxValue;
            string minKey = string.Empty;
            foreach (var record in _records)
            {
                if (record.Value < minValue)
                {
                    minValue = record.Value;
                    minKey = record.Key;
                }
            }
            int currentMax = int.MaxValue;
            while (currentMax > minValue)
            {
                bool isResultFound = false;
                int loopMax = 0;
                string loopMaxKey = string.Empty;
                foreach (var record in _records)
                {
                    if (record.Value < currentMax)
                    {
                        if (record.Value > loopMax)
                        {
                            loopMax = record.Value;
                            loopMaxKey = record.Key;
                            isResultFound = true;
                        }
                    }
                }
                if(!isResultFound)
                    break;
                currentMax = loopMax;
                result.Add(loopMaxKey, loopMax);
            }
            if(!result.ContainsKey(minKey))
                result.Add(minKey, minValue);
            return result;
        }

        public int GetRecordCount()
        {
            return _records.Count;
        }
    }
}

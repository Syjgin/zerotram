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
        private String _currentUserName;

        public static RecordsManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RecordsManager();
            }
            return _instance;
        }

        private Dictionary<String, int> _records; 

        private RecordsManager()
        {
            _records = new Dictionary<string, int>();
            String recordKeys = PlayerPrefs.GetString(KeysString);
            if (!String.IsNullOrEmpty(recordKeys))
            {
                String[] keys = recordKeys.Split(Delimiter);
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

        public String GetCurrentUserName()
        {
            return _currentUserName;
        }

        public void SetCurrentUserName(String currentUserName)
        {
            if (String.IsNullOrEmpty(currentUserName))
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
            _records.Add(_currentUserName, record);
            PlayerPrefs.SetInt(_currentUserName, record);
            String keys = String.Empty;
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

        public Dictionary<String, int> GetSortedRecords()
        {
            Dictionary<String, int> result = new Dictionary<string, int>();
            int minValue = int.MaxValue;
            String minKey = String.Empty;
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
                String loopMaxKey = String.Empty;
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

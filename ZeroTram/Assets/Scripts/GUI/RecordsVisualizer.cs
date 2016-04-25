using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecordsVisualizer : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _recordEntityPrefab;
	// Use this for initialization
	void Start ()
	{
	    Dictionary<string, int> records = RecordsManager.GetInstance().GetSortedRecords();
	    int index = 0;
	    if (RecordsManager.GetInstance().GetRecordCount() == 0)
	    {
            GameObject instantiatedRecordEntity = Instantiate(_recordEntityPrefab);
            Text text = instantiatedRecordEntity.GetComponentInChildren<Text>();
            text.text = "Пока нет рекордов";
            instantiatedRecordEntity.transform.SetParent(_background.transform, false);
            instantiatedRecordEntity.transform.localPosition = new Vector3(0,0);
	    }
	    else
	    {
            foreach (var record in records)
            {
                GameObject instantiatedRecordEntity = Instantiate(_recordEntityPrefab);
                Text text = instantiatedRecordEntity.GetComponentInChildren<Text>();
                int realIndex = index + 1;
                text.text = realIndex + ". " + record.Key + " " + record.Value;
                instantiatedRecordEntity.transform.SetParent(_background.transform, false);
                instantiatedRecordEntity.transform.localPosition = new Vector3(0, -index * 50);
                index++;
            }   
	    }
        _background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, index * 50);
	}
	
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BenchCombinationManager : MonoBehaviour
{
    private int _currentCoef = 1;
    private Combination _previousCombination = Combination.NoCombination;
    private Combination _currentCombination;

    [SerializeField] private Bench _bench1;
    [SerializeField] private Bench _bench2;
    [SerializeField] private Bench _bench3;
    [SerializeField] private Bench _bench4;
    [SerializeField] private Bench _bench5;
    [SerializeField] private Bench _bench6;

    //will be replaced with current skin config
    public float GetSitPossibility()
    {
        return ConfigReader.GetConfig().GetField("tram").GetField("SitPossibility").n;
    }

    public float GetStandPossibility()
    {
        return ConfigReader.GetConfig().GetField("tram").GetField("StandPossibility").n;
    }

    public void Start()
    {
        _currentCoef = 1;
        _previousCombination = _currentCombination = Combination.NoCombination;
    }

    public int GetCombinationReward()
    {
        _currentCombination = CalculateCurrent();
        int baseReward =
            (int) ConfigReader.GetConfig().GetField("combinations").GetField(_currentCombination.ToString()).n;
        if (_previousCombination == _currentCombination)
            _currentCoef *= 2;
        _previousCombination = _currentCombination;
        return baseReward*_currentCoef;
    }

    public Combination CalculateCurrent()
    {
        List<String> currentNames = new List<string>();
        currentNames.Add(_bench1.CurrentPassengerClassName());
        currentNames.Add(_bench2.CurrentPassengerClassName());
        currentNames.Add(_bench3.CurrentPassengerClassName());
        currentNames.Add(_bench4.CurrentPassengerClassName());
        currentNames.Add(_bench5.CurrentPassengerClassName());
        currentNames.Add(_bench6.CurrentPassengerClassName());

        Dictionary<String, int> equalsDist = new Dictionary<string, int>();
        for (int i = 0; i < currentNames.Count; i++)
        {
            String example = currentNames[i];
            for (int j = 0; j < currentNames.Count; j++)
            {
                if (currentNames[j] == example && i != j)
                {
                    if (equalsDist.ContainsKey(example))
                    {
                        equalsDist[example]++;
                    }
                    else
                    {
                        equalsDist.Add(example, 1);
                    }
                }
            }
        }
        if(equalsDist.Count == 0)
            return Combination.NoCombination;
        if (equalsDist.Count == 1)
        {
            if(equalsDist.First().Value == 1)
                return Combination.TwoEquals;
            if (equalsDist.First().Value == 2)
                return Combination.ThreeEquals;
            if (equalsDist.First().Value == 3)
                return Combination.FourEquals;
            if (equalsDist.First().Value == 4)
                return Combination.FiveEquals;
            if (equalsDist.First().Value == 5)
                return Combination.SixEquals;
        }
        if (equalsDist.Count == 2)
        {
            bool twoFound = false;
            bool threeFound = false;
            bool fourFound = false;
            foreach (KeyValuePair<string, int> pair in equalsDist)
            {
                if (pair.Value == 1)
                    twoFound = true;
                if (pair.Value == 2)
                    threeFound = true;
                if (pair.Value == 3)
                    fourFound = true;
            }
            if (twoFound)
            {
                if (threeFound)
                    return Combination.ThreeAndTwo;
                if (fourFound)
                    return Combination.FourAndTwo;
                return Combination.TwoAndTwo;
            }
            if (threeFound)
                return Combination.TwoThrees;
        }
        if (equalsDist.Count == 3)
        {
            return Combination.ThreeTweens;
        }
        return Combination.NoCombination;
    }

    public enum Combination
    {
        NoCombination,
        TwoEquals,
        ThreeEquals,
        TwoAndTwo,
        ThreeTweens,
        ThreeAndTwo,
        TwoThrees,
        FourEquals,
        FourAndTwo,
        FiveEquals,
        SixEquals
    }
}
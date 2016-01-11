using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BenchCombinationManager : MonoBehaviour
{
    private int _currentCoef = 1;
    private Combination _previousCombination = Combination.NoCombination;
    private Combination _currentCombination;

    [SerializeField] private List<Bench> _benches;
    
    public void Start()
    {
        _currentCoef = 1;
        _previousCombination = _currentCombination = Combination.NoCombination;
    }

    public int GetCombinationReward()
    {
        _currentCombination = CalculateCurrent();
        Debug.Log(_currentCombination);
        int baseReward =
            (int) ConfigReader.GetConfig().GetField("combinations").GetField(_currentCombination.ToString()).n;
        if (_previousCombination == _currentCombination)
        {
            _currentCoef *= 2;
        }
        else
        {
            _currentCoef = 1;
        }
        _previousCombination = _currentCombination;
        return baseReward*_currentCoef;
    }

    public Combination CalculateCurrent()
    {
        List<string> currentNames = new List<string>();
        foreach (var bench in _benches)
        {
            currentNames.Add(bench.CurrentPassengerClassName());
        }

        Dictionary<string, int> equalsDist = new Dictionary<string, int>();
        Dictionary<string, int> temp = new Dictionary<string, int>();
        for (int i = 0; i < currentNames.Count; i++)
        {
            if (currentNames[i] == string.Empty)
            {
                continue;
            }
            if (!temp.ContainsKey(currentNames[i]))
            {
                temp.Add(currentNames[i], 0);
            }
            else
            {
                temp[currentNames[i]]++;
            }
        }
        foreach (KeyValuePair<string, int> pair in temp)
        {
            if(pair.Value > 0)
                equalsDist.Add(pair.Key, pair.Value);
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
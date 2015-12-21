using UnityEngine;

public class BenchCombinationManager : MonoBehaviour
{
    private int _currentCoef = 1;
    private Combination _previousCombination = Combination.NoCombination;

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
        _previousCombination = Combination.NoCombination;
    }

    public int GetCombinationReward(Combination currentCombination)
    {
        int baseReward =
            (int) ConfigReader.GetConfig().GetField("combinations").GetField(currentCombination.ToString()).n;
        if (_previousCombination == currentCombination)
            _currentCoef *= 2;
        _previousCombination = currentCombination;
        return baseReward*_currentCoef;
    }

    public enum BenchPosition
    {
        North,
        South,
        East,
        West,
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest
    }

    public enum Combination
    {
        NoCombination,
        Triangle,
        Line,
        GShape,
        TwoLines,
        Bracket,
        TShape,
        TwoColorLines,
        LineWithTwoPoints,
        Dao,
        TripleLines,
        TwoLinesTwoPoints,
        FullHouse
    }
}
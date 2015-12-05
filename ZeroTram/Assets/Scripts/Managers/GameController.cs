using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Assets.Scripts.Level;
using UnityEngine;

namespace Assets
{
    public class GameController
    {
        public enum BonusTypes
        {
            Wheel = 0,
            Ticket = 1,
            Boot = 2,
            Magnet = 3,
            Smile = 4,
            AntiHare = 5,
            SandGlass = 6,
            Vortex = 7,
            Snow = 8,
            Wrench = 9,
            Cogwheel = 10,
            Heal = 11,
            Clew = 12
        }
        private const int MAX_BONUS_COUNT = 3;
        private Dictionary<int, IBonus> _bonuses;

        private bool _isPassengersListChanged;
        private List<PassengerSM> _passengersToAdd;
        private List<PassengerSM> _passengersToDelete; 

        public void BonusEffectToPassengers(IBonus bonus, bool additition)
        {
            foreach (var passengerSm in _passengers)
            {
                if(additition)
                    bonus.AddEffect(passengerSm);
                else
                    bonus.RemoveEffect(passengerSm);
            }
            if (_isPassengersListChanged)
            {
                _isPassengersListChanged = false;
                foreach (var passengerSm in _passengersToDelete)
                {
                    _passengers.Remove(passengerSm);
                    MonoBehaviour.Destroy(passengerSm.gameObject);
                }
                foreach (var passengerSm in _passengersToAdd)
                {
                    _passengers.Add(passengerSm);
                }
                _passengersToAdd.Clear();
                _passengersToDelete.Clear();
            }
        }
        
        private float _minDistance;
        private bool _isGameFinished;

        private bool _isDoorsOpen;
        public bool IsDoorsOpen()
        {
            return _isDoorsOpen;
        }

        public void SetDoorsOpen(bool open)
        {
            _isDoorsOpen = open;
        }

        public bool IsGameFinished
        {
            get { return _isGameFinished; }
        }
        private float _maxHaresPercent;
        public float MaxHaresPercent
        {
            get { return _maxHaresPercent; }
        }
        private float _maxKilledPercent;
        public float MaxKilledPercent
        {
            get { return _maxKilledPercent; }
        }
        public class StateInformation
        {
            public int Killed;
            public int Hares;
            public int TicketCount;
            public int StationNumber;
            public bool IsConductorDied;
            public bool IsLevelFinished;
        }
        private static GameController _instance;

        private int _incomingPassengers;
        private int _killedPassengers;
        private int _totalHares;
        private int _ticketCount;

        private List<PassengerSM> _passengers;
        private List<GameStateNotificationListener> _listeners;

        private int _killedPercent;
        private int _haresPercent;

        private float _initialSpawnCount;
        private float _spawnIncrementCount;

        private int _currentSpawnCount;
        private int _currentStationNumber;
        
        public static GameController GetInstance()
        {
            if(_instance == null)
                _instance = new GameController();
            return _instance;
        }

        private int _stickPeriod;

        public int GetStickPeriod()
        {
            return _stickPeriod;
        }

        GameController()
        {
            _listeners = new List<GameStateNotificationListener>();
            _maxHaresPercent = ConfigReader.GetConfig().GetField("tram").GetField("MaxHarePercent").n;
            _maxKilledPercent = ConfigReader.GetConfig().GetField("tram").GetField("MaxKilledPercent").n;
            _initialSpawnCount = ConfigReader.GetConfig().GetField("tram").GetField("InitialSpawnCount").n;
            _spawnIncrementCount = ConfigReader.GetConfig().GetField("tram").GetField("SpawnIncrementCount").n;
            _minDistance = ConfigReader.GetConfig().GetField("tram").GetField("MinDistance").n;
            _stickPeriod = (int)ConfigReader.GetConfig().GetField("tram").GetField("StickPeriod").n;
            _passengers = new List<PassengerSM>();
            _bonuses = new Dictionary<int, IBonus>();
        }

        public bool IsBonusCanBeCreated()
        {
            return _bonuses.Count < MAX_BONUS_COUNT;
        }

        public void AddBonus(IBonus bonus)
        {
            int index = _bonuses.Count;
            _bonuses.Add(index, bonus);
        }

        public IBonus ActivateBonusByNumber(int number)
        {
            IBonus bonusToActivate = null;
            if(_bonuses.TryGetValue(number, out bonusToActivate))
            {
                bonusToActivate.Activate();
                _bonuses.Remove(number);
            }
            return bonusToActivate;
        }

        public void StartNewGame()
        {
            _passengers.Clear();
            _totalHares = 0;
            _incomingPassengers = 0;
            _currentSpawnCount = (int)_initialSpawnCount;
            _currentStationNumber = 0;
            _ticketCount = 0;
            _killedPassengers = 0;
            _killedPercent = 0;
            _haresPercent = 0;
            _isGameFinished = false;
            
            //for debug
            /*VortexBonus bonus = new VortexBonus();
            AddBonus(bonus);
            MonobehaviorHandler.GetMonobeharior().GetObject<BonusTimer>("bonusTimer").ActivateBonusByNumber(0);*/
        }

        public int GetCurrentStationNumber()
        {
            return _currentStationNumber;
        }

        public int GetCurrentSpawnCount()
        {
            return _currentSpawnCount;
        }

        public int GetPassengersCount()
        {
            return _passengers.Count();
        }

        public void AddListener(GameStateNotificationListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(GameStateNotificationListener listener)
        {
            _listeners.Remove(listener);
        }

        public void RegisterPassenger(PassengerSM ps)
        {
            if (ps.HasTicket())
                _incomingPassengers++;
            _totalHares++;
            _passengers.Add(ps);
            UpdateStats();
        }

        public void ReplacePassenger(PassengerSM newPassenger, PassengerSM oldPassenger)
        {
            if(_passengersToAdd == null)
                _passengersToAdd = new List<PassengerSM>();
            if(_passengersToDelete == null)
                _passengersToDelete = new List<PassengerSM>();
            _passengersToAdd.Add(newPassenger);
            _passengersToDelete.Add(oldPassenger);
            _isPassengersListChanged = true;
        }

        public void UpdatePassenger(PassengerSM ps)
        {
            if (ps.HasTicket())
            {
                if(_totalHares > 0)
                    _totalHares--;
                _ticketCount++;
            }
            UpdateStats();
        }

        private void Victory()
        {
            StateInformation info = new StateInformation();
            info.Hares = _haresPercent;
            info.Killed = _killedPercent;
            info.StationNumber = _currentStationNumber;
            info.TicketCount = _ticketCount;
            info.IsConductorDied = false;
            info.IsLevelFinished = true;
            foreach (var gameStateNotificationListener in _listeners)
            {
                gameStateNotificationListener.UpdateInfo(info);
                gameStateNotificationListener.GameOver();
            }
        }

        private void UpdateListeners(bool isCondutctorDied)
        {
            StateInformation info = new StateInformation();
            info.Hares = _haresPercent;
            info.Killed = _killedPercent;
            info.StationNumber = _currentStationNumber;
            info.TicketCount = _ticketCount;
            info.IsConductorDied = isCondutctorDied;
            foreach (var gameStateNotificationListener in _listeners)
            {
                gameStateNotificationListener.UpdateInfo(info);
            }
        }

        public bool IsAnybodyStick()
        {
            foreach (var passenger in _passengers)
            {
                if (passenger.IsStick())
                    return true;
            }
            return false;
        }

        public void RegisterDeath(MovableCharacterSM obj)
        {
            if (obj is ConductorSM)
            {
                GameOver(true);
            }
            else
            {
                if (obj is PassengerSM)
                {
                    var ps = obj as PassengerSM;
                    if (ps.HasTicket())
                    {
                        if (!ps.WasStickWhenFlyAway)
                        {
                            _killedPassengers++;  
                        }
                        if (!ps.IsVisibleToHero())
                        {
                            if(_totalHares > 0)
                                _totalHares--;   
                        }
                    }
                    else
                    {
                        if(_totalHares > 0)
                            _totalHares--;
                    }

                    _passengers.Remove(ps);
                }
                UpdateStats();
                if (_killedPercent > _maxKilledPercent)
                {
                    GameOver(false);
                }
                else
                {
                    if (_passengers.Count == 0)
                    {
                        MonobehaviorHandler.GetMonobeharior().GetObject<DoorsTimer>("DoorsTimer").StopNow();
                    }
                }
            }
        }

        public void GameOver(bool isConductorDied)
        {
            MonobehaviorHandler.GetMonobeharior().GetObject<Floor>("Floor").GetHero().StopDrag();
            _isGameFinished = true;
            Time.timeScale = 0;
            UpdateListeners(isConductorDied);
            foreach (var gameStateNotificationListener in _listeners)
            {
                gameStateNotificationListener.GameOver();
            }
        }

        private void UpdateStats()
        {
            _passengers.RemoveAll(item => item == null);
            float haresPercent = _passengers.Count > 0 ? (_totalHares / (float)_passengers.Count) : 0;
            _haresPercent = Mathf.Min(Mathf.RoundToInt(haresPercent * 100), 100);
            if (_incomingPassengers > 0)
            {
                float killedPercent = _killedPassengers/(float) _incomingPassengers;
                _killedPercent = Mathf.Min(Mathf.RoundToInt(killedPercent*100), 100);
            }
            else
            {
                _killedPercent = 0;
            }
            UpdateListeners(false);
        }

        private bool CheckStats()
        {
            UpdateStats();
            if (_haresPercent > MaxHaresPercent)
            {
                GameOver(false);
                return false;
            }
            return true;
        }

        public void CheckBeforeDoorsOpen()
        {
            _killedPassengers = 0;
            _incomingPassengers = _passengers.Count;
            if (CheckStats())
                NextStationReached();
        }

        private void NextStationReached()
        {
            _currentStationNumber++;
            if (_currentStationNumber == MapManager.GetInstance().GetCurrentCheckPointsCount())
            {
                MapManager.GetInstance().OpenNextLevel();
                Victory();
            }
            _currentSpawnCount += (int)_spawnIncrementCount;
            foreach (var passenger in _passengers)
            {
                passenger.IncrementStationCount();
            }
        }

        public bool IsPlaceFree(Vector2 place)
        {
            _passengers.RemoveAll(item => item == null);
            foreach (var passenger in _passengers)
            {
                Vector2 position = passenger.transform.position;
                float dist = (place - position).sqrMagnitude;
                if (dist < _minDistance)
                    return false;
            }
            return true;
        }

        public bool IsNearOtherPassenger(PassengerSM ps)
        {
            _passengers.RemoveAll(item => item == null);
            foreach (var passenger in _passengers)
            {
                if (passenger != ps)
                {
                    float dist = ((Vector2)passenger.transform.position - (Vector2)ps.transform.position).sqrMagnitude;
                    if (dist < _minDistance)
                        return true;
                }
            }
            return false;
        }

        public PassengerSM GetPassengerNearClick(Vector2 point)
        {
            _passengers.RemoveAll(item => item == null);
            foreach (var passenger in _passengers)
            {
                float dist = ((Vector2)passenger.transform.position - point).sqrMagnitude;
                if (dist < 1)
                {
                    return passenger;
                }
            }
            return null;
        }

        public List<PassengerSM> AllPassengersInDist(Vector2 point, float targetDist)
        {
            List<PassengerSM> result = new List<PassengerSM>();
            foreach (var passenger in _passengers)
            {
                float dist = ((Vector2)passenger.transform.position - point).sqrMagnitude;
                if (dist < targetDist)
                {
                    result.Add(passenger);
                }
            }
            return result;
        }

        public void KillStickPassenger()
        {
            foreach (var passengerSm in _passengers)
            {
                if (passengerSm.IsStick())
                {
                    RegisterDeath(passengerSm);
                    MonoBehaviour.Destroy(passengerSm.gameObject);
                    return;
                }
            }
        }

        public PassengerSM GetStickPassenger()
        {
            foreach (var passengerSm in _passengers)
            {
                if (passengerSm.IsStick())
                {
                    return passengerSm;
                }
            }
            return null;
        }
    }
}

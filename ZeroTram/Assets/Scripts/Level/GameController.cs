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
        private float _minDistance;

        private bool _isGameFinished;

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
            public int Successfull;
            public int StationNumber;
            public bool IsConductorDied;
        }
        private static GameController _instance;

        private int _incomingPassengers;
        private int _killedPassengers;
        private int _totalHares;
        private int _successfullyFinishedCount;

        private List<Passenger> _passengers;
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

        GameController()
        {
            _listeners = new List<GameStateNotificationListener>();
            _maxHaresPercent = ConfigReader.GetConfig().GetField("tram").GetField("MaxHarePercent").n;
            _maxKilledPercent = ConfigReader.GetConfig().GetField("tram").GetField("MaxKilledPercent").n;
            _initialSpawnCount = ConfigReader.GetConfig().GetField("tram").GetField("InitialSpawnCount").n;
            _spawnIncrementCount = ConfigReader.GetConfig().GetField("tram").GetField("SpawnIncrementCount").n;
            _minDistance = ConfigReader.GetConfig().GetField("tram").GetField("MinDistance").n;
            Init();
        }

        public void Init()
        {
            if(_passengers == null)
                _passengers = new List<Passenger>();
            else 
                _passengers.Clear();
            _totalHares = 0;
            _incomingPassengers = 0;
            _currentSpawnCount = (int)_initialSpawnCount;
            _currentStationNumber = 0;
            _successfullyFinishedCount = 0;
            _killedPassengers = 0;
            _killedPercent = 0;
            _haresPercent = 0;
            _isGameFinished = false;
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

        public void RegisterPassenger(Passenger ps)
        {
            if (ps.HasTicket())
                _incomingPassengers++;
            _totalHares++;
            _passengers.Add(ps);
            UpdateStats();
        }

        public void UpdatePassenger(Passenger ps)
        {
            if (ps.HasTicket())
            {
                if(_totalHares > 0)
                    _totalHares--;   
            }
            UpdateStats();
        }

        public void RegisterTravelFinish()
        {
            _successfullyFinishedCount++;
            UpdateListeners(false);
        }

        private void UpdateListeners(bool isCondutctorDied)
        {
            StateInformation info = new StateInformation();
            info.Hares = _haresPercent;
            info.Killed = _killedPercent;
            info.StationNumber = _currentStationNumber;
            info.Successfull = _successfullyFinishedCount;
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
                if (passenger.IsStick)
                    return true;
            }
            return false;
        }

        public void RegisterDeath(MovableObject obj)
        {
            if (obj as Hero != null)
            {
                GameOver(true);
            }
            else
            {
                Passenger ps = obj as Passenger;
                if (ps != null)
                {
                    if (ps.HasTicket())
                    {
                        if (!ps.WasStickWhenFlyAway)
                        {
                            _killedPassengers++;  
                        }
                        if (ps.IsVisibleToHero)
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
                }
                _passengers.Remove(ps);
                UpdateStats();
            }
        }

        public void UndragAll()
        {
            foreach (var passenger in _passengers)
            {
                passenger.SetDragged(false);
            }
        }

        public void GameOver(bool isConductorDied)
        {
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
            float haresPercent = _totalHares / (float)_passengers.Count;
            _haresPercent = Mathf.RoundToInt(haresPercent * 100);
            if (_incomingPassengers > 0)
            {
                float killedPercent = _killedPassengers/(float) _incomingPassengers;
                _killedPercent = Mathf.RoundToInt(killedPercent*100);
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
            if (_haresPercent > MaxHaresPercent || _killedPercent > MaxKilledPercent)
            {
                GameOver(false);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CheckBeforeDoorsOpen()
        {
            if(CheckStats())
                NextStationReached();
        }

        private void NextStationReached()
        {
            _currentStationNumber++;
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
                Vector2 position = passenger.GetPosition();
                float dist = (place - position).sqrMagnitude;
                if (dist < _minDistance)
                    return false;
            }
            return true;
        }
    }
}

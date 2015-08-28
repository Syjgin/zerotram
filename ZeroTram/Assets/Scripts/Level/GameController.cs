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
        public class StateInformation
        {
            public int Killed;
            public int Hares;
            public int Successfull;
            public int StationNumber;
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

        private const int InitialSpawnCount = 5;
        private const int SpawnIncrementCount = 3;

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
            _passengers = new List<Passenger>();
            _listeners = new List<GameStateNotificationListener>();
            _totalHares = 0;
            _incomingPassengers = 0;
            _currentSpawnCount = InitialSpawnCount;
            _currentStationNumber = 0;
            _successfullyFinishedCount = 0;
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

        public void RegisterPassenger(Passenger ps)
        {
            if (ps.HasTicket())
                _incomingPassengers++;
            else
                _totalHares++;
            _passengers.Add(ps);
            UpdateStats();
        }

        public void RegisterTravelFinish()
        {
            _successfullyFinishedCount++;
            UpdateListeners();
        }

        private void UpdateListeners()
        {
            StateInformation info = new StateInformation();
            info.Hares = _haresPercent;
            info.Killed = _killedPercent;
            info.StationNumber = _currentStationNumber;
            info.Successfull = _successfullyFinishedCount;
            foreach (var gameStateNotificationListener in _listeners)
            {
                gameStateNotificationListener.UpdateInfo(info);
            }
        }

        public void RegisterDeath(MovableObject obj)
        {
            if (obj as Hero != null)
            {
                GameOver();
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
                    }
                    else
                    {
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

        public void GameOver()
        {
            Debug.Log("game over");
            Time.timeScale = 0;
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
            UpdateListeners();
        }

        private bool CheckStats()
        {
            UpdateStats();
            if (_haresPercent > 90 || _killedPercent > 90)
            {
                Debug.Log("caused by stats");
                GameOver();
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
            _currentSpawnCount += SpawnIncrementCount;
            foreach (var passenger in _passengers)
            {
                passenger.IncrementStationCount();
            }
        }
    }
}

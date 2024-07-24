using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Data {
    public class GameState {
        private static GameState _instance;

        public static GameState Instance {
            get {
                if (_instance == null) {
                    Debug.Log("No GameStatesManager instance found in current scene");
                }

                return _instance;
            }
        }
        
        private readonly List<Func<GameStates>> _subscribers = new List<Func<GameStates>>();
        public void Subscribe(Func<GameStates> subscriber) {
            _subscribers.Add(subscriber);
        }

        private GameStates _state;
        public GameStates State {
            get => _state;
            set {
                _state = value;
                foreach (var subscriber in _subscribers) {
                    subscriber.Invoke();
                }
            }
        }
    }

    public enum GameStates {
        MainMenu,
        InGame,
        GameOver
    }
}
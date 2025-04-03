using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class GamePhaseSubject
    {
        private List<IGamePhaseObserver> observers = new List<IGamePhaseObserver>();

        private EGamePhases currentState = EGamePhases.Default;

        public void Subscribe(IGamePhaseObserver observer)
        {
            observers.Add(observer);
        }

        public void Remove(IGamePhaseObserver observer)
        {
            observers!.Remove(observer);
        }

        public void Notify(EGamePhases state)
        {
            if (state == currentState)
            {
                return;
            }

            currentState = state;

            foreach (IGamePhaseObserver observer in observers)
            {
                observer.Notify(state);
            }
        }
    }
}

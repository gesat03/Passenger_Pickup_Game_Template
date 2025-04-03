using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class BaseGamePhaseObserver : MonoBehaviour, IGamePhaseObserver
    {

        protected virtual void OnEnable()
        {
            GameManager.Instance.GameStateSub.Subscribe(this);
        }

        internal virtual void OnDisable()
        {
            GameManager.Instance.GameStateSub.Remove(this);
        }

        public virtual void LevelPreparingPhase() { }

        public virtual void StartPhase() { }

        public virtual void LevelWinPhase() { }

        public virtual void LevelLosePhase() { }

        public virtual void MenuPhase() { }

        public virtual void Notify(EGamePhases states)
        {
            switch (states)
            {
                case EGamePhases.LevelPreparing:
                    LevelPreparingPhase();
                    break;
                case EGamePhases.StartPhase:
                    StartPhase();
                    break;
                case EGamePhases.LevelWin:
                    LevelWinPhase();
                    break;
                case EGamePhases.LevelLose:
                    LevelLosePhase();
                    break;
                case EGamePhases.MenuPhase:
                    MenuPhase();
                    break;
                case EGamePhases.Default:
                    break;
                default:
                    break;
            }
        }
    }
}

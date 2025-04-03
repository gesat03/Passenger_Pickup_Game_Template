using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GamePhaseSubject GameStateSub;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            GameStateSub = new GamePhaseSubject();
        }

        private void Start()
        {
            GameStateSub.Notify(EGamePhases.StartPhase);
        }

        public void LevelPreparing()
        {
            GameStateSub.Notify(EGamePhases.LevelPreparing);
        }

        public void LevelStart()
        {
            GameStateSub.Notify(EGamePhases.StartPhase);
        }

        public void LevelWin()
        {
            GameStateSub.Notify(EGamePhases.LevelWin);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class UIManager : BaseGamePhaseObserver
    {
        public static UIManager Instance;

        [SerializeField] private GameObject _winPanel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public override void StartPhase()
        {
            base.StartPhase();

            _winPanel.SetActive(false);
        }

        public override void LevelWinPhase()
        {
            base.LevelWinPhase();

            _winPanel.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public interface IGamePhaseObserver
    {
        public void Notify(EGamePhases states);

        public void StartPhase();

        public void LevelWinPhase();

        public void LevelLosePhase();
    }
}

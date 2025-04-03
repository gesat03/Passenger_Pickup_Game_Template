using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class LevelEndTriger : BaseGamePhaseObserver
    {
        [SerializeField] private TransporterCrowdController _transporterCrowdController;

        private float _timeToWait = 1f;
        private float _timer = 0f;

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<WagonData>())
            {
                if(_timeToWait > _timer)
                {
                    _timer += Time.deltaTime;
                }

                _timer = 0;

                _transporterCrowdController.IsAllTransportersFull();
            }
        }
    }
}

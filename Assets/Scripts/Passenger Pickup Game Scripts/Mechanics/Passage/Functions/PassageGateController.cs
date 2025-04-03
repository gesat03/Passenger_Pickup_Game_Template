using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PPG
{
    public class PassageGateController : MonoBehaviour
    {
        //[SerializeField] float _placementTime = 0.2f;

        private NPCCrowdController _nPCCrowdCont;
        private TransporterCrowdController _transporterCrowdCont;

        private int _passageGateID;

        public void Initialize(int passageID, NPCCrowdController nPCCrowdCont, TransporterCrowdController transporterCrowdCont)
        {
            _nPCCrowdCont = nPCCrowdCont;
            _transporterCrowdCont = transporterCrowdCont;
            _passageGateID = passageID;
        }

        private void AddPassengers(int transporterID)
        {
            StartCoroutine(AddCoroutine(transporterID));
        }

        private IEnumerator AddCoroutine(int transporterID)
        {
            while (_nPCCrowdCont.IsAnyPassengerInTheGroup(_passageGateID)
                && _transporterCrowdCont.CheckAllEmptySeats(transporterID))
            {
                GameObject passenger = _nPCCrowdCont.GetPassenger(_passageGateID);

                if (!_transporterCrowdCont.CheckEmptySeatColorIsMatchingWithPassangerColor(transporterID, passenger.GetComponent<NPCData>().NPCColor)) break;

                _transporterCrowdCont.PlaceNPCtoEmptySeat(transporterID, passenger);

                _nPCCrowdCont.RearrangeGroup(_passageGateID);

                yield return new WaitForSeconds(0.25f);
            }

            _transporterCrowdCont.CheckAllEmptySeats(transporterID);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<WagonNPCPlacement>())
            {
                int transporterID = other.GetComponentInParent<TransporterMovement>()._TransporterID;
                AddPassengers(transporterID);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PPG
{
    public class WagonNPCPlacement : MonoBehaviour
    {
        [SerializeField] private WagonSeatData[] _HeadWagonSeatPos;
        [SerializeField] private WagonSeatData[] _MidWagonSeatPos;

        public void InitializePlacement()
        {
            SetSeatsEmpty();
        }

        public void SitTheNPCs(GameObject passenger)
        {
            //Vector3 seatPos = GetNPCSittingPlace();
            Transform seatTransform = GetNPCSittingPlace();

            if (seatTransform != null)
            {
                Debug.Log(seatTransform.position);
                passenger.transform.SetParent(transform);
                passenger.GetComponent<NPCMovement>().JumpToWagon(seatTransform);

                //passenger.transform.localPosition = seatPos;
            }
        }

        public Transform GetNPCSittingPlace()
        {
            if (_HeadWagonSeatPos.Length != 0)
            {
                for (int i = 0; i < _HeadWagonSeatPos.Length; i++)
                {
                    if (_HeadWagonSeatPos[i].IsOccupied) continue;
                    _HeadWagonSeatPos[i].IsOccupied = true;
                    return _HeadWagonSeatPos[i].SeatObject.transform;
                }
            }

            if (_MidWagonSeatPos.Length != 0)
            {
                for (int i = 0; i < _MidWagonSeatPos.Length; i++)
                {
                    if (_MidWagonSeatPos[i].IsOccupied) continue;
                    _MidWagonSeatPos[i].IsOccupied = true;
                    return _MidWagonSeatPos[i].SeatObject.transform;
                }
            }

            if (_HeadWagonSeatPos[_HeadWagonSeatPos.Length - 1].IsOccupied)
            {
                Debug.Log("No more seats available in head wagon");
                return null; // Vector3.zero;
            }

            if (_MidWagonSeatPos[_MidWagonSeatPos.Length - 1].IsOccupied)
            {
                Debug.Log("No more seats available in mid wagon");
                return null; // Vector3.zero;
            }

            return null; // Vector3.zero;
        }

        public bool IsThereEmptySeat()
        {
            if ((_HeadWagonSeatPos.Length != 0 && !_HeadWagonSeatPos[_HeadWagonSeatPos.Length - 1].IsOccupied) ||
                (_MidWagonSeatPos.Length != 0 && !_MidWagonSeatPos[_MidWagonSeatPos.Length - 1].IsOccupied))
            {
                Debug.Log("there is still empty seats");
                return true;
            }
            else
            {
                Debug.Log("no empty seats");
                return false;
            }
        }

        public void SetSeatsEmpty()
        {
            for (int i = 0; i < _HeadWagonSeatPos.Length; i++)
            {
                _HeadWagonSeatPos[i].IsOccupied = false;
            }
            for (int i = 0; i < _MidWagonSeatPos.Length; i++)
            {
                _MidWagonSeatPos[i].IsOccupied = false;
            }
        }
    }
}

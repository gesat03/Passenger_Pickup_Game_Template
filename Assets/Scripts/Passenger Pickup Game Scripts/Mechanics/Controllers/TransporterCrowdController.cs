using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace PPG
{
    public class TransporterCrowdController : MonoBehaviour
    {
        public TransporterGenerator transporterGenerator;

        public void PlaceNPCtoEmptySeat(int transporterID, GameObject passenger)
        {
            TransporterMovement transporter = transporterGenerator._TransporterMovementList[transporterID];

            foreach (var wagon in transporter.GetComponentsInChildren<WagonNPCPlacement>())
            {
                if (!wagon.gameObject.activeInHierarchy) continue;

                if (!wagon.IsThereEmptySeat()) continue;

                if (passenger.GetComponent<NPCData>().NPCColor != wagon.GetComponent<WagonData>().WagonColor) continue;

                wagon.SitTheNPCs(passenger);

                return;
            }
        }

        public bool CheckEmptySeatColorIsMatchingWithPassangerColor(int transporterID, EItemColor color)
        {
            TransporterMovement transporter = transporterGenerator._TransporterMovementList[transporterID];

            foreach (var wagon in transporter.GetComponentsInChildren<WagonNPCPlacement>())
            {
                if (!wagon.gameObject.activeInHierarchy) continue;

                if (!wagon.IsThereEmptySeat()) continue;

                if (wagon.GetComponent<WagonData>().WagonColor != color) continue;

                return true;
            }

            return false;
        }

        public bool IsAllTransportersFull()
        {
            foreach (var transporter in transporterGenerator._TransporterMovementList)
            {
                if (!transporter._IsCompliated) return false;
            }

            GameManager.Instance.LevelWin();

            return true;
        }

        public bool CheckAllEmptySeats(int transporterID)
        {
            TransporterMovement transporter = transporterGenerator._TransporterMovementList[transporterID];

            foreach (var wagon in transporter.GetComponentsInChildren<WagonNPCPlacement>())
            {
                if (!wagon.gameObject.activeInHierarchy) continue;

                if (wagon.IsThereEmptySeat()) return true;
            }

            transporter.SimulateFinalMove();

            return false;
        }

    }
}

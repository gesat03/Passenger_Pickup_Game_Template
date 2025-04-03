using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class NPCWagonPlacement : MonoBehaviour
    {
        [SerializeField] private NPCMovement _nPCMovement;
        [SerializeField] private NPCData _nPCData;

        public void SitNPC(Vector3 position)
        {
            GetComponent<NPCData>().IsSeated = true;
            GetComponent<NPCData>().NPCPosition = position;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class NPCData : MonoBehaviour
    {
        public bool IsSeated;
        public Vector3 NPCPosition;
        public EItemColor NPCColor;

        public void Initialized(bool seated, Vector3 nPCPosition, EItemColor nPCColor)
        {
            IsSeated = seated;
            NPCPosition = nPCPosition;
            NPCColor = nPCColor;
        }
    }
}

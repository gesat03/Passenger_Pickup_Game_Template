using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace PPG
{
    public class NPCCrowdController : MonoBehaviour
    {
        public NPCGenerator NPCGen;
        public PassageGenerator PassageGenerator;

        public GameObject GetPassenger(int groupIndex)
        {
            if (PassageGenerator.NPCGroupContainer.Count == 1) return null;
            GameObject nPC = PassageGenerator.NPCGroupContainer[groupIndex].transform.GetChild(1).gameObject;
            return nPC;
        }

        public void RearrangeGroup(int groupIndex) // Rearrange the group after a passenger is removed
        {
            Transform groupTransform = PassageGenerator.NPCGroupContainer[groupIndex].transform;

            Debug.Log("group Count: " + groupTransform.childCount);

            int childCount = groupTransform.childCount;
            float dist = 0f;

            for (int i = 1; i < childCount; i++)
            {
                groupTransform.GetChild(i).GetComponent<NPCMovement>().MoveToFrontLine(Vector3.forward * (dist/* * i*/));

                dist += 0.6f;
            }
        }

        public EItemColor CheckFirstPassengerColor(int groupIndex) 
        {
            if (PassageGenerator.NPCGroupContainer.Count == 1) return EItemColor.Default;

            return PassageGenerator.NPCGroupContainer[groupIndex].transform.GetChild(0).transform.GetComponent<NPCData>().NPCColor;
        }

        public bool IsAnyPassengerInTheGroup(int groupIndex)
        {
            if(PassageGenerator.NPCGroupContainer[groupIndex].transform.childCount == 1) 
            {
                PassageGenerator.NPCGroupContainer[groupIndex].GetComponent<PassageMaterialSetter>().SetMaterial(EItemColor.Default);
                return false; 
            }

            EItemColor color = PassageGenerator.NPCGroupContainer[groupIndex].transform.GetChild(1).GetComponent<NPCData>().NPCColor;

            PassageGenerator.NPCGroupContainer[groupIndex].GetComponent<PassageMaterialSetter>().SetMaterial(color);

            return true;
        }
    }
}

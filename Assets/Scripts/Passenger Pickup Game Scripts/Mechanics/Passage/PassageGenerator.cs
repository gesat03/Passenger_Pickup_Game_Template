using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class PassageGenerator : MonoBehaviour
    {
        private PassageFactory _passageFactory;

        public List<GameObject> NPCGroupContainer;

        public GameObject NPCContainer;

        public void Initialization() // Initialize the factory objects
        {
            NPCGroupContainer = new List<GameObject>();

            _passageFactory = new PassageFactory(new PassageObject(), NPCContainer);
        }

        public GameObject CreateGroupContainer(Vector3 pos, int passageID, NPCCrowdController nPCCrowdCont, TransporterCrowdController transporterCrowdCont)
        {
            GameObject containerObject = _passageFactory.CreateObj();
            NPCGroupContainer.Add(containerObject);

            containerObject.name = "NPC Group " + NPCGroupContainer.Count;
            containerObject.transform.position = pos;
            containerObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            containerObject.GetComponent<PassageGateController>().Initialize(passageID, nPCCrowdCont, transporterCrowdCont);
            return containerObject;
        }

        public void ReleaseAllPassages()
        {
            _passageFactory.ReleaseAllObj();
            NPCGroupContainer = new List<GameObject>();
        }
    }
}

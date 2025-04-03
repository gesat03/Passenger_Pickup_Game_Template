using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PPG
{
    public class NPCGenerator : MonoBehaviour
    {
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private PassageGenerator _passageGenerator;
        [SerializeField] private NPCCrowdController _nPCCrowdController;
        [SerializeField] private TransporterCrowdController _transporterCrowdCont;

        public GameObject NPCContainer;

        private List<int[]> NPCGroupSizeList;
        private List<Vector2> NPCGroupInitialPlacement;

        private NPCFactory _nPCFactory;

        public void Initialization() // Initialize the factory objects
        {
            //_nPCCrowdController.Initialization(NPCContainer);

            _nPCFactory = new NPCFactory(new NPCObject(), NPCContainer);
        }

        public void CreateNPC(List<EditorCellData> dataList)
        {
            Vector3 containerPos;

            for (int i = 0; i < NPCGroupSizeList.Count; i++)
            {
                containerPos = new Vector3(NPCGroupInitialPlacement[i].x, 0, NPCGroupInitialPlacement[i].y);
                GameObject containerObject = _passageGenerator.CreateGroupContainer(containerPos, i, _nPCCrowdController, _transporterCrowdCont);

                if (IsDownBorder(dataList[i].CellID))
                {
                    containerObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0)); //= new Vector3(0, 180, 0);
                }

                float dist = 0;

                for (int j = 0; j < NPCGroupSizeList[i].Length; j++)
                {
                    for (int z = 0; z < NPCGroupSizeList[i][j]; z++)
                    {
                        GameObject nPC = _nPCFactory.CreateObj();
                        nPC.transform.SetParent(containerObject.transform);
                        nPC.transform.localPosition = new Vector3(0, 0.125f, dist);
                        nPC.transform.localEulerAngles = Vector3.zero;

                        nPC.GetComponent<NPCMaterialSetter>().SetMaterial(dataList[i].PassengerColor[j]);
                        nPC.GetComponent<NPCData>().Initialized(false, nPC.transform.position, dataList[i].PassengerColor[j]);
                        //containerObject.GetComponent<PassageMaterialSetter>().SetMaterial(dataList[i].PassengerColor[j]);
                        dist += 0.6f;

                        if (z == 0 && j == 0)
                        {
                            containerObject.GetComponent<PassageMaterialSetter>().SetMaterial(dataList[i].PassengerColor[j]);
                        }
                    }
                }
            }
        }

        public void GetNPCPlacementDataFromEditorData(List<EditorCellData> dataList) 
        {
            NPCGroupSizeList = new List<int[]>();
            NPCGroupInitialPlacement = new List<Vector2>();

            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].Value == 0) continue;

                //List<Vector2> NPCPlacementList = new List<Vector2>();
                int[] passengerValue = dataList[i].PassengerValue; //Get the passenger value from the editor cell data

                Vector2 initialVec2 = BorderCellIDtoV3InitPos(dataList[i].CellID, _gridGenerator.Map); //Get the initial position for the NPC

                NPCGroupInitialPlacement.Add(initialVec2); //Get the initial position for the NPC

                NPCGroupSizeList.Add(dataList[i].PassengerValue); //Add the passenger value to the list
            }

            CreateNPC(dataList);
        }

        private Vector2 BorderCellIDtoV3InitPos(int mapPositionID, GridMap _map) // Convert the border cell ID to a Vector3 position
        {
            int id = mapPositionID;

            if (!IsDownBorder(id))
            {
                //Debug.Log("ID: " + id + " - " + _map.GetCellPositionWithID(id));
                return new Vector2(
                _map.GetCellPositionWithID(id).x,
                _map.GetCellPositionWithID(id).y + 1);
            }
            else
            {
                id = _gridGenerator.Rows * _gridGenerator.Columns 
                    - (LevelManager.Instance.CurrentLevelEditorAxis.x * LevelManager.Instance.CurrentLevelEditorAxis.y - (mapPositionID + 2));

                //Debug.Log("ID: " + id + " - " + _map.GetCellPositionWithID(id));
                return new Vector2(
                _map.GetCellPositionWithID(id).x,
                _map.GetCellPositionWithID(id).y - 1);
            }
        }

        private bool IsDownBorder(int id)
        {
            return id > _gridGenerator.Rows * _gridGenerator.Columns;
        }

        public void ReleaseComponent(GameObject component)
        {
            _nPCFactory.ReleaseObj(component);
        }

        public void ReleaseAllNPCs()
        {
            _nPCFactory.ReleaseAllObj();
        }

    }
}

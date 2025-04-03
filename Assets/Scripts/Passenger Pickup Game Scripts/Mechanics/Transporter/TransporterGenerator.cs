using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PPG
{
    public class TransporterGenerator : MonoBehaviour
    {
        [SerializeField] private GridGenerator gridGenerator;

        [SerializeField] private TransporterCrowdController transporterCrowdController; // Transporter Crowd Controller

        [SerializeField] private GameObject allTranportersContainer; // Container for all transporters

        internal List<TransporterMovement> _TransporterMovementList; // List of all transporters controllers

        private TransporterCompFact _transporterCompFact;  // Transporter Component Factory

        public void Initialization()
        {
            _TransporterMovementList = new List<TransporterMovement>();

            _transporterCompFact = new TransporterCompFact(new TransporterComp(), allTranportersContainer); //Create a new transporter component factory
        }

        public void GetTransportersPlacementDataFromEditorData(List<EditorCellData> dataList) // Get the transporter data from the editor data
        {
            List<List<int>> transporterList = new List<List<int>>(); // List of all transporters
            List<List<EItemColor>> colorList = new List<List<EItemColor>>(); // List of all transporters colors
            HashSet<int> processed = new HashSet<int>(); // Processed transporters

            for (int i = 0; i < dataList.Count; i++)
            {
                if (!processed.Contains(dataList[i].Value))
                {
                    List<int> group = new List<int>();
                    List<EItemColor> groupColor = new List<EItemColor>();
                    for (int j = i; j < dataList.Count; j++)
                    {
                        if (dataList[j].Value == dataList[i].Value)
                        {
                            group.Add(dataList[j].CellID);
                            groupColor.Add(dataList[j].WagonColor);
                            //Debug.Log("Transporter ID: " + dataList[j].Value + " Cell ID: " + group[group.Count - 1] + "Color: " + groupColor[j]);
                        }
                    }
                    colorList.Add(groupColor); // Add the group color to the color list
                    transporterList.Add(group); // Add the group to the transporter list
                    processed.Add(dataList[i].Value); // Add the processed transporter to the hashset

                    CreateTransporter(transporterList[transporterList.Count - 1].ToArray(), colorList[colorList.Count - 1]); // Create the transporter
                }
            }
        }

        public void CreateTransporter(int[] mapPosIDArray, List<EItemColor> colorList) //Create a new transporter with the given size of wagons atleast 3!
        {
            GameObject transporter = _transporterCompFact.CreateObj(); //Create a new transporter object
            transporter.transform.localPosition = Vector3.zero; // Set the transporter position to zero
            int transporterID = _transporterCompFact.ObjPool._poolList.IndexOf(transporter); // Get the transporter ID

            transporter.GetComponent<TransporterMovement>().Initialize(gridGenerator, mapPosIDArray.Length, transporterID); //Initialize the transporter controller
            _TransporterMovementList.Add(transporter.GetComponent<TransporterMovement>()); //Add a new transporter controller to the list

            _transporterCompFact.CreateWagonParts(gridGenerator.Map, mapPosIDArray, transporter, transporterID, colorList); //Create the wagons for the transporter
        }

        [ContextMenu("ReleaseAllTransporters")]
        public void ReleaseAllTransporters()
        {
            _transporterCompFact.ReleaseAllObj();
            _TransporterMovementList = new List<TransporterMovement>();
        }

        public void ReleaseSelectedTransporter(int index)
        {
            _transporterCompFact.ReleaseObj(_TransporterMovementList[index].gameObject);
            _TransporterMovementList.RemoveAt(index);
        }
    }

    #region Previous Codes
    /*
        public void SetWagonV3Position(int mapPositionID, GameObject wagon)
        {
            //Set the initial position of the wagon from the map position ID
            wagon.transform.position = new Vector3(
                _gridGenerator.Map.GetCellPositionWithID(mapPositionID).x, 
                0, 
                _gridGenerator.Map.GetCellPositionWithID(mapPositionID).y);

            // Set the map position ID of the wagon
            wagon.GetComponent<WagonData>().WagonPosID = mapPositionID;
        }


        public void CreateTransporter(int[] mapPosIDArray) //Create a new transporter with the given size of wagons atleast 3!
        {
            _transporterCompFact = new TransporterCompFact(new TransporterComp(), allTranportersContainer); //Create a new transporter component factory

            GameObject transporter = _transporterCompFact.CreateObj(); //Create a new transporter object
            transporter.GetComponent<TransporterController>().Initialize(_gridGenerator, mapPosIDArray.Length); //Initialize the transporter controller
            _transporterController.Add(transporter.GetComponent<TransporterController>()); //Add a new transporter controller to the list

            _transporterCompFact.CreateWagonParts(_gridGenerator.Map, mapPosIDArray, transporter); //Create the wagons for the transporter

            //_headCompFact = new WagonCompFact(new HeadWagonComp(), transporter); //Create a new head wagon component factory
            //_midCompFact = new WagonCompFact(new MidWagonComp(), transporter); //Create a new middle wagon component factory

            //transporter.GetComponent<TransporterController>().wagonList = new List<GameObject>(); //Create a new list of wagons for the transporter

            //for (int i = 0; i < mapPosIDArray.Length; i++) //Create the wagons for the transporter
            //{
            //    GameObject wagon;

            //    if (i == 0 || i == mapPosIDArray.Length - 1)
            //    {
            //        wagon = CreateWagonPart(EWagonPart.HeadWagon);

            //        transporter.GetComponent<TransporterController>().wagonList.Add(wagon); //Create a head wagon and add it to the list
            //    }
            //    else
            //    {
            //        wagon = CreateWagonPart(EWagonPart.MiddleWagon);

            //        transporter.GetComponent<TransporterController>().wagonList.Add(wagon); //Create a middle wagon and add it to the list
            //    }

            //    _gridGenerator.Map.SetCellOccupation(mapPosIDArray[i], true); //Set the cell occupation to true

            //    wagon.GetComponent<WagonMovement>().InitializeWagon(_gridGenerator.Map, mapPosIDArray[i], EWagonDirections.Forward); // Initialize the wagon
            //}
        }

    */
    #endregion
}

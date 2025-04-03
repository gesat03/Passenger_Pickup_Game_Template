using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class TransporterCompFact : BaseObjectFactory
    {
        private IPoolObject _transporterComponent;

        private List<WagonCompFact> _headCompFactList;  // Head Wagon Component Factory
        private List<WagonCompFact> _midCompFactList;  // Middle Wagon Component Factory

        public TransporterCompFact(IPoolObject wagonComponent, GameObject transporter)
        {
            _transporterComponent = wagonComponent;

            ObjContainer = transporter;

            CreateObjPool();
        }

        private void CreateObjPool()
        {
            ObjPool = new ObjectPool(_transporterComponent.Size(), _transporterComponent.Root(), ObjContainer);

            CreateWagonCompFactories();
        }

        public void CreateWagonParts(GridMap map, int[] mapPosIDArray, GameObject transporter, int transPorterID, List<EItemColor> colorList)
        {
            transporter.GetComponent<TransporterMovement>()._WagonList = new List<GameObject>(); //Create a new list of wagons for the transporter

            for (int i = 0; i < mapPosIDArray.Length; i++) //Create the wagons for the transporter
            {
                GameObject wagon;

                if (i == 0 || i == mapPosIDArray.Length - 1)
                {
                    wagon = CreateWagonPart(EWagonPart.HeadWagon, transporter.transform, transPorterID);

                    transporter.GetComponent<TransporterMovement>()._WagonList.Add(wagon); //Create a head wagon and add it to the list

                    if (i == 0) wagon.GetComponent<WagonData>().IsWagonHeaded(true); //Set the wagon headed to true
                    if (i == mapPosIDArray.Length - 1) wagon.GetComponent<WagonData>().IsWagonHeaded(false); //Set the wagon headed to false
                }
                else
                {
                    wagon = CreateWagonPart(EWagonPart.MiddleWagon, transporter.transform, transPorterID);

                    transporter.GetComponent<TransporterMovement>()._WagonList.Add(wagon); //Create a middle wagon and add it to the list
                }

                map.SetCellOccupation(mapPosIDArray[i], true); //Set the cell occupation to true

                //Debug.Log("Color List: " + colorList[i]);

                wagon.GetComponent<WagonMovement>().InitializeWagon(map, mapPosIDArray[i], EWagonDirections.Forward, colorList[i]); // Initialize the wagon
                wagon.GetComponent<WagonNPCPlacement>().InitializePlacement(); // Initialize the wagon NPC placement
            }
        }

        private void CreateWagonCompFactories() // Create factories for the wagon components
        {
            _headCompFactList = new List<WagonCompFact>(); //Create a new head wagon component factory
            _midCompFactList = new List<WagonCompFact>(); //Create a new middle wagon component factory

            for (int i = 0; i < ObjPool._poolList.Count; i++)
            {
                WagonCompFact _headCompFact = new WagonCompFact(new HeadWagonComp(), ObjPool._poolList[i]);
                _headCompFactList.Add(_headCompFact);

                WagonCompFact _midCompFact = new WagonCompFact(new MidWagonComp(), ObjPool._poolList[i]);
                _midCompFactList.Add(_midCompFact);
            }
        }

        private GameObject CreateWagonPart(EWagonPart wagonPart, Transform parent, int parentID)
        {
            //GameObject wagon;

            switch (wagonPart)
            {
                case EWagonPart.HeadWagon:
                    return _headCompFactList[parentID].CreateObj();
                case EWagonPart.MiddleWagon:
                    return _midCompFactList[parentID].CreateObj();
                default:
                    return _headCompFactList[parentID].CreateObj();
            }
        }

        public override void ReleaseObj(GameObject obj)
        {
            int parentID = ObjPool._poolList.IndexOf(obj);

            _headCompFactList[parentID].ReleaseAllObj();
            _midCompFactList[parentID].ReleaseAllObj();
            base.ReleaseObj(obj);
        }

        public override void ReleaseAllObj()
        {
            for (int i = 0; i < _headCompFactList.Count; i++)
            {
                _headCompFactList[i].ReleaseAllObj();
                _midCompFactList[i].ReleaseAllObj();
            }
            base.ReleaseAllObj();
        }
    }
}

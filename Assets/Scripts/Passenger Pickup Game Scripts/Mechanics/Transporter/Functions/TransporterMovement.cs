using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PPG
{
    public class TransporterMovement : MonoBehaviour
    {
        internal GridGenerator _GridGenerator;
        internal List<GameObject> _WagonList = new List<GameObject>();

        internal int _TransporterID;

        internal bool _IsSelected = false;
        internal bool _IsReversed = false;
        internal bool _IsCompliated = false;

        private bool _isTracking = false;
        private bool _isAbleToMove = true;
        private Vector2 _mouseStartPos;
        private float _minDragOffset = 20f; // Minimum scroll distance (pixels)
        private float _directionChangeDelay = 0.1f; // Wait time to change direction (seconds)
        private float _lastDirectionTime = 0f;
        private EWagonDirections _lastDirection = EWagonDirections.Default;

        private int[] _prevPosIDs;
        private EWagonDirections[] _prevRotIDs;

        void Update()
        {
            HandleKeyboardInput();

            HandleMouseInput();
        }

        public void Initialize(GridGenerator gridgen, int wagonSize, int transporterID)
        {
            _isTracking = false;
            _IsSelected = false;
            _IsCompliated = false;
            _isAbleToMove = true;
            _GridGenerator = gridgen;
            _TransporterID = transporterID;
            _prevPosIDs = new int[wagonSize];
            _prevRotIDs = new EWagonDirections[wagonSize];
            _lastDirection = EWagonDirections.Default;
        }

        private void HandleMouseInput()
        {
            if (!_isAbleToMove) return;

            EWagonDirections direction = GetCurrentDragDirection();

            if (direction != EWagonDirections.Default && _IsSelected)
            {
                MoveTransporter(direction, _IsReversed);
            }
        }

        private void HandleKeyboardInput()
        {
            if (!_isAbleToMove) return;

            EWagonDirections moveDirection = EWagonDirections.Default;

            if (Input.GetKeyDown(KeyCode.W)) moveDirection = EWagonDirections.Forward;
            if (Input.GetKeyDown(KeyCode.S)) moveDirection = EWagonDirections.Backward;
            if (Input.GetKeyDown(KeyCode.A)) moveDirection = EWagonDirections.Left;
            if (Input.GetKeyDown(KeyCode.D)) moveDirection = EWagonDirections.Right;

            if (moveDirection != EWagonDirections.Default && _IsSelected)
            {
                MoveTransporter(moveDirection, _IsReversed);
            }
        }

        public EWagonDirections GetCurrentDragDirection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseStartPos = Input.mousePosition;
                _isTracking = true;
                _lastDirection = EWagonDirections.Default;
                return EWagonDirections.Default;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isTracking = false;
                _lastDirection = EWagonDirections.Default;
                return EWagonDirections.Default;
            }

            if (_isTracking)
            {
                Vector2 currentMousePos = Input.mousePosition;
                Vector2 dragOffset = currentMousePos - _mouseStartPos;

                // If offset is not enough return Default
                if (dragOffset.magnitude < _minDragOffset)
                {
                    return EWagonDirections.Default;
                }

                // Calculate direction
                float angle = Mathf.Atan2(dragOffset.y, dragOffset.x) * Mathf.Rad2Deg;
                EWagonDirections newDirection = EWagonDirections.Default;

                if (angle > -45f && angle <= 45f)
                    newDirection = EWagonDirections.Right;
                else if (angle > 45f && angle <= 135f)
                    newDirection = EWagonDirections.Forward;
                else if (angle > 135f || angle <= -135f)
                    newDirection = EWagonDirections.Left;
                else if (angle > -135f && angle <= -45f)
                    newDirection = EWagonDirections.Backward;

                // If a new direction is specified and (either the first time or the waiting time has expired)
                if (newDirection != EWagonDirections.Default)
                {
                    if (_lastDirection == EWagonDirections.Default ||
                        (/*newDirection != _lastDirection &&*/ Time.time - _lastDirectionTime >= _directionChangeDelay))
                    {
                        _lastDirection = newDirection;
                        _lastDirectionTime = Time.time;
                        return newDirection;
                    }
                }
            }

            return EWagonDirections.Default;
        }

        private void MoveTransporter(EWagonDirections direction, bool isRevered)
        {
            int currentPosID;

            // 1. Get current position of the first wagon
            if (!isRevered)
            {
                currentPosID = _WagonList[0].GetComponent<WagonData>().WagonPosID;
            }
            else
            {
                currentPosID = _WagonList[_WagonList.Count - 1].GetComponent<WagonData>().WagonPosID;
            }
   
            // 2.Calculate target location ID
            int targetPosID = TargetPositionID(direction, currentPosID);

            // 3.Grid boundary control
            if (!IsPositionValid(direction, targetPosID))
            {
                Debug.Log("Out of boundry!");
                return;
            }

            // 4. Wagon collision check (ALL wagons are checked)
            if (IsPositionOccupied(targetPosID))
            {
                Debug.Log("There is a wagon at the target location!");
                return;
            }

            // 5. Perform the move
            UpdateTransporterPositions(targetPosID, direction, isRevered);
        }

        private bool IsPositionValid(EWagonDirections direction, int targetPosID) // Check if the target position is within the grid boundaries
        {
            if(targetPosID < 1)
            {
                return false;
            }

            switch (direction)
            {
                case EWagonDirections.Forward:
                    return targetPosID > 0;
                case EWagonDirections.Backward:
                    return targetPosID <= _GridGenerator.Rows * _GridGenerator.Columns;
                case EWagonDirections.Left:
                    return targetPosID % _GridGenerator.Columns != 0;
                case EWagonDirections.Right:
                    return targetPosID % _GridGenerator.Columns != 1;
                case EWagonDirections.Default:
                    return false;
                default:
                    return false;
            }
        }

        private bool IsPositionOccupied(int targetPosID) // Check if the target position is occupied by another wagon
        {
            if (targetPosID < 1)
            {
                return false;
            }

            if (_GridGenerator.Map.IsCellOccupied(targetPosID))//wagon.GetComponent<WagonData>().WagonPosID == targetPosID
            {
                return true;
            }

            return false;
        }

        private int TargetPositionID(EWagonDirections direction, int wagonCurPosID) // Return the target position ID
        {
            switch (direction)
            {
                case EWagonDirections.Forward:
                    return wagonCurPosID - _GridGenerator.Columns;
                case EWagonDirections.Backward:
                    return wagonCurPosID + _GridGenerator.Columns;
                case EWagonDirections.Right:
                    return wagonCurPosID + 1;
                case EWagonDirections.Left:
                    return wagonCurPosID - 1;
                case EWagonDirections.Default:
                    return - 1;
                default:
                    return -1;
            }
        }

        [ContextMenu("SimulateFinalMove")]
        public void SimulateFinalMove()
        {
            if (_IsCompliated) return;

            StartCoroutine(CoroutineMovement());
        }

        private IEnumerator CoroutineMovement()
        {
            transform.DOMoveY(1, 1);

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < 10; i++)
            {
                FinalMove();

                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.5f);

            _IsCompliated = true;
        } 

        private void FinalMove()
        {
            _isAbleToMove = false;

            Vector3[] prevPos = new Vector3[_WagonList.Count];
            Quaternion[] prevRot = new Quaternion[_WagonList.Count];

            // Save all previous positions
            for (int i = 0; i < _WagonList.Count; i++)
            {
                prevPos[i] = _WagonList[i].GetComponent<WagonData>().transform.localPosition;
                prevRot[i] = _WagonList[i].GetComponent<WagonData>().transform.localRotation;
            }

            // Move the trasporter
            _WagonList[0].GetComponent<WagonMovement>().FinalMove(_WagonList[0].transform.localPosition + Vector3.down);
            // Update the cell occupation
            _GridGenerator.Map.SetCellOccupation(_WagonList[0].GetComponent<WagonData>().WagonPosID, false);

            // Update the other wagons
            for (int i = 1; i < _WagonList.Count; i++)
            {
                // Move the wagon
                _WagonList[i].GetComponent<WagonMovement>().FinalMove(prevPos[i - 1], prevRot[i - 1]);
                // Update the cell occupation
                _GridGenerator.Map.SetCellOccupation(_WagonList[i].GetComponent<WagonData>().WagonPosID, false);
            }
        }

        private void UpdateTransporterPositions(int newPosID, EWagonDirections direction, bool isReversed)
        {
            if (!isReversed)
            {
                // Save all previous positions
                for (int i = 0; i < _WagonList.Count; i++)
                {
                    _prevRotIDs[i] = _WagonList[i].GetComponent<WagonData>().WagonDirection;
                    _prevPosIDs[i] = _WagonList[i].GetComponent<WagonData>().WagonPosID;
                    _GridGenerator.Map.SetCellOccupation(_prevPosIDs[i], false);
                }

                // Update wagon as first
                _WagonList[0].GetComponent<WagonData>().IsWagonLast = false;
                // Move the trasporter
                _WagonList[0].GetComponent<WagonMovement>().TouchMove(newPosID, direction);
                // Update the cell occupation
                _GridGenerator.Map.SetCellOccupation(newPosID, true);

                // Update the other wagons
                for (int i = 1; i < _WagonList.Count; i++)
                {
                    // Move the wagon
                    _WagonList[i].GetComponent<WagonMovement>().TouchMove(_prevPosIDs[i - 1], _prevRotIDs[i - 1]);
                    // Update wagon as last
                    if (i == _WagonList.Count - 1) _WagonList[i].GetComponent<WagonData>().IsWagonLast = true;
                    // Update the cell occupation
                    _GridGenerator.Map.SetCellOccupation(_prevPosIDs[i - 1], true);
                }
            }
            else
            {
                for (int i = _WagonList.Count - 1; i >= 0; i--)
                {
                    _prevRotIDs[i] = _WagonList[i].GetComponent<WagonData>().WagonDirection;
                    _prevPosIDs[i] = _WagonList[i].GetComponent<WagonData>().WagonPosID;
                    _GridGenerator.Map.SetCellOccupation(_prevPosIDs[i], false);
                }

                // Update wagon as first
                _WagonList[_WagonList.Count - 1].GetComponent<WagonData>().IsWagonLast = false;
                // Move the trasporter
                _WagonList[_WagonList.Count - 1].GetComponent<WagonMovement>().TouchMove(newPosID, direction);
                // Update the cell occupation
                _GridGenerator.Map.SetCellOccupation(newPosID, true);

                // Update the other wagons
                for (int i = _WagonList.Count - 2; i >= 0; i--)
                {
                    // Move the wagon
                    _WagonList[i].GetComponent<WagonMovement>().TouchMove(_prevPosIDs[i + 1], _prevRotIDs[i + 1]);
                    // Update wagon as last
                    if (i == 0) _WagonList[i].GetComponent<WagonData>().IsWagonLast = true;
                    // Update the cell occupation
                    _GridGenerator.Map.SetCellOccupation(_prevPosIDs[i + 1], true);
                }
            }
        }
        public void SetInitialWagonRotations()
        {
            EWagonDirections initialWagonDirection = _WagonList[0].GetComponent<WagonData>().WagonDirection;
        }
    }
    #region Previous Codes
    //private bool IsPositionOccupied(Vector3 targetPos)
    //    {
    //        Vector3 targetGrid = new Vector3(
    //            Mathf.Round(targetPos.x / _gridSize) * _gridSize,
    //            Mathf.Round(targetPos.z / _gridSize) * _gridSize,
    //            0
    //        );

    //        foreach (Transform wagon in wagons)
    //        {
    //            Vector3 wagonGrid = new Vector3(
    //                Mathf.Round(wagon.position.x / _gridSize) * _gridSize,
    //                Mathf.Round(wagon.position.z / _gridSize) * _gridSize,
    //                0
    //            );

    //            if (wagonGrid == targetGrid) return true;
    //        }
    //        return false;
    //    }



    /*
        void HandleInput()
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector3.forward;
            if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector3.back;
            if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector3.left;
            if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector3.right;

            if (moveDirection != Vector3.zero)
            {
                TryMoveTrain(moveDirection);
            }
        }    
    
            private EWagonDirections SetWagonRotationDirection(int previousWagonID, int currentWagonID)
        {
            int diff = currentWagonID - previousWagonID;

            if (diff == -gridGenerator.Rows) return EWagonDirections.Forward;
            if (diff == gridGenerator.Rows) return EWagonDirections.Backward;
            if (diff == 1) return EWagonDirections.Right;
            if (diff == -1) return EWagonDirections.Left;
            else return EWagonDirections.Default;
        }
     */

    //void SetWagons(int mapGridID)
    //{
    //    wagons = new Transform[transform.childCount];

    //    _previousPositionsID = new int[wagons.Length];

    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        wagons[i] = transform.GetChild(i);
    //        //_previousPositionsID[i] = wagons[i].position;
    //    }
    //}

    /*
     * private bool IsPositionValid(Vector3 pos)
        {
            // Check grid boundaries
            return pos.x >= _horizontalBoundry.x && pos.x <= _horizontalBoundry.y && pos.z >= _verticalBoundry.x && pos.z <= _verticalBoundry.y;
        }

        private bool IsPositionOccupied(Vector3 targetPos)
        {
            Vector3 targetGrid = new Vector3(
                Mathf.Round(targetPos.x / _gridSize) * _gridSize,
                Mathf.Round(targetPos.z / _gridSize) * _gridSize,
                0
            );

            foreach (Transform wagon in wagons)
            {
                Vector3 wagonGrid = new Vector3(
                    Mathf.Round(wagon.position.x / _gridSize) * _gridSize,
                    Mathf.Round(wagon.position.z / _gridSize) * _gridSize,
                    0
                );

                if (wagonGrid == targetGrid) return true;
            }
            return false;
        }
     */


    //private void UpdateTrainPositions(Vector3 newLocomotivePos, bool direction)
    //    {
    //        // Tüm eski pozisyonları kaydet
    //        for (int i = 0; i < wagons.Length; i++)
    //        {
    //            _previousPositions[i] = wagons[i].position;
    //        }

    //        // Lokomotifi hareket ettir
    //        //wagons[0].position = newLocomotivePos;
    //        wagons[0].GetComponent<WagonMovement>().Move(newLocomotivePos, direction);

    //        // Diğer vagonları güncelle
    //        for (int i = 1; i < wagons.Length; i++)
    //        {
    //            //wagons[i].position = _previousPositions[i - 1];
    //            wagons[i].GetComponent<WagonMovement>().Move(_previousPositions[i - 1], direction);
    //        }
    //    }
    #endregion
}

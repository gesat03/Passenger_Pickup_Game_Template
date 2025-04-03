using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PPG
{
    [RequireComponent(typeof (WagonData))]
    public class WagonMovement : MonoBehaviour
    {
        //[SerializeField] private float _moveSpeed = 5.0f;
        [SerializeField] private float _turnAngle = 90.0f;
        [SerializeField] private float _turnSmoothing = 0.1f;
        [SerializeField] private float _moveSmoothing = 0.125f;

        private Vector3 _targetPosition;
        private Vector3 _smoothedPosition;

        private EWagonDirections _direction;
        private Quaternion _targetRotation;

        private bool _isMoving;
        private bool _isRotating;

        private GridMap _map;

        public void InitializeWagon(GridMap map, int mapPositionID, EWagonDirections direction, EItemColor color)
        {
            _map = map;
            _isMoving = false;
            _isRotating = false;
            _direction = direction;
            transform.localPosition = CellIDtoV3Pos(mapPositionID, true);
            transform.localRotation = WagonRotation(_direction, false);
            transform.GetComponent<WagonMaterialSetter>().SetMaterial(color);
        }

        private void LateUpdate()
        {
            HandleMovement();
            HandleRotation();
        }

        public void TouchMove(int mapPositionID, EWagonDirections direction)
        {
            _targetPosition = CellIDtoV3Pos(mapPositionID, false);
            _targetRotation = WagonRotation(direction, false);
            _isMoving = true;
            _isRotating = true;
        }

        public void FinalMove(Vector3 targetPos)
        {
            _targetPosition = targetPos;
            _targetRotation = WagonRotation(GetComponent<WagonData>().WagonDirection, true);
            _isMoving = true;
            _isRotating = true;
        }

        public void FinalMove(Vector3 targetPos, Quaternion targetRot)
        {
            _targetPosition = targetPos;
            _targetRotation = targetRot;
            _isMoving = true;
            _isRotating = true;
        }

        private void HandleMovement()
        {
            if (!_isMoving) return;

            // If the distance between the target position and the current position is less than 0.05f
            if (MapTranslationUtilities.CheckV3AndRound(transform.localPosition, _targetPosition, 0.05f))
            {
                _isMoving = false;
                transform.localPosition = _targetPosition;
                return;
            }

            // Smoothly move the wagon to the target position
            _smoothedPosition = Vector3.Lerp(transform.localPosition, _targetPosition, _moveSmoothing);
            transform.localPosition = _smoothedPosition;
        }


        private void HandleRotation()
        {
            if (!_isRotating) return;

            if (Quaternion.Angle(transform.localRotation, _targetRotation) < 1f) // Adjust the threshold as needed
            {
                _isRotating = false;
                transform.localRotation = _targetRotation;
                return;
            }

            transform.localRotation = Quaternion.Slerp(transform.localRotation, _targetRotation, _turnSmoothing);
        }

        private EWagonDirections GetOppositeDirection(EWagonDirections direction)
        {
            switch (direction)
            {
                case EWagonDirections.Forward:
                    return EWagonDirections.Backward;
                case EWagonDirections.Backward:
                    return EWagonDirections.Forward;
                case EWagonDirections.Right:
                    return EWagonDirections.Left;
                case EWagonDirections.Left:
                    return EWagonDirections.Right;
                default:
                    return EWagonDirections.Default;
            }
        }

        private Vector3 CellIDtoV3Pos(int mapPositionID, bool isInitial)
        {
            // Set the map position ID of the wagon
            transform.GetComponent<WagonData>().WagonPosID = mapPositionID;

            float localY = isInitial ? 0 : transform.localPosition.y;

            //Set the initial position of the wagon from the map position ID
            return new Vector3(
                _map.GetCellPositionWithID(mapPositionID).x,
                localY,
                _map.GetCellPositionWithID(mapPositionID).y);
        }

        private Quaternion WagonRotation(EWagonDirections direction, bool isEnded) // Set the rotation of the wagon
        {
            bool isHeadWagon = GetComponent<WagonData>().IsWagonLast;

            int xAxis = isEnded ? 90 : 0;

            direction = isHeadWagon ? GetOppositeDirection(direction) : direction;

            Quaternion targetRotation = transform.localRotation;

            GetComponent<WagonData>().WagonDirection = direction;

            switch (direction)
            {
                case EWagonDirections.Forward:
                    targetRotation = Quaternion.Euler(xAxis, 0, 0);
                    return targetRotation;
                case EWagonDirections.Backward:
                    targetRotation = Quaternion.Euler(xAxis, 180, 0);
                    return targetRotation;
                case EWagonDirections.Right:
                    targetRotation = Quaternion.Euler(xAxis, _turnAngle, 0);
                    return targetRotation;
                case EWagonDirections.Left:
                    targetRotation = Quaternion.Euler(xAxis, -_turnAngle, 0);
                    return targetRotation;
                case EWagonDirections.Default:
                    targetRotation = Quaternion.Euler(xAxis, 0, 0);
                    return targetRotation;
                default:
                    targetRotation = Quaternion.Euler(xAxis, 0, 0);
                    return targetRotation;
            }
        }

    }
}

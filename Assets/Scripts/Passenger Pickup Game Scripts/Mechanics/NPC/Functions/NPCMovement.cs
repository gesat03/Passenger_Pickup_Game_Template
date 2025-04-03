using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class NPCMovement : MonoBehaviour
    {
        public bool IsMoving = false;

        [SerializeField] private NPCStateController _nPCStateController;
        //[SerializeField] private float _moveSmoothing = 0.125f;

        internal Transform _targetTransform;
        internal Vector3 _targetMovePosition;
        private Vector3 _smoothedPosition;

        public void MoveToFrontLine(Vector3 targetPosition)
        {
            _nPCStateController.ChangeState(ENPCAnimStates.Idle);
            _targetMovePosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            _nPCStateController.ChangeState(ENPCAnimStates.Run);
        }

        public void JumpToWagon(Transform targetTransform)
        {
            _targetTransform = targetTransform;
            _nPCStateController.ChangeState(ENPCAnimStates.Jump);
        }

        public float duration = 0.25f; // Hedefe ulaşma süresi (saniye)
        private float progress = 0;

        internal void HandleJump(Vector3 additionVector)
        {
            if (!IsMoving) return;

            // If the distance between the target position and the current position is less than 0.05f
            if (MapTranslationUtilities.CheckV3AndRound(transform.position, _targetTransform.position + additionVector, 0.1f))
            {
                IsMoving = false;
                transform.position = _targetTransform.position + additionVector;
                //transform.SetParent(_targetTransform);
                progress = 0;
                return;
            }

            // Smoothly move the wagon to the target position
            progress += Time.deltaTime / duration;
            transform.position = Vector3.Lerp( transform.position, _targetTransform.position + additionVector, progress );

            //_smoothedPosition = Vector3.Lerp(transform.position, _targetTransform.position + additionVector, _moveSmoothing);
            //transform.position = _smoothedPosition;
        }

        internal void HandleMovement()
        {
            if (!IsMoving) return;

            // If the distance between the target position and the current position is less than 0.05f
            if (MapTranslationUtilities.CheckV3AndRound(transform.localPosition, _targetMovePosition, 0.1f))
            {
                IsMoving = false;
                Debug.Log("NPC Position: " + transform.localPosition + "Target Position: " + _targetMovePosition);
                transform.localPosition = _targetMovePosition;
                progress = 0;
                return;
            }

            // Smoothly move the wagon to the target position
            progress += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _targetMovePosition, progress);
        }

    }
}

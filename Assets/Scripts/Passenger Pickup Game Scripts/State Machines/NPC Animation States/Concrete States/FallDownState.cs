using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class FallDownState : IState
    {
        private NPCStateController _stateController;
        private NPCMovement _nPCMovement;

        private Vector3 targetPos;

        public FallDownState(NPCStateController stateController, NPCMovement nPCMovement)
        {
            _stateController = stateController;
            _nPCMovement = nPCMovement;
        }

        public void EnterState()
        {
            _nPCMovement.IsMoving = true;
            targetPos = Vector3.zero;
            _stateController._Animator.Play("FallDown");
        }

        public void UpdateState()
        {
            _nPCMovement.HandleJump(targetPos);

            if (!_nPCMovement.IsMoving)
            {
                ExitState();
            }
        }

        public void ExitState()
        {
            _stateController.ChangeState(ENPCAnimStates.Sit_Idle);
        }
    }
}

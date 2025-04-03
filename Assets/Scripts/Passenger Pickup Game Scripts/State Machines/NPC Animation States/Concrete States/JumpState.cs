using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace PPG
{
    public class JumpState : IState
    {
        private NPCStateController _stateController;
        private NPCMovement _nPCMovement;

        //private Tween _jumpTween;

        private Vector3 targetPos;

        private float _jumpHeight = 1.5f;

        public JumpState(NPCStateController stateController, NPCMovement nPCMovement)
        {
            _nPCMovement = nPCMovement;
            _stateController = stateController;
        }

        public void EnterState()
        {
            _nPCMovement.IsMoving = true;
            targetPos = Vector3.up * _jumpHeight;
            _stateController._Animator.Play("Jump");
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
            _stateController.ChangeState(ENPCAnimStates.FallDown);
        }

    }
}

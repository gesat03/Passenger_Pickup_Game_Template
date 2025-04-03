using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class RunState : IState
    {
        private NPCStateController _stateController;
        private NPCMovement _nPCMovement;

        private Vector3 targetPos;

        public RunState(NPCStateController stateController, NPCMovement nPCMovement)
        {
            _stateController = stateController;
            _nPCMovement = nPCMovement;
        }

        public void EnterState()
        {
            _nPCMovement.IsMoving = true;
            _stateController._Animator.Play("Run");
        }

        public void UpdateState()
        {
            _nPCMovement.HandleMovement();

            if (!_nPCMovement.IsMoving)
            {
                ExitState();
            }
        }

        public void ExitState()
        {
            _stateController._Animator.Play("Idle");
        }
    }
}

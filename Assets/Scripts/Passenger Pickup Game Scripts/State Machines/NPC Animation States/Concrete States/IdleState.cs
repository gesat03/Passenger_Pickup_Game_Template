using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class IdleState : IState
    {
        private NPCStateController _stateController;
        private NPCMovement _nPCMovement;


        public IdleState(NPCStateController stateController, NPCMovement nPCMovement)
        {
            _stateController = stateController;
            _nPCMovement = nPCMovement;
        }

        public void EnterState()
        {
            //_nPCMovement.IsMoving = false;
            _stateController._Animator.Play("Idle");
        }

        public void UpdateState()
        {
            
        }

        public void ExitState()
        {
            
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class NPCStateController : MonoBehaviour
    {
        [SerializeField] internal Animator _Animator;
        [SerializeField] internal NPCMovement _NPCMovement;

        private StateMachine _stateMachine;

        private ENPCAnimStates _currentAnimState;

        private IState _idleState;
        private IState _runState;
        private IState _jumpState;
        private IState _fallDownState;
        private IState _sit_IdleState;

        private void OnEnable()
        {
            Initialization();
        }

        private void Initialization()
        {
            _stateMachine = new StateMachine();

            _idleState = new IdleState(this, _NPCMovement);
            _runState = new RunState(this, _NPCMovement);
            _jumpState = new JumpState(this, _NPCMovement);
            _fallDownState = new FallDownState(this, _NPCMovement);
            _sit_IdleState = new Sit_IdleState(this, _NPCMovement);

            ChangeState(ENPCAnimStates.Idle);
        }

        private void Update()
        {
            _stateMachine.UpdateState();
        }

        public void ChangeState(ENPCAnimStates state)
        {
            if (state == _currentAnimState) return;

            _currentAnimState = state;

            switch (state)
            {
                case ENPCAnimStates.Idle:
                    _stateMachine.ChangeState(_idleState);
                    break;
                case ENPCAnimStates.Sit_Idle:
                    _stateMachine.ChangeState(_sit_IdleState);
                    break;
                case ENPCAnimStates.Run:
                    _stateMachine.ChangeState(_runState);
                    break;
                case ENPCAnimStates.Jump:
                    _stateMachine.ChangeState(_jumpState);
                    break;
                case ENPCAnimStates.FallDown:
                    _stateMachine.ChangeState(_fallDownState);
                    break;
                case ENPCAnimStates.Default:
                    _stateMachine.ChangeState(_idleState);
                    break;
                default:
                    break;
            }
        }
    }
}

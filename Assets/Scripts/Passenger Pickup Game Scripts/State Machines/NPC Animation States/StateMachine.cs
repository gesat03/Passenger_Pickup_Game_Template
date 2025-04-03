using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPG
{
    public class StateMachine
    {
        private IState _currentState;

        public void ChangeState(IState newState)
        {
            if (_currentState != null)
            {
                _currentState.ExitState();
            }

            _currentState = newState;
            _currentState.EnterState();
        }

        public void UpdateState()
        {
            if (_currentState != null)
            {
                _currentState.UpdateState();
            }
        }
    }
}

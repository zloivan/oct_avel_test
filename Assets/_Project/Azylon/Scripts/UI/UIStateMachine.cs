using System;
using System.Collections.Generic;

namespace Azylon.UI
{
    public class UIStateMachine
    {
        private readonly Dictionary<Type, IUIState> _statesByType;
        private IUIState _currentState;
        
        public UIStateMachine(IUIState[] states)
        {
            _statesByType = new Dictionary<Type, IUIState>();

            foreach (var state in states)
                _statesByType.Add(state.GetType(), state);
        }

        public void SwitchTo<T>() where T: IUIState
        {
            _currentState?.Exit();
            _currentState = _statesByType[typeof(T)];
            _currentState.Enter();
        }

        public void SwitchTo(Type stateType)
        {
            _currentState?.Exit();
            _currentState = _statesByType[stateType];
            _currentState.Enter();
        }
    }
}
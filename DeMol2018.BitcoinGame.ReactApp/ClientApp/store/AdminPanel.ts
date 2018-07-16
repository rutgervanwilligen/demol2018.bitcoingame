import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AdminPanelState {
    
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface StartNewRoundAction {
    type: 'START_NEW_ROUND',
    invokerId: string,
    lengthOfNewRoundInMinutes: number
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = StartNewRoundAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    startNewRound: (invokerId: string, lengthOfNewRoundInMinutes: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'START_NEW_ROUND',
            invokerId: invokerId,
            lengthOfNewRoundInMinutes: lengthOfNewRoundInMinutes
        });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: AdminPanelState = {
    
};

export const reducer: Reducer<AdminPanelState> = (state: AdminPanelState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            // const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};

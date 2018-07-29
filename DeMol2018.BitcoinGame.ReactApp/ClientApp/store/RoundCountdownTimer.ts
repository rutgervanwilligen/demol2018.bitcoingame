import {Action, Reducer} from 'redux';
import {ReceiveLoginResultAction, ReceiveNewGameStateAction, ReceiveNewRoundResultAction} from "./BitcoinGame";
import {AppThunkAction} from "./index";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface RoundCountdownTimerState {
    currentEndTime: Date;
    minutesLeft: number;
    secondsLeft: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
interface UpdateTimeLeftAction {
    type: 'UPDATE_TIME_LEFT';
    secondsLeft: number;
    minutesLeft: number;
}


// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = ReceiveLoginResultAction | UpdateTimeLeftAction | ReceiveNewGameStateAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    updateTimeLeft: (minutesLeft: number, secondsLeft: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'UPDATE_TIME_LEFT',
            secondsLeft: secondsLeft,
            minutesLeft: minutesLeft
        });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: RoundCountdownTimerState = {
    minutesLeft: 0,
    secondsLeft: 0,
    currentEndTime: new Date()
};

export const reducer: Reducer<RoundCountdownTimerState> = (state: RoundCountdownTimerState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    console.log("acctiiieee in de RoundCountdownTimerState");
    console.log(action);
    switch (action.type) {
        case 'RECEIVE_LOGIN_RESULT':
            if (action.currentRoundEndTime == null) {
                return state;
            }
            return {
                ...state,
                currentEndTime: new Date(action.currentRoundEndTime)
            };
        case 'RECEIVE_NEW_GAME_STATE':
            return {
                ...state,
                currentEndTime: new Date(action.currentRoundEndTime)
            };
        case 'UPDATE_TIME_LEFT':
            return {
                ...state,
                minutesLeft: action.minutesLeft,
                secondsLeft: action.secondsLeft
            };
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};
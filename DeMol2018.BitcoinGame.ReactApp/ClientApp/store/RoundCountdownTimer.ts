import {Action, Reducer} from 'redux';
import {ReceiveLoginResultAction, ReceiveNewGameStateAction} from "./BitcoinGame";
import {AppThunkAction} from "./index";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface RoundCountdownTimerState {
    playerGuid: string;
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

interface FetchNewGameStateAction {
    type: 'FETCH_NEW_GAME_STATE';
    playerGuid: string;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = ReceiveLoginResultAction | UpdateTimeLeftAction | ReceiveNewGameStateAction | FetchNewGameStateAction;

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
    },
    fetchNewGameState: (playerGuid: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'FETCH_NEW_GAME_STATE',
            playerGuid: playerGuid
        });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: RoundCountdownTimerState = {
    playerGuid: '',
    minutesLeft: 0,
    secondsLeft: 0,
    currentEndTime: new Date()
};

export const reducer: Reducer<RoundCountdownTimerState> = (state: RoundCountdownTimerState | undefined, incomingAction: Action) => {
    const action = incomingAction as KnownAction;

    if (state === undefined) {
        state = unloadedState;
    }

    switch (action.type) {
        case 'RECEIVE_LOGIN_RESULT':
            return {
                ...state,
                playerGuid: action.playerGuid,
                currentEndTime: new Date(action.currentRoundEndTime!)
            };
        case 'RECEIVE_NEW_GAME_STATE':
            return {
                ...state,
                currentEndTime: new Date(action.currentRoundEndTime!)
            };
        case 'UPDATE_TIME_LEFT':
            return {
                ...state,
                minutesLeft: action.minutesLeft,
                secondsLeft: action.secondsLeft
            };
        case 'FETCH_NEW_GAME_STATE':
            return state;
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state;
};
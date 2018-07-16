import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface BitcoinGameState {
    isLoggedIn: boolean;
    isAdmin: boolean;
    playerGuid: string;
    currentRoundNumber: number;
    currentRoundEndTime?: Date;
    currentBalance?: number;
    usersWalletAddress?: number;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface MakeTransactionAction {
    type: 'MAKE_TRANSACTION';
    amount: number;
    receiverAddress: number;
}

interface ReceiveLoginResultAction {
    type: 'RECEIVE_LOGIN_RESULT';
    loginSuccessful: boolean;
    isAdmin: boolean;
    playerGuid: string;
    usersWalletAddress?: number;
    usersCurrentBalance?: number;
}

interface ReceiveNewRoundResultAction {
    type: 'RECEIVE_NEW_ROUND_RESULT';
    newRoundNumber: number;
    newRoundEndTime: Date
}

interface ReceiveRoundEndTimeAction {
    type: 'RECEIVE_ROUND_END_TIME';
    endTime: Date;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = MakeTransactionAction | ReceiveRoundEndTimeAction | ReceiveNewRoundResultAction | ReceiveLoginResultAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    makeTransaction: (amount: number, receiverId: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'MAKE_TRANSACTION',
            amount: amount,
            receiverAddress: receiverId
        });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: BitcoinGameState = {
    isLoggedIn: false,
    isAdmin: false,
    playerGuid: '',
    currentRoundNumber: 0,
    currentRoundEndTime: undefined,
    currentBalance: undefined,
    usersWalletAddress: undefined
};

export const reducer: Reducer<BitcoinGameState> = (state: BitcoinGameState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'RECEIVE_LOGIN_RESULT':
            console.log("acctiiieee");
            console.log(action);
            return {
                ...state,
                isLoggedIn: action.loginSuccessful,
                isAdmin: action.isAdmin,
                playerGuid: action.loginSuccessful ? action.playerGuid : '',
                usersWalletAddress: action.usersWalletAddress,
                currentBalance: action.usersCurrentBalance
            };
        case 'MAKE_TRANSACTION':
            return {
                ...state,
                currentBalance: state.currentBalance === undefined ? 0 : state.currentBalance - action.amount
            };
        case 'RECEIVE_NEW_ROUND_RESULT':
            return {
                ...state,
                currentRoundNumber: action.newRoundNumber,
                currentRoundEndTime: action.newRoundEndTime
            };
        case 'RECEIVE_ROUND_END_TIME':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
//            if (action.startDateIndex === state.startDateIndex) {
            return {
                ...state,
                currentRoundEndTime: action.endTime
            };
//            }
//            break;
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};

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
    invokerId: string;
    receiverAddress: number;
    amount: number;
}

export interface ReceiveLoginResultAction {
    type: 'RECEIVE_LOGIN_RESULT';
    loginSuccessful: boolean;
    isAdmin: boolean;
    playerGuid: string;
    userWalletAddress?: number;
    userCurrentBalance?: number;
    currentRoundNumber: number;
    currentRoundEndTime?: string;
}

export interface ReceiveNewGameStateAction {
    type: 'RECEIVE_NEW_GAME_STATE';
    currentRoundNumber: number;
    currentRoundEndTime: string;
    userCurrentBalance: number;
}

interface ReceiveRoundEndTimeAction {
    type: 'RECEIVE_ROUND_END_TIME';
    endTime: Date;
}

interface ReceiveMakeTransactionResult {
    type: 'RECEIVE_MAKE_TRANSACTION_RESULT';
    transactionSuccessful: boolean;
    userCurrentBalance: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = MakeTransactionAction | ReceiveRoundEndTimeAction | ReceiveLoginResultAction | ReceiveMakeTransactionResult | ReceiveNewGameStateAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    makeTransaction: (invokerId: string, receiverAddress: number, amount: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'MAKE_TRANSACTION',
            invokerId: invokerId,
            receiverAddress: receiverAddress,
            amount: amount
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
    console.log("acctiiieee in de BitcoinGameState");
    console.log(action);
    switch (action.type) {
        case 'RECEIVE_NEW_GAME_STATE':
            return {
                ...state,
                currentBalance: action.userCurrentBalance,
                currentRoundNumber: action.currentRoundNumber,
                currentRoundEndTime: new Date(action.currentRoundEndTime)
            };
        case 'RECEIVE_LOGIN_RESULT':
            return {
                ...state,
                isLoggedIn: action.loginSuccessful,
                isAdmin: action.isAdmin,
                playerGuid: action.loginSuccessful ? action.playerGuid : '',
                usersWalletAddress: action.userWalletAddress,
                currentBalance: action.userCurrentBalance,
                currentRoundEndTime: action.currentRoundEndTime != null ? new Date(action.currentRoundEndTime) : undefined,
                currentRoundNumber: action.currentRoundNumber
            };
        case 'MAKE_TRANSACTION':
            return {
                ...state,
                currentBalance: state.currentBalance === undefined ? 0 : state.currentBalance - action.amount
            };
        case 'RECEIVE_MAKE_TRANSACTION_RESULT':
            return {
                ...state,
                currentBalance: action.userCurrentBalance
            };
        case 'RECEIVE_ROUND_END_TIME':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
//            if (action.startDateIndex === state.startDateIndex) {
            return {
                ...state,
                currentRoundEndTime: action.endTime != null ? new Date(action.endTime) : undefined,
            };
//            }
//            break;
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};

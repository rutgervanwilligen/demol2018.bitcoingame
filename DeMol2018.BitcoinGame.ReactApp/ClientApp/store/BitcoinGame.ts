import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface NonPlayerWalletState {
    address: number;
    currentBalance: number;
    name: number;
}

export interface BitcoinGameState {
    isLoggedIn: boolean;
    isAdmin: boolean;
    playerGuid?: string;
    currentGameId?: string;
    currentRoundNumber?: number;
    currentRoundEndTime?: Date;
    currentBalance?: number;
    usersWalletAddress?: number;
    nonPlayerWallets: NonPlayerWalletState[];
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
    currentGameId?: string;
    userWalletAddress?: number;
    userCurrentBalance?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    nonPlayerWallets?: NonPlayerWalletState[];
}

export interface ReceiveNewGameStateAction {
    type: 'RECEIVE_NEW_GAME_STATE';
    currentGameId?: string;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    userCurrentBalance?: number;
    userWalletAddress?: number;
    nonPlayerWallets?: NonPlayerWalletState[];
}

interface ReceiveMakeTransactionResult {
    type: 'RECEIVE_MAKE_TRANSACTION_RESULT';
    transactionSuccessful: boolean;
    userCurrentBalance: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = MakeTransactionAction | ReceiveLoginResultAction | ReceiveMakeTransactionResult | ReceiveNewGameStateAction;

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
    playerGuid: undefined,
    currentGameId: undefined,
    currentRoundNumber: undefined,
    currentRoundEndTime: undefined,
    currentBalance: undefined,
    usersWalletAddress: undefined,
    nonPlayerWallets: []
};

export const reducer: Reducer<BitcoinGameState> = (state: BitcoinGameState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;

    switch (action.type) {
        case 'RECEIVE_NEW_GAME_STATE':
            return {
                ...state,
                currentGameId: action.currentGameId,
                currentBalance: action.userCurrentBalance,
                usersWalletAddress: action.userWalletAddress,
                currentRoundNumber: action.currentRoundNumber != null ? action.currentRoundNumber : undefined,
                currentRoundEndTime: action.currentRoundEndTime != null ? new Date(action.currentRoundEndTime) : undefined,
                nonPlayerWallets: action.nonPlayerWallets != null ? action.nonPlayerWallets : []
            };
        case 'RECEIVE_LOGIN_RESULT':
            return {
                ...state,
                isLoggedIn: action.loginSuccessful,
                isAdmin: action.isAdmin,
                playerGuid: action.loginSuccessful ? action.playerGuid : '',
                usersWalletAddress: action.userWalletAddress,
                currentGameId: action.currentGameId,
                currentBalance: action.userCurrentBalance,
                currentRoundEndTime: action.currentRoundEndTime != null ? new Date(action.currentRoundEndTime) : undefined,
                currentRoundNumber: action.currentRoundNumber != null ? action.currentRoundNumber : undefined,
                nonPlayerWallets: action.nonPlayerWallets != null ? action.nonPlayerWallets : []
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
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};

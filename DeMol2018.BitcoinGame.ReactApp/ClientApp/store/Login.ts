import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LoginState {
    loggedIn: boolean,
    playerGuid: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface LoginAction {
    type: 'LOGIN',
    name: string,
    code: string;
}

interface ReceiveLoginAction {
    type: 'RECEIVE_LOGIN_RESULT',
    loginSuccessful: boolean,
    playerGuid: string,
    usersWalletAddress?: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = ReceiveLoginAction | LoginAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    login: (name: string, code: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'LOGIN',
            name: name,
            code: code
        });
    },
    receiveLoginResult: (loginSuccessful: boolean, playerGuid: string, usersWalletAddress?: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({
            type: 'RECEIVE_LOGIN_RESULT',
            loginSuccessful: loginSuccessful,
            playerGuid: playerGuid,
            usersWalletAddress: usersWalletAddress
        });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: LoginState = {
    loggedIn: false,
    playerGuid: ""
};

export const reducer: Reducer<LoginState> = (state: LoginState | undefined, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'RECEIVE_LOGIN_RESULT':
            return {
                loggedIn: action.loginSuccessful,
                playerGuid: action.loginSuccessful ? action.playerGuid : "",
                usersWalletAddress: action.loginSuccessful ? action.usersWalletAddress : undefined
            };
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            // const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};

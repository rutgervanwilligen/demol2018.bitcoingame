import * as signalR from "@aspnet/signalr";

import { ApplicationState } from "./index";
import { Store } from "redux";

// Declare connection
let connection = new signalR.HubConnection("http://localhost:63426/bitcoinGameHub");
//let connection = new signalR.HubConnection("https://bitcoin.demol2018.nl/bitcoinGameHub");

export function signalRInvokeMiddleware() {
    return (next: any) => async (action: any) => {

        switch (action.type) {
            case "MAKE_TRANSACTION":
                connection.invoke("MakeTransaction", action.invokerId, action.receiverAddress, action.amount).then(function () {
                    console.log("make transaction fulfilled");
                }).catch(function () {
                    console.log("make transaction rejected");
                });
                break;

            case "LOGIN":
                connection.invoke("Login", action.name, action.code).then(function () {
                    console.log("login fulfilled");
                }).catch(function () {
                    console.log("login rejected");
                });
                break;

            case "FETCH_NEW_GAME_STATE":
                connection.invoke("FetchNewGameState", action.playerGuid).then(function () {
                    console.log("fetch new game state fulfilled");
                }).catch(function () {
                    console.log("fetch new game state rejected");
                });
                break;
                
            case "START_NEW_ROUND":
                connection.invoke("StartNewRound", action.invokerId, action.lengthOfNewRoundInMinutes).then(function () {
                    
                }).catch(function () {
                    
                });
                break;
            default:
                console.log("Unknown action (" + action.type + "). SignalR hub is not invoked.");
                break;
        }

        return next(action);
    }
}

export function signalRRegisterCommands(store: Store<ApplicationState>) {

    connection.on('LoginResult', loginResult => {
        store.dispatch({
            type: 'RECEIVE_LOGIN_RESULT',
            loginSuccessful: loginResult.loginSuccessful,
            playerGuid: loginResult.playerGuid,
            isAdmin: loginResult.isAdmin,
            userWalletAddress: loginResult.updatedState.userWalletAddress,
            userCurrentBalance: loginResult.updatedState.userCurrentBalance,
            currentRoundNumber: loginResult.updatedState.currentRoundNumber,
            currentRoundEndTime: loginResult.updatedState.currentRoundEndTime
        });
    });

    connection.on('AnnounceNewRoundResult', () => {
        store.dispatch({
            type: 'FETCH_NEW_GAME_STATE',
            playerGuid: store.getState().bitcoinGame.playerGuid
        });
    });

    connection.on('FetchNewGameStateResult', fetchNewGameStateResult => {
        store.dispatch({
            type: 'RECEIVE_NEW_GAME_STATE',
            currentRoundNumber: fetchNewGameStateResult.updatedState.currentRoundNumber,
            currentRoundEndTime: fetchNewGameStateResult.updatedState.currentRoundEndTime,
            userCurrentBalance: fetchNewGameStateResult.updatedState.userCurrentBalance
        });
    });

    connection.on('StartNewRoundResult', data => {
        store.dispatch({
            type: 'RECEIVE_NEW_ROUND_RESULT',
            newRoundNumber: data.newRoundNumber,
            newRoundEndTime: data.newRoundEndTime
        });
    });

    connection.on('MakeTransactionResult', makeTransactionResult => {
        store.dispatch({
            type: 'RECEIVE_MAKE_TRANSACTION_RESULT',
            transactionSuccessful: makeTransactionResult.transactionSuccessful,
            userCurrentBalance: makeTransactionResult.userCurrentBalance
        });
    });

    connection.start();
}

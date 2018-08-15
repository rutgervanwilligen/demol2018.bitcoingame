import * as signalR from "@aspnet/signalr";

import { ApplicationState } from "./index";
import { Store } from "redux";
import { NonPlayerWalletState } from "./BitcoinGame";

// Declare connection
let connection = new signalR.HubConnection("http://localhost:63426/bitcoinGameHub");
//let connection = new signalR.HubConnection("https://bitcoin.demol2018.nl/bitcoinGameHub");

export function signalRInvokeMiddleware() {
    return (next: any) => async (action: any) => {

        let ignoredActionTypes = ['UPDATE_TIME_LEFT'];

        switch (action.type) {
            case "MAKE_TRANSACTION":
                connection.invoke("MakeTransaction", action.invokerId, action.receiverAddress, action.amount).then(function () {
                }).catch(function () {
                    console.log("make transaction rejected");
                });
                break;

            case "LOGIN":
                connection.invoke("Login", action.name, action.code).then(function () {
                }).catch(function () {
                    console.log("login rejected");
                });
                break;

            case "FETCH_NEW_GAME_STATE":
                connection.invoke("FetchNewGameState", action.playerGuid).then(function () {
                }).catch(function () {
                    console.log("fetch new game state rejected");
                });
                break;
                
            case "START_NEW_ROUND":
                connection.invoke("StartNewRound", action.invokerId, action.lengthOfNewRoundInMinutes).then(function () {
                }).catch(function () {
                    console.log("start new round rejected");
                });
                break;

            case "START_NEW_GAME":
                connection.invoke("StartNewGame", action.invokerId).then(function () {
                }).catch(function () {
                    console.log("start new game rejected");
                });
                break;

            case "FINISH_CURRENT_GAME":
                connection.invoke("FinishCurrentGame", action.invokerId).then(function () {
                }).catch(function () {
                    console.log("finish current game rejected");
                });
                break;

            default:
                if (!action.type.startsWith('RECEIVE') && ignoredActionTypes.indexOf(action.type) == -1 ) {
                    console.log("Unknown action (" + action.type + "). SignalR hub is not invoked.");
                }

                break;
        }

        return next(action);
    }
}

export function signalRRegisterCommands(store: Store<ApplicationState>) {

    function sortWallets(wallets: NonPlayerWalletState[]): NonPlayerWalletState[] {
        return wallets != null
            ? wallets.sort((a: NonPlayerWalletState, b: NonPlayerWalletState) => {
                return a.address - b.address
            })
            : [];
    }

    connection.on('LoginResult', loginResult => {
        let sortedWallets = sortWallets(loginResult.updatedState.nonPlayerWallets);

        store.dispatch({
            type: 'RECEIVE_LOGIN_RESULT',
            loginSuccessful: loginResult.loginSuccessful,
            playerGuid: loginResult.playerGuid,
            isAdmin: loginResult.isAdmin,
            userWalletAddress: loginResult.updatedState.userWalletAddress,
            userCurrentBalance: loginResult.updatedState.userCurrentBalance,
            currentGameId: loginResult.updatedState.currentGameId,
            lastRoundNumber: loginResult.updatedState.lastRoundNumber,
            currentRoundNumber: loginResult.updatedState.currentRoundNumber,
            currentRoundEndTime: loginResult.updatedState.currentRoundEndTime,
            nonPlayerWallets: sortedWallets,
            moneyWonSoFar: loginResult.updatedState.moneyWonSoFar,
            gameHasFinished: loginResult.updatedState.gameHasFinished,
            numberOfJokersWon: loginResult.updatedState.numberOfJokersWon
        });
    });

    connection.on('AnnounceNewGameStateResult', () => {
        store.dispatch({
            type: 'FETCH_NEW_GAME_STATE',
            playerGuid: store.getState().bitcoinGame.playerGuid
        });
    });

    connection.on('FetchNewGameStateResult', fetchNewGameStateResult => {
        let sortedWallets = sortWallets(fetchNewGameStateResult.updatedState.nonPlayerWallets);

        store.dispatch({
            type: 'RECEIVE_NEW_GAME_STATE',
            currentGameId: fetchNewGameStateResult.updatedState.currentGameId,
            lastRoundNumber: fetchNewGameStateResult.updatedState.lastRoundNumber,
            currentRoundNumber: fetchNewGameStateResult.updatedState.currentRoundNumber,
            currentRoundEndTime: fetchNewGameStateResult.updatedState.currentRoundEndTime,
            userWalletAddress: fetchNewGameStateResult.updatedState.userWalletAddress,
            userCurrentBalance: fetchNewGameStateResult.updatedState.userCurrentBalance,
            nonPlayerWallets: sortedWallets,
            moneyWonSoFar: fetchNewGameStateResult.updatedState.moneyWonSoFar,
            gameHasFinished: fetchNewGameStateResult.updatedState.gameHasFinished,
            numberOfJokersWon: fetchNewGameStateResult.updatedState.numberOfJokersWon
        });
    });

    connection.on('StartNewRoundResult', data => {
        store.dispatch({
            type: 'RECEIVE_NEW_ROUND_RESULT',
            callSuccessful: data.callSuccessful
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

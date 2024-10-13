import * as signalR from "@microsoft/signalr";

import { ApplicationState } from "./index";
import { Store } from "redux";
import {JokerWinner, NonPlayerWalletState} from "./BitcoinGame";

// Declare connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5000/bitcoinGameHub")
//    .withUrl("https://bitcoingame.rutgervanwilligen.nl/bitcoinGameHub")
    .build();

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
                console.log("ik ga login invoken met name = " + action.name + " en code is " + action.code);
                let name = action.name;
                let code = Number(action.code);
                connection.invoke("Login", name, code).then(function () {
                }).catch(error => function () {
                    console.log("login rejected");
                    console.log(error);
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

export async function signalRRegisterCommands(store: Store<ApplicationState>) {

    function sortWallets(wallets: NonPlayerWalletState[]): NonPlayerWalletState[] {
        return wallets != null
            ? wallets.sort((a: NonPlayerWalletState, b: NonPlayerWalletState) => {
                return a.address - b.address
            })
            : [];
    }

    function sortJokerWinners(wallets: JokerWinner[]): JokerWinner[] {
        return wallets != null
            ? wallets.sort((a: JokerWinner, b: JokerWinner) => {
                let nameA = a.name.toLowerCase();
                let nameB = b.name.toLowerCase();

                if (nameA < nameB) {
                    return -1;
                }

                if (nameB < nameA) {
                    return 1;
                }

                return 0;
            })
            : [];
    }

    connection.on('LoginResult', loginResult => {
        let sortedWallets = sortWallets(loginResult.updatedState.nonPlayerWallets);
        let sortedJokerWinners = sortJokerWinners(loginResult.updatedState.jokerWinners);

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
            numberOfJokersWon: loginResult.updatedState.numberOfJokersWon,
            jokerWinners: sortedJokerWinners
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
        let sortedJokerWinners = sortJokerWinners(fetchNewGameStateResult.updatedState.jokerWinners);

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
            numberOfJokersWon: fetchNewGameStateResult.updatedState.numberOfJokersWon,
            jokerWinners: sortedJokerWinners
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

    await connection.start().catch(error => console.log("Error! " + error));
    console.log("connected");
}

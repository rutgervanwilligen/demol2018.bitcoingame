﻿import * as signalR from "@microsoft/signalr";
import {HubConnection, HubConnectionState} from "@microsoft/signalr";
import {Middleware, MiddlewareAPI, PayloadAction} from "@reduxjs/toolkit";
import {sortJokerWinners, sortWallets} from "./Utils";
import {
    fetchNewGameState,
    makeTransaction,
    receiveMakeTransactionResult,
    receiveNewGameState
} from "./bitcoinGame/bitcoinGameSlice";
import {login, receiveLoginResult} from "./user/userSlice";
import {finishCurrentGame, startNewGame, startNewRound} from "./adminPanel/adminPanelSlice";
import {connectWebsocket, updateConnectionStatus} from "./websocketConnection/websocketConnectionSlice";
import {AppDispatch, RootState} from "../configureStore";

export const websocketMiddleware: Middleware = store => {
    const { dispatch } = store;

    let connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5000/bitcoinGameHub")
        //    .withUrl("https://bitcoingame.rutgervanwilligen.nl/bitcoinGameHub")
        .withAutomaticReconnect()
        .build();

    return next => async (action: PayloadAction) => {
        console.log("Action binnengekomen! " + action.type);
        await registerWebsocketConnection(connection, store, action);
        invokeWebsocketIfNecessary(connection, dispatch, action);

        next(action);
    };
}

const registerWebsocketConnection = async (connection: HubConnection, store: MiddlewareAPI<AppDispatch, RootState>, action: PayloadAction) => {
    const { dispatch } = store;

    if (connectWebsocket.match(action)) {
        if (connection.state == HubConnectionState.Connected) {
            return;
        }

        connection.start()
            .then(() => {
                dispatch(updateConnectionStatus({ isConnected: true }));
            })
            .catch(error => {
                console.log("Error while connecting websocket: " + error);
                dispatch(updateConnectionStatus({ isConnected: false }));
            });

        connection.onreconnecting(() => {
            dispatch(updateConnectionStatus({ isConnected: false }));
        })

        connection.onreconnected(() => {
            dispatch(updateConnectionStatus({ isConnected: true }));
        })

        connection.onclose(() => {
            dispatch(updateConnectionStatus({ isConnected: false }));
        });

        await registerIncomingWebsocketMessages(connection, store);
    }
}

const invokeWebsocketIfNecessary = (connection: HubConnection, dispatch: AppDispatch, action: PayloadAction) => {
    let ignoredActionTypes = ['UPDATE_TIME_LEFT'];

    if (makeTransaction.match(action)) {
        const { invokerId, receiverAddress, amount } = action.payload;
        connection.invoke("MakeTransaction", invokerId, receiverAddress, amount).then(function () {
        }).catch(function () {
            console.log("make transaction rejected");
        });
        return;
    }

    if (login.match(action)) {
        const { name, code } = action.payload;
        console.log("ik ga login invoken met name = " + name + " en code = " + code);
        connection.invoke("Login", name, code).then(function () {
        }).catch(error => function () {
            console.log("login rejected");
            console.log(error);
        });
        return;
    }

    if (fetchNewGameState.match(action)) {
        const { playerGuid } = action.payload;
        connection.invoke("FetchNewGameState", playerGuid).then(function () {
        }).catch(function () {
            console.log("fetch new game state rejected");
        });
        return;
    }

    if (startNewRound.match(action)) {
        const { invokerId, lengthOfNewRoundInMinutes } = action.payload;
        connection.invoke("StartNewRound", invokerId, lengthOfNewRoundInMinutes).then(function () {
        }).catch(function () {
            console.log("start new round rejected");
        });
        return;
    }

    if (startNewGame.match(action)) {
        const { invokerId } = action.payload;
        connection.invoke("StartNewGame", invokerId).then(function () {
        }).catch(function () {
            console.log("start new game rejected");
        });
        return;
    }

    if (finishCurrentGame.match(action)) {
        const { invokerId } = action.payload;
        connection.invoke("FinishCurrentGame", invokerId).then(function () {
        }).catch(function () {
            console.log("finish current game rejected");
        });
        return;
    }
}

const registerIncomingWebsocketMessages = async (connection: HubConnection, store: MiddlewareAPI<AppDispatch, RootState>) => {
    const { dispatch } = store;

    connection.on('LoginResult', loginResult => {
        let sortedWallets = sortWallets(loginResult.updatedState.nonPlayerWallets);
        let sortedJokerWinners = sortJokerWinners(loginResult.updatedState.jokerWinners);

        dispatch(receiveLoginResult({
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
        }));
    });

    connection.on('AnnounceNewGameStateResult', () => {
        dispatch(fetchNewGameState({
            playerGuid: store.getState().user.playerGuid
        }));
    });

    connection.on('FetchNewGameStateResult', fetchNewGameStateResult => {
        let sortedWallets = sortWallets(fetchNewGameStateResult.updatedState.nonPlayerWallets);
        let sortedJokerWinners = sortJokerWinners(fetchNewGameStateResult.updatedState.jokerWinners);

        dispatch(receiveNewGameState({
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
        }));
    });

    // connection.on('StartNewRoundResult', data => {
    //     dispatch(receiveNewR{
    //         type: 'RECEIVE_NEW_ROUND_RESULT',
    //         callSuccessful: data.callSuccessful
    //     });
    // });

    connection.on('MakeTransactionResult', makeTransactionResult => {
        dispatch(receiveMakeTransactionResult({
            transactionSuccessful: makeTransactionResult.transactionSuccessful,
            userCurrentBalance: makeTransactionResult.userCurrentBalance
        }));
    });
}

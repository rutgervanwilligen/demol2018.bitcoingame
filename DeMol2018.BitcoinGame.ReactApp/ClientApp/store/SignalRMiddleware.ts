import * as signalR from "@microsoft/signalr";
import { HubConnection, HubConnectionState } from "@microsoft/signalr";
import { Middleware, MiddlewareAPI, PayloadAction } from "@reduxjs/toolkit";
import { sortJokerWinners, sortWallets } from "./Utils";
import {
    fetchNewGameState,
    makeTransaction,
    updateCurrentBalance,
    updateGameState,
} from "./bitcoinGame/bitcoinGameSlice";
import { login, updateLoginStatus } from "./user/userSlice";
import {
    finishCurrentGame,
    startNewGame,
    startNewRound,
} from "./adminPanel/adminPanelSlice";
import {
    connectWebsocket,
    updateConnectionStatus,
    WebsocketConnectionStatus,
} from "./websocketConnection/websocketConnectionSlice";
import { AppDispatch, RootState } from "../configureStore";
import {
    FetchNewGameStateResult,
    FetchNewGameStateResultHubMethod,
    MapJokerWinnerToDomain,
    MapNonPlayerWalletToDomain,
} from "../infrastructure/responses/FetchNewGameStateResult";
import {
    LoginResult,
    LoginResultHubMethod,
} from "../infrastructure/responses/LoginResult";
import {
    MakeTransactionResult,
    MakeTransactionResultHubMethod,
} from "../infrastructure/responses/MakeTransactionResult";
import { AnnounceNewGameStateResultHubMethod } from "../infrastructure/responses/AnnounceNewGameStateResult";

export const websocketMiddleware: Middleware = (store: MiddlewareAPI) => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(`${process.env.HUB_SERVER_BASE_URL}/bitcoinGameHub`)
        .withAutomaticReconnect()
        .build();

    registerIncomingWebsocketMessages(connection, store);

    return (next) => (action) => {
        const typedAction = action as PayloadAction;
        const { dispatch } = store;

        console.log("Action binnengekomen! " + typedAction.type);
        invokeWebsocketIfNecessary(connection, typedAction, dispatch);

        next(action);
    };
};

const connectWebsocketAndRegisterUpdates = (
    connection: HubConnection,
    dispatch: AppDispatch,
) => {
    if (connection.state == HubConnectionState.Connected) {
        return;
    }

    connection
        .start()
        .then(() => {
            dispatch(
                updateConnectionStatus({
                    newConnectionStatus: WebsocketConnectionStatus.Connected,
                }),
            );
        })
        .catch((error) => {
            console.log("Error while connecting websocket: " + error);
            dispatch(
                updateConnectionStatus({
                    newConnectionStatus:
                        WebsocketConnectionStatus.ConnectionError,
                }),
            );
        });

    connection.onreconnecting(() => {
        dispatch(
            updateConnectionStatus({
                newConnectionStatus: WebsocketConnectionStatus.Reconnecting,
            }),
        );
    });

    connection.onreconnected(() => {
        dispatch(
            updateConnectionStatus({
                newConnectionStatus: WebsocketConnectionStatus.Connected,
            }),
        );
    });

    connection.onclose(() => {
        dispatch(
            updateConnectionStatus({
                newConnectionStatus: WebsocketConnectionStatus.Disconnected,
            }),
        );
    });
};

const invokeWebsocketIfNecessary = (
    connection: HubConnection,
    action: PayloadAction,
    dispatch: AppDispatch,
) => {
    if (connectWebsocket.match(action)) {
        connectWebsocketAndRegisterUpdates(connection, dispatch);
        return;
    }

    if (makeTransaction.match(action)) {
        const { invokerId, receiverAddress, amount } = action.payload;
        connection
            .invoke("MakeTransaction", invokerId, receiverAddress, amount)
            .then(function () {})
            .catch(function () {
                console.log("make transaction rejected");
            });
        return;
    }

    if (login.match(action)) {
        const { name, code } = action.payload;
        console.log(
            "ik ga login invoken met name = " + name + " en code = " + code,
        );
        connection
            .invoke("Login", name, code)
            .then(function () {})
            .catch(
                (error) =>
                    function () {
                        console.log("login rejected");
                        console.log(error);
                    },
            );
        return;
    }

    if (fetchNewGameState.match(action)) {
        const { playerGuid } = action.payload;
        connection
            .invoke("FetchNewGameState", playerGuid)
            .then(function () {})
            .catch(function () {
                console.log("fetch new game state rejected");
            });
        return;
    }

    if (startNewRound.match(action)) {
        const { invokerId, lengthOfNewRoundInMinutes } = action.payload;
        connection
            .invoke("StartNewRound", invokerId, lengthOfNewRoundInMinutes)
            .then(function () {})
            .catch(function () {
                console.log("start new round rejected");
            });
        return;
    }

    if (startNewGame.match(action)) {
        const { invokerId } = action.payload;
        connection
            .invoke("StartNewGame", invokerId)
            .then(function () {})
            .catch(function () {
                console.log("start new game rejected");
            });
        return;
    }

    if (finishCurrentGame.match(action)) {
        const { invokerId } = action.payload;
        connection
            .invoke("FinishCurrentGame", invokerId)
            .then(function () {})
            .catch(function () {
                console.log("finish current game rejected");
            });
        return;
    }
};

const registerIncomingWebsocketMessages = (
    connection: HubConnection,
    store: MiddlewareAPI<AppDispatch, RootState>,
) => {
    const { dispatch } = store;

    connection.on(LoginResultHubMethod, (loginResult: LoginResult) => {
        dispatch(
            updateLoginStatus({
                loginSuccessful: loginResult.loginSuccessful,
                playerGuid: loginResult.playerGuid,
                isAdmin: loginResult.isAdmin,
            }),
        );

        if (loginResult.loginSuccessful && loginResult.playerGuid) {
            dispatch(
                fetchNewGameState({
                    playerGuid: loginResult.playerGuid!,
                }),
            );
        }
    });

    connection.on(AnnounceNewGameStateResultHubMethod, () => {
        dispatch(
            fetchNewGameState({
                playerGuid: store.getState().user.playerGuid!,
            }),
        );
    });

    connection.on(
        FetchNewGameStateResultHubMethod,
        (result: FetchNewGameStateResult) => {
            const sortedNonPlayerWallets = sortWallets(
                result.updatedState.nonPlayerWallets.map(
                    MapNonPlayerWalletToDomain,
                ),
            );
            const sortedJokerWinners = sortJokerWinners(
                result.updatedState.jokerWinners.map(MapJokerWinnerToDomain),
            );

            dispatch(
                updateGameState({
                    currentGameId: result.updatedState.currentGameId,
                    lastRoundNumber: result.updatedState.lastRoundNumber,
                    currentRoundNumber: result.updatedState.currentRoundNumber,
                    currentRoundEndTime:
                        result.updatedState.currentRoundEndTime,
                    userCurrentBalance: result.updatedState.userCurrentBalance,
                    userWalletAddress: result.updatedState.userWalletAddress,
                    nonPlayerWallets: sortedNonPlayerWallets,
                    moneyWonSoFar: result.updatedState.moneyWonSoFar,
                    gameHasFinished: result.updatedState.gameHasFinished,
                    numberOfJokersWon: result.updatedState.numberOfJokersWon,
                    jokerWinners: sortedJokerWinners,
                }),
            );
        },
    );

    connection.on(
        MakeTransactionResultHubMethod,
        (makeTransactionResult: MakeTransactionResult) => {
            if (!makeTransactionResult.transactionSuccessful) {
                console.log("Transaction niet gelukt");
                return;
            }

            dispatch(
                updateCurrentBalance({
                    newCurrentBalance: makeTransactionResult.userCurrentBalance,
                }),
            );
        },
    );
};

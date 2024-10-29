import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";

export interface NonPlayerWallet {
    address: number;
    currentBalance: number;
    name: string;
}

export interface JokerWinner {
    name: string;
    numberOfJokersWon: number;
}

interface BitcoinGameState {
    currentGameId?: string;
    lastRoundNumber?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    currentBalance?: number;
    userWalletAddress?: number;
    nonPlayerWallets: NonPlayerWallet[];
    moneyWonSoFar: number;
    gameHasFinished?: boolean;
    numberOfJokersWon?: number;
}

interface MakeTransactionAction {
    invokerId: string;
    receiverAddress: number;
    amount: number;
}

interface FetchNewGameStateAction {
    playerGuid: string;
}

export interface UpdateGameStateAction {
    currentGameId?: string;
    lastRoundNumber?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    userCurrentBalance?: number;
    userWalletAddress?: number;
    nonPlayerWallets?: NonPlayerWallet[];
    moneyWonSoFar: number;
    gameHasFinished?: boolean;
    numberOfJokersWon?: number;
    jokerWinners?: JokerWinner[];
}

interface UpdateCurrentBalance {
    newCurrentBalance: number;
}

const initialState: BitcoinGameState = {
    currentGameId: undefined,
    currentRoundNumber: undefined,
    currentRoundEndTime: undefined,
    currentBalance: undefined,
    userWalletAddress: undefined,
    nonPlayerWallets: [],
    moneyWonSoFar: 0,
    lastRoundNumber: undefined,
    gameHasFinished: undefined,
    numberOfJokersWon: undefined,
};

/* eslint-disable @typescript-eslint/no-unused-vars */
export const bitcoinGameSlice = createSlice({
    name: "bitcoinGame",
    initialState,
    reducers: {
        fetchNewGameState: (
            state: BitcoinGameState,
            action: PayloadAction<FetchNewGameStateAction>,
        ) => {
            // No-op; caught in websocketMiddleware
        },
        updateGameState: (
            state: BitcoinGameState,
            action: PayloadAction<UpdateGameStateAction>,
        ) => {
            state.currentGameId = action.payload.currentGameId;
            state.gameHasFinished = action.payload.gameHasFinished;
            state.currentBalance = action.payload.userCurrentBalance;
            state.userWalletAddress = action.payload.userWalletAddress;
            state.lastRoundNumber = action.payload.lastRoundNumber;
            state.currentRoundNumber = action.payload.currentRoundNumber;
            state.currentRoundEndTime = action.payload.currentRoundEndTime;
            state.nonPlayerWallets =
                action.payload.nonPlayerWallets != undefined
                    ? action.payload.nonPlayerWallets
                    : [];
            state.moneyWonSoFar = action.payload.moneyWonSoFar;
            state.numberOfJokersWon = action.payload.numberOfJokersWon;
        },
        makeTransaction: (
            state,
            action: PayloadAction<MakeTransactionAction>,
        ) => {
            if (state.currentBalance === undefined) {
                return;
            }

            if (
                state.currentBalance >= action.payload.amount &&
                action.payload.amount > 0
            ) {
                state.currentBalance -= action.payload.amount;
            }
        },
        updateCurrentBalance: (
            state,
            action: PayloadAction<UpdateCurrentBalance>,
        ) => {
            state.currentBalance = action.payload.newCurrentBalance;
        },
    },
    selectors: {
        selectCurrentRoundNumber: (sliceState: BitcoinGameState) =>
            sliceState.currentRoundNumber,
        selectCurrentGameId: (sliceState: BitcoinGameState) =>
            sliceState.currentGameId,
        selectGameHasFinished: (sliceState: BitcoinGameState) =>
            sliceState.gameHasFinished,
        selectNonPlayerWallets: (sliceState: BitcoinGameState) =>
            sliceState.nonPlayerWallets,
        selectLastRoundNumber: (sliceState: BitcoinGameState) =>
            sliceState.lastRoundNumber,
        selectCurrentRoundEndTime: (sliceState: BitcoinGameState) =>
            sliceState.currentRoundEndTime,
        selectCurrentBalance: (sliceState: BitcoinGameState) =>
            sliceState.currentBalance,
        selectMoneyWonSoFar: (sliceState: BitcoinGameState) =>
            sliceState.moneyWonSoFar,
        selectUsersWalletAddress: (sliceState: BitcoinGameState) =>
            sliceState.userWalletAddress,
        selectNumberOfJokersWon: (sliceState: BitcoinGameState) =>
            sliceState.numberOfJokersWon,
    },
});
/* eslint-enable @typescript-eslint/no-unused-vars */

export const {
    fetchNewGameState,
    updateGameState,
    updateCurrentBalance,
    makeTransaction,
} = bitcoinGameSlice.actions;

export const {
    selectCurrentRoundNumber,
    selectCurrentGameId,
    selectGameHasFinished,
    selectNonPlayerWallets,
    selectLastRoundNumber,
    selectCurrentRoundEndTime,
    selectCurrentBalance,
    selectUsersWalletAddress,
    selectMoneyWonSoFar,
    selectNumberOfJokersWon,
} = bitcoinGameSlice.selectors;

export default bitcoinGameSlice.reducer;

import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from '@reduxjs/toolkit';

export interface NonPlayerWalletState {
    address: number;
    currentBalance: number;
    name: string;
}

export interface JokerWinner {
    name: string;
    numberOfJokersWon: number;
}

export interface BitcoinGameState {
    currentGameId?: string;
    lastRoundNumber?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: Date;
    currentBalance?: number;
    usersWalletAddress?: number;
    nonPlayerWallets: NonPlayerWalletState[];
    moneyWonSoFar: number;
    gameHasFinished?: boolean;
    numberOfJokersWon?: number;
}

interface MakeTransactionAction {
    invokerId: string;
    receiverAddress: number;
    amount: number;
}

export interface ReceiveLoginResultAction {
    loginSuccessful: boolean;
    isAdmin: boolean;
    playerGuid: string;
    currentGameId?: string;
    userWalletAddress?: number;
    userCurrentBalance?: number;
    lastRoundNumber?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    nonPlayerWallets?: NonPlayerWalletState[];
    moneyWonSoFar: number;
    gameHasFinished?: boolean;
    numberOfJokersWon?: number;
    jokerWinners?: JokerWinner[];
}

export interface ReceiveNewGameStateAction {
    currentGameId?: string;
    lastRoundNumber?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    userCurrentBalance?: number;
    userWalletAddress?: number;
    nonPlayerWallets?: NonPlayerWalletState[];
    moneyWonSoFar: number;
    gameHasFinished?: boolean;
    numberOfJokersWon?: number;
    jokerWinners?: JokerWinner[];
}

interface ReceiveMakeTransactionResult {
    transactionSuccessful: boolean;
    userCurrentBalance: number;
}

const initialState: BitcoinGameState = {
    currentGameId: undefined,
    currentRoundNumber: undefined,
    currentRoundEndTime: undefined,
    currentBalance: undefined,
    usersWalletAddress: undefined,
    nonPlayerWallets: [],
    moneyWonSoFar: 0,
    lastRoundNumber: undefined,
    gameHasFinished: undefined,
    numberOfJokersWon: undefined,
};

export const bitcoinGameSlice = createSlice({
    name: 'bitcoinGame',
    initialState,
    reducers: {
        receiveNewGameState: (state, action: PayloadAction<ReceiveNewGameStateAction>) => {
            state.currentGameId = action.payload.currentGameId;
            state.gameHasFinished = action.payload.gameHasFinished;
            state.currentBalance = action.payload.userCurrentBalance;
            state.usersWalletAddress = action.payload.userWalletAddress;
            state.lastRoundNumber = action.payload.lastRoundNumber != null ? action.payload.lastRoundNumber : undefined;
            state.currentRoundNumber = action.payload.currentRoundNumber != null ? action.payload.currentRoundNumber : undefined;
            state.currentRoundEndTime = action.payload.currentRoundEndTime != null ? new Date(action.payload.currentRoundEndTime) : undefined;
            state.nonPlayerWallets = action.payload.nonPlayerWallets != null ? action.payload.nonPlayerWallets : [];
            state.moneyWonSoFar = action.payload.moneyWonSoFar;
            state.numberOfJokersWon = action.payload.numberOfJokersWon != null ? action.payload.numberOfJokersWon : undefined;
            state.jokerWinners = action.payload.jokerWinners != null ? action.payload.jokerWinners : [];
        },

        receiveLoginResult: (state,  action: PayloadAction<ReceiveLoginResultAction>) => {
            state.isLoggedIn = action.payload.loginSuccessful;
            state.isAdmin = action.payload.isAdmin;
            state.playerGuid = action.payload.loginSuccessful ? action.payload.playerGuid : '';
            state.usersWalletAddress = action.payload.userWalletAddress;
            state.currentGameId = action.payload.currentGameId;
            state.gameHasFinished = action.payload.gameHasFinished;
            state.currentBalance = action.payload.userCurrentBalance;
            state.lastRoundNumber = action.payload.lastRoundNumber != null ? action.payload.lastRoundNumber : undefined;
            state.currentRoundEndTime = action.payload.currentRoundEndTime != null ? new Date(action.payload.currentRoundEndTime) : undefined;
            state.currentRoundNumber = action.payload.currentRoundNumber != null ? action.payload.currentRoundNumber : undefined;
            state.nonPlayerWallets = action.payload.nonPlayerWallets != null ? action.payload.nonPlayerWallets : [];
            state.moneyWonSoFar = action.payload.moneyWonSoFar;
            state.numberOfJokersWon = action.payload.numberOfJokersWon != null ? action.payload.numberOfJokersWon : undefined;
            state.jokerWinners = action.payload.jokerWinners != null ? action.payload.jokerWinners : [];
        },
        makeTransaction: (state, action: PayloadAction<MakeTransactionAction>) => {
            state.currentBalance = state.currentBalance === undefined
                ? undefined
                : state.currentBalance >= action.payload.amount && action.payload.amount > 0
                    ? state.currentBalance - action.payload.amount
                    : state.currentBalance;
        },
        receiveMakeTransactionResult: (state, action: PayloadAction<ReceiveMakeTransactionResult>) => {
            state.currentBalance = action.payload.userCurrentBalance;
        }
    },
    selectors: {
        selectCurrentRoundNumber: (sliceState: BitcoinGameState) => sliceState.currentRoundNumber,
        selectCurrentGameId: (sliceState: BitcoinGameState) => sliceState.currentGameId,
        selectGameHasFinished: (sliceState: BitcoinGameState) => sliceState.gameHasFinished,
        selectNonPlayerWallets: (sliceState: BitcoinGameState) => sliceState.nonPlayerWallets,
        selectLastRoundNumber: (sliceState: BitcoinGameState) => sliceState.lastRoundNumber,
        selectCurrentRoundEndTime: (sliceState: BitcoinGameState) => sliceState.currentRoundEndTime,
        selectCurrentBalance: (sliceState: BitcoinGameState) => sliceState.currentBalance,
        selectUsersWalletAddress: (sliceState: BitcoinGameState) => sliceState.usersWalletAddress,
        selectMoneyWonSoFar: (sliceState: BitcoinGameState) => sliceState.moneyWonSoFar,
        selectNumberOfJokersWon: (sliceState: BitcoinGameState) => sliceState.numberOfJokersWon,
    }
});

export const {
    receiveNewGameState,
    receiveMakeTransactionResult,
    receiveLoginResult,
    makeTransaction
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

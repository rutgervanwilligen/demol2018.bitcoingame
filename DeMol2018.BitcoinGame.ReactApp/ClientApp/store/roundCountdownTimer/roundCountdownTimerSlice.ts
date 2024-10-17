import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {BitcoinGameState, ReceiveLoginResultAction, ReceiveNewGameStateAction} from "../bitcoinGame/bitcoinGameSlice";

export interface RoundCountdownTimerState {
    playerGuid: string;
    currentEndTime: Date;
    minutesLeft: number;
    secondsLeft: number;
}

interface UpdateTimeLeftAction {
    secondsLeft: number;
    minutesLeft: number;
}

interface FetchNewGameStateAction {
    playerGuid: string;
}

const initialState: RoundCountdownTimerState = {
    playerGuid: '',
    minutesLeft: 0,
    secondsLeft: 0,
    currentEndTime: new Date()
};

export const roundCountdownTimerSlice = createSlice({
    name: 'roundCountdownTimer',
    initialState,
    reducers: {
        receiveLoginResult: (state, action: PayloadAction<ReceiveLoginResultAction>) => {
            state.playerGuid = action.payload.playerGuid;
            state.currentEndTime = new Date(action.payload.currentRoundEndTime!);
        },
        receiveNewGameState: (state, action: PayloadAction<ReceiveNewGameStateAction>) => {
            state.currentEndTime = new Date(action.payload.currentRoundEndTime!);
        },
        updateTimeLeft: (state, action: PayloadAction<UpdateTimeLeftAction>) => {
            state.minutesLeft = action.payload.minutesLeft;
            state.secondsLeft = action.payload.secondsLeft;
        },
        fetchNewGameState: (state, action: PayloadAction<FetchNewGameStateAction>) => {
            // No-op
        }
    },
    selectors: {
        selectMinutesLeft: (sliceState: RoundCountdownTimerState) => sliceState.minutesLeft,
        selectSecondsLeft: (sliceState: RoundCountdownTimerState) => sliceState.secondsLeft,
    }
});

export const {
    receiveLoginResult,
    receiveNewGameState,
    updateTimeLeft,
    fetchNewGameState
} = roundCountdownTimerSlice.actions;

export const {
    selectMinutesLeft,
    selectSecondsLeft,
} = roundCountdownTimerSlice.selectors;

export default roundCountdownTimerSlice.reducer;

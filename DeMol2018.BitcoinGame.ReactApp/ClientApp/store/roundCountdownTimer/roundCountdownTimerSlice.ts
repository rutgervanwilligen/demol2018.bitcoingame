import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
    ReceiveLoginResultAction,
    receiveNewGameState,
    ReceiveNewGameStateAction
} from "../bitcoinGame/bitcoinGameSlice";
import { receiveLoginResult } from "../user/userSlice";

export interface RoundCountdownTimerState {
    playerGuid: string;
    currentEndTime?: string;
    minutesLeft: number;
    secondsLeft: number;
}

interface UpdateTimeLeftAction {
    secondsLeft: number;
    minutesLeft: number;
}

const initialState: RoundCountdownTimerState = {
    playerGuid: '',
    minutesLeft: 0,
    secondsLeft: 0,
    currentEndTime: undefined
};

export const roundCountdownTimerSlice = createSlice({
    name: 'roundCountdownTimer',
    initialState,
    reducers: {
        updateTimeLeft: (state, action: PayloadAction<UpdateTimeLeftAction>) => {
            state.minutesLeft = action.payload.minutesLeft;
            state.secondsLeft = action.payload.secondsLeft;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(receiveLoginResult, (state: RoundCountdownTimerState, action: PayloadAction<ReceiveLoginResultAction>) => {
                state.playerGuid = action.payload.playerGuid;
                state.currentEndTime = action.payload.currentRoundEndTime;
            })
            .addCase(receiveNewGameState, (state: RoundCountdownTimerState, action: PayloadAction<ReceiveNewGameStateAction>) => {
                state.currentEndTime = action.payload.currentRoundEndTime;
            });
    },
    selectors: {
        selectMinutesLeft: (sliceState: RoundCountdownTimerState) => sliceState.minutesLeft,
        selectSecondsLeft: (sliceState: RoundCountdownTimerState) => sliceState.secondsLeft,
    }
});

export const {
    updateTimeLeft,
} = roundCountdownTimerSlice.actions;

export const {
    selectMinutesLeft,
    selectSecondsLeft,
} = roundCountdownTimerSlice.selectors;

export default roundCountdownTimerSlice.reducer;

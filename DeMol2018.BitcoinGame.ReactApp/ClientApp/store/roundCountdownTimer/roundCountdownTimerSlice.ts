import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
    receiveNewGameState,
    ReceiveNewGameStateAction,
} from "../bitcoinGame/bitcoinGameSlice";

export interface RoundCountdownTimerState {
    currentEndTime?: string;
    minutesLeft: number;
    secondsLeft: number;
}

interface UpdateTimeLeftAction {
    secondsLeft: number;
    minutesLeft: number;
}

const initialState: RoundCountdownTimerState = {
    minutesLeft: 0,
    secondsLeft: 0,
    currentEndTime: undefined,
};

export const roundCountdownTimerSlice = createSlice({
    name: "roundCountdownTimer",
    initialState,
    reducers: {
        updateTimeLeft: (
            state,
            action: PayloadAction<UpdateTimeLeftAction>,
        ) => {
            state.minutesLeft = action.payload.minutesLeft;
            state.secondsLeft = action.payload.secondsLeft;
        },
    },
    extraReducers: (builder) => {
        builder.addCase(
            receiveNewGameState,
            (
                state: RoundCountdownTimerState,
                action: PayloadAction<ReceiveNewGameStateAction>,
            ) => {
                state.currentEndTime = action.payload.currentRoundEndTime;
            },
        );
    },
    selectors: {
        selectMinutesLeft: (sliceState: RoundCountdownTimerState) =>
            sliceState.minutesLeft,
        selectSecondsLeft: (sliceState: RoundCountdownTimerState) =>
            sliceState.secondsLeft,
    },
});

export const { updateTimeLeft } = roundCountdownTimerSlice.actions;

export const { selectMinutesLeft, selectSecondsLeft } =
    roundCountdownTimerSlice.selectors;

export default roundCountdownTimerSlice.reducer;

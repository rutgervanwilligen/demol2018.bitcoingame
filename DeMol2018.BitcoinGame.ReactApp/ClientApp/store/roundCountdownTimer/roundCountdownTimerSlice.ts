import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ReceiveLoginResultAction, ReceiveNewGameStateAction} from "../bitcoinGame/bitcoinGameSlice";

export interface RoundCountdownTimerState {
    playerGuid: string;
    currentEndTime: Date;
    minutesLeft: number;
    secondsLeft: number;
}

interface UpdateTimeLeftAction {
    type: 'UPDATE_TIME_LEFT';
    secondsLeft: number;
    minutesLeft: number;
}

interface FetchNewGameStateAction {
    type: 'FETCH_NEW_GAME_STATE';
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
});

export const {
    receiveLoginResult,
    receiveNewGameState,
    updateTimeLeft,
    fetchNewGameState
} = roundCountdownTimerSlice.actions;

export default roundCountdownTimerSlice.reducer;

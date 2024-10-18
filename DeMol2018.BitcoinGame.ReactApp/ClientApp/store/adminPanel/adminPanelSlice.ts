import {JokerWinner, receiveNewGameState} from "../bitcoinGame/bitcoinGameSlice";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {receiveLoginResult} from "../user/userSlice";

export interface AdminPanelState {
    jokerWinners?: JokerWinner[];
}

interface StartNewRoundAction {
    invokerId: string,
    lengthOfNewRoundInMinutes: number
}

interface StartNewGameAction {
    invokerId: string
}

interface FinishCurrentGameAction {
    invokerId: string
}

const initialState: AdminPanelState = {
    jokerWinners: undefined,
};

export const adminPanelSlice = createSlice({
    name: 'adminPanel',
    initialState,
    reducers: {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        startNewRound: (state: AdminPanelState, action: PayloadAction<StartNewRoundAction>) => {
            // No-op; caught in websocketMiddleware
        },
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        startNewGame: (state: AdminPanelState, action: PayloadAction<StartNewGameAction>) => {
            // No-op; caught in websocketMiddleware
        },
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        finishCurrentGame: (state: AdminPanelState, action: PayloadAction<FinishCurrentGameAction>) => {
            // No-op; caught in websocketMiddleware
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(receiveLoginResult, (state: AdminPanelState, action) => {
                state.jokerWinners = action.payload.jokerWinners;
            })
            .addCase(receiveNewGameState, (state: AdminPanelState, action) => {
                state.jokerWinners = action.payload.jokerWinners;
            })
    },
    selectors: {
        selectJokerWinners: (sliceState: AdminPanelState) => sliceState.jokerWinners,
    }
});

export const {
    startNewRound,
    startNewGame,
    finishCurrentGame,
} = adminPanelSlice.actions;

export const {
    selectJokerWinners,
} = adminPanelSlice.selectors;

export default adminPanelSlice.reducer;

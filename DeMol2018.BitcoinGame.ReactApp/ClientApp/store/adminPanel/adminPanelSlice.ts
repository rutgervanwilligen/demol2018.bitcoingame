import {
    JokerWinner,
    updateGameState,
    UpdateGameStateAction,
} from "../bitcoinGame/bitcoinGameSlice";
import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface AdminPanelState {
    jokerWinners?: JokerWinner[];
}

interface StartNewRoundAction {
    invokerId: string;
    lengthOfNewRoundInMinutes: number;
}

interface StartNewGameAction {
    invokerId: string;
}

interface FinishCurrentGameAction {
    invokerId: string;
}

const initialState: AdminPanelState = {
    jokerWinners: undefined,
};

/* eslint-disable @typescript-eslint/no-unused-vars */
export const adminPanelSlice = createSlice({
    name: "adminPanel",
    initialState,
    reducers: {
        startNewRound: (
            state: AdminPanelState,
            action: PayloadAction<StartNewRoundAction>,
        ) => {
            // No-op; caught in websocketMiddleware
        },

        startNewGame: (
            state: AdminPanelState,
            action: PayloadAction<StartNewGameAction>,
        ) => {
            // No-op; caught in websocketMiddleware
        },

        finishCurrentGame: (
            state: AdminPanelState,
            action: PayloadAction<FinishCurrentGameAction>,
        ) => {
            // No-op; caught in websocketMiddleware
        },
    },
    extraReducers: (builder) => {
        builder.addCase(
            updateGameState,
            (
                state: AdminPanelState,
                action: PayloadAction<UpdateGameStateAction>,
            ) => {
                state.jokerWinners = action.payload.jokerWinners;
            },
        );
    },
    selectors: {
        selectJokerWinners: (sliceState: AdminPanelState) =>
            sliceState.jokerWinners,
    },
});
/* eslint-enable @typescript-eslint/no-unused-vars */

export const { startNewRound, startNewGame, finishCurrentGame } =
    adminPanelSlice.actions;

export const { selectJokerWinners } = adminPanelSlice.selectors;

export default adminPanelSlice.reducer;

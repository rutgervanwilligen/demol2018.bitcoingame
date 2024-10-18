import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { ReceiveLoginResultAction } from "../bitcoinGame/bitcoinGameSlice";

export interface UserState {
    isLoggedIn: boolean,
    isAdmin: boolean,
    playerGuid: string | undefined,
    walletAddress: string | undefined,
}

export interface LoginAction {
    name: string,
    code: number;
}

const initialState: UserState = {
    isLoggedIn: false,
    isAdmin: false,
    playerGuid: undefined,
    walletAddress: undefined,
};

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        // eslint-disable-next-line @typescript-eslint/no-unused-vars
        login: (state, action: PayloadAction<LoginAction>) => {
            // No-op; caught in websocketMiddleware
        },
        receiveLoginResult: (state, action: PayloadAction<ReceiveLoginResultAction>) => {
            state.isLoggedIn = action.payload.loginSuccessful;
            state.playerGuid = action.payload.loginSuccessful ? action.payload.playerGuid : undefined;
            state.walletAddress = action.payload.loginSuccessful ? action.payload.userWalletAddress : undefined;
            state.isAdmin = action.payload.isAdmin;
        }
    },
    selectors: {
        selectIsLoggedIn: (sliceState: UserState) => sliceState.isLoggedIn,
        selectIsAdmin: (sliceState: UserState) => sliceState.isAdmin,
        selectPlayerGuid: (sliceState: UserState) => sliceState.playerGuid,
    }
});

export const {
    login,
    receiveLoginResult
} = userSlice.actions;

export const {
    selectIsLoggedIn,
    selectIsAdmin,
    selectPlayerGuid,
} = userSlice.selectors;

export default userSlice.reducer;

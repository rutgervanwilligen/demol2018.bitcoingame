import { createSlice, PayloadAction } from "@reduxjs/toolkit"

export interface WebsocketConnectionState {
    isConnected: boolean,
}

interface UpdateConnectionStatusAction {
    isConnected: boolean,
}

const initialState: WebsocketConnectionState = {
    isConnected: false,
};

export const websocketConnectionSlice = createSlice({
    name: 'websocketConnection',
    initialState,
    reducers: {
        connectWebsocket: () => {
            // No-op; caught in websocketMiddleware
        },
        updateConnectionStatus: (state: WebsocketConnectionState, action: PayloadAction<UpdateConnectionStatusAction>) => {
            state.isConnected = action.payload.isConnected;
        }
    },
    selectors: {
        selectIsConnected: (sliceState: WebsocketConnectionState) => sliceState.isConnected,
    }
});

export const {
    connectWebsocket,
    updateConnectionStatus,
} = websocketConnectionSlice.actions;

export const {
    selectIsConnected
} = websocketConnectionSlice.selectors;

export default websocketConnectionSlice.reducer;

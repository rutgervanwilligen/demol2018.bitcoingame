import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export enum WebsocketConnectionStatus {
    NotConnected,
    Connected,
    Reconnecting,
    Disconnected,
    ConnectionError,
}

export interface WebsocketConnectionState {
    connectionStatus: WebsocketConnectionStatus;
}

interface UpdateConnectionStatusAction {
    newConnectionStatus: WebsocketConnectionStatus;
}

const initialState: WebsocketConnectionState = {
    connectionStatus: WebsocketConnectionStatus.NotConnected,
};

export const websocketConnectionSlice = createSlice({
    name: "websocketConnection",
    initialState,
    reducers: {
        connectWebsocket: () => {
            // No-op; caught in websocketMiddleware
        },
        updateConnectionStatus: (
            state: WebsocketConnectionState,
            action: PayloadAction<UpdateConnectionStatusAction>,
        ) => {
            state.connectionStatus = action.payload.newConnectionStatus;
        },
    },
    selectors: {
        selectConnectionStatus: (sliceState: WebsocketConnectionState) =>
            sliceState.connectionStatus,
    },
});

export const { connectWebsocket, updateConnectionStatus } =
    websocketConnectionSlice.actions;

export const { selectConnectionStatus } = websocketConnectionSlice.selectors;

export default websocketConnectionSlice.reducer;

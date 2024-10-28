import { websocketMiddleware } from "./store/SignalRMiddleware";

import { configureStore } from "@reduxjs/toolkit";
import bitcoinGameReducer from "./store/bitcoinGame/bitcoinGameSlice";
import roundCountdownTimerReducer from "./store/roundCountdownTimer/roundCountdownTimerSlice";
import userReducer from "./store/user/userSlice";
import adminPanelReducer from "./store/adminPanel/adminPanelSlice";
import websocketConnectionReducer from "./store/websocketConnection/websocketConnectionSlice";
import { useDispatch } from "react-redux";

const store = configureStore({
    reducer: {
        user: userReducer,
        bitcoinGame: bitcoinGameReducer,
        roundCountdownTimer: roundCountdownTimerReducer,
        adminPanel: adminPanelReducer,
        websocketConnection: websocketConnectionReducer,
    },
    middleware: (getDefaultMiddleware) => {
        return getDefaultMiddleware().concat([websocketMiddleware]);
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = useDispatch.withTypes<AppDispatch>();

export default store;

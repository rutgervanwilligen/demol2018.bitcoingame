import { websocketMiddleware } from "./store/SignalRMiddleware";

import { configureStore } from '@reduxjs/toolkit';
import bitcoinGameReducer from "./store/bitcoinGame/bitcoinGameSlice";
import roundCountdownTimerReducer from "./store/roundCountdownTimer/roundCountdownTimerSlice";
import userReducer from "./store/user/userSlice";
import adminPanelReducer from "./store/adminPanel/adminPanelSlice";
import { useDispatch } from "react-redux";

const store = configureStore({
    reducer: {
        user: userReducer,
        bitcoinGame: bitcoinGameReducer,
        roundCountdownTimer: roundCountdownTimerReducer,
        adminPanel: adminPanelReducer,
    },
    middleware: (getDefaultMiddleware) => {
        return getDefaultMiddleware().concat([websocketMiddleware]);
    }
})

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = useDispatch.withTypes<AppDispatch>();

export default store;

// export default function configureStore(initialState?: ApplicationState) : Store<ApplicationState> {
//     // Build middleware. These are functions that can process the actions before they reach the store.
//     const windowIfDefined = typeof window === 'undefined' ? null : window as any;
//     // If devTools is installed, connect to it
//     const devToolsExtension = windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__ as () => StoreEnhancer;
//     const createStoreWithMiddleware = compose(
//         applyMiddleware(thunk, signalRInvokeMiddleware),
//         devToolsExtension ? devToolsExtension() : <S>(next: StoreEnhancerStoreCreator<S>) => next
//     )(createStore);
//
//     // Combine all reducers and instantiate the app-wide store instance
//     const allReducers = buildRootReducer(reducers);
//     const store = (<any>createStoreWithMiddleware)(allReducers, initialState) as Store<ApplicationState>;
//
//     signalRRegisterCommands(store);
//
//     // Enable Webpack hot module replacement for reducers
//     if (module.hot) {
//         module.hot.accept('./store', () => {
//             const nextRootReducer = require<typeof StoreModule>('./store');
//             store.replaceReducer(buildRootReducer(nextRootReducer.reducers));
//         });
//     }
//
//     return store;
// }
//
// function buildRootReducer(allReducers: ReducersMapObject) {
//     return combineReducers<ApplicationState>(Object.assign({}, allReducers));
// }

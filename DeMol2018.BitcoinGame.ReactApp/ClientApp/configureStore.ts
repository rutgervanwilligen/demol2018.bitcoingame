import { createStore, applyMiddleware, compose, combineReducers, StoreEnhancer, Store, StoreEnhancerStoreCreator, ReducersMapObject } from "redux";
import thunk from "redux-thunk";
import * as StoreModule from "./store";
import { ApplicationState, reducers } from "./store";
import { signalRInvokeMiddleware, signalRRegisterCommands } from "./store/SignalRMiddleware";

export default function configureStore(initialState?: ApplicationState) : Store<ApplicationState> {
    // Build middleware. These are functions that can process the actions before they reach the store.
    const windowIfDefined = typeof window === 'undefined' ? null : window as any;
    // If devTools is installed, connect to it
    const devToolsExtension = windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__ as () => StoreEnhancer;
    const createStoreWithMiddleware = compose(
        applyMiddleware(thunk, signalRInvokeMiddleware),
        devToolsExtension ? devToolsExtension() : <S>(next: StoreEnhancerStoreCreator<S>) => next
    )(createStore);

    // Combine all reducers and instantiate the app-wide store instance
    const allReducers = buildRootReducer(reducers);
    const store = (<any>createStoreWithMiddleware)(allReducers, initialState) as Store<ApplicationState>;

    signalRRegisterCommands(store);

    // Enable Webpack hot module replacement for reducers
    if (module.hot) {
        module.hot.accept('./store', () => {
            const nextRootReducer = require<typeof StoreModule>('./store');
            store.replaceReducer(buildRootReducer(nextRootReducer.reducers));
        });
    }

    return store;
}

function buildRootReducer(allReducers: ReducersMapObject) {
    return combineReducers<ApplicationState>(Object.assign({}, allReducers));
}

import './css/site.css';
import * as React from 'react';
import { Provider } from 'react-redux';
import { ApplicationState }  from './store';
import BitcoinGame from "./components/BitcoinGame";
import configureStore from "./configureStore";
import { createRoot } from 'react-dom/client';

// Create browser history to use in the Redux store
// const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
// const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialReduxState as ApplicationState;
const store = configureStore(initialState);

function renderApp() {
    const container = document.getElementById('react-app');
    const root = createRoot(container!);

    root.render(
        <Provider store={ store }>
            <BitcoinGame />
        </Provider>
    );
}

renderApp();
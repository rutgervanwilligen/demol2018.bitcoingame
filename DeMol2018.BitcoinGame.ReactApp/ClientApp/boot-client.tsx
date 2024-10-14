import './css/site.css';
import * as React from 'react';
import { Provider } from 'react-redux';
import { ApplicationState }  from './store';
import BitcoinGame from "./components/BitcoinGame";
import configureStore from "./configureStore";
import { createRoot } from 'react-dom/client';

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

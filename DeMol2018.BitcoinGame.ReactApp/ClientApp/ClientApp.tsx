import './css/site.css';
import * as React from 'react';
import { Provider } from 'react-redux';
import BitcoinGame from "./components/BitcoinGame";
import store from "./configureStore";
import { createRoot } from 'react-dom/client';

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

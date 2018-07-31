import './css/site.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ApplicationState, reducers }  from './store';
import BitcoinGame from "./components/BitcoinGame";
import configureStore from "./configureStore";

// Create browser history to use in the Redux store
// const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;
// const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialReduxState as ApplicationState;
const store = configureStore(initialState);

function renderApp() {
    ReactDOM.render(
            <Provider store={ store }>
                <BitcoinGame />
            </Provider>,
        document.getElementById('react-app')
    );
}

renderApp();
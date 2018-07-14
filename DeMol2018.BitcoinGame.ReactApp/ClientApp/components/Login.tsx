import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

type LoginProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class Login extends React.Component<LoginProps> {
    public render() {
        return  <div className="login">
                    <h2>Login</h2>
                    <input className="ïnputField" placeholder='Naam' ref='name' />
                    <input className="ïnputField" placeholder='Code' ref='loginCode' />
                    <button onClick={() => this.props.makeTransaction(10, 10)}>Hoi</button>
                </div>;
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(Login);//as typeof Login;
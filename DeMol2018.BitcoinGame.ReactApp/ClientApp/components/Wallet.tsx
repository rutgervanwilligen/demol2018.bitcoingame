import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import MakeTransaction from "./MakeTransaction";

type WalletProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class Wallet extends React.Component<WalletProps> {
    public render() {
        return (
            <div className="wallet">
                <h2>Mijn walletadres: { this.props.usersWalletAddress }</h2>
                <h2>Mijn saldo: { this.props.currentBalance }</h2>
                <MakeTransaction />
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(Wallet);// as typeof Wallet;
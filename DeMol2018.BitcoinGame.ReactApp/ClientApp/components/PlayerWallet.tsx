import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import MakeTransaction from "./MakeTransaction";

type WalletProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class PlayerWallet extends React.Component<WalletProps> {
    public render() {
        return (
            <div className="wallet">
                <h2 className="walletHeader">Mijn wallet</h2>
                <div className="currentBalance">
                    <div className="currentBalanceHeader">Saldo</div>
                    <div className="currentBalanceText">{ this.props.currentBalance } BTC</div>
                </div>
                <div className="walletAddress">
                    <div className="walletAddressHeader">Adres</div>
                    <div className="walletAddressText">{ this.props.usersWalletAddress }</div>
                </div>
                <MakeTransaction />
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(PlayerWallet);// as typeof PlayerWallet;
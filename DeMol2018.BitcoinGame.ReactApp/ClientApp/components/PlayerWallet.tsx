import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import MakeTransaction from "./MakeTransaction";

const connector = connect((state: ApplicationState) => state.bitcoinGame);
type PlayerWalletProps = ConnectedProps<typeof connector>

class PlayerWallet extends React.Component<PlayerWalletProps> {
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
export default connector(PlayerWallet);
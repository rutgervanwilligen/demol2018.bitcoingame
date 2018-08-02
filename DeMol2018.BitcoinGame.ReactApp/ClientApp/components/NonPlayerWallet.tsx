import * as React from 'react';
import {NonPlayerWalletState} from "../store/BitcoinGame";

export default class NonPlayerWallet extends React.Component<NonPlayerWalletState> {
    public render() {
        return (
            <div className="nonPlayerWallet">
                <h2 className="walletHeader">{ this.props.name }</h2>
                <div className="currentBalance">
                    <div className="currentBalanceHeader">Saldo</div>
                    <div className="currentBalanceText">{ this.props.currentBalance } BTC</div>
                </div>
                <div className="walletAddress">
                    <div className="walletAddressHeader">Adres</div>
                    <div className="walletAddressText">{ this.props.address }</div>
                </div>
            </div>
        );
    }
}

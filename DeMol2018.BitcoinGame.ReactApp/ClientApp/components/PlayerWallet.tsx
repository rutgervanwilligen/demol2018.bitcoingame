import * as React from 'react';
import { useSelector } from 'react-redux';
import { selectCurrentBalance, selectUsersWalletAddress } from "../store/bitcoinGame/bitcoinGameSlice";
import { MakeTransaction } from "./MakeTransaction";

export const PlayerWallet = () => {
    const currentBalance = useSelector(selectCurrentBalance);
    const usersWalletAddress = useSelector(selectUsersWalletAddress);

    return (
        <div className="wallet">
            <h2 className="walletHeader">Mijn wallet</h2>
            <div className="currentBalance">
                <div className="currentBalanceHeader">Saldo</div>
                <div className="currentBalanceText">{ currentBalance } BTC</div>
            </div>
            <div className="walletAddress">
                <div className="walletAddressHeader">Adres</div>
                <div className="walletAddressText">{ usersWalletAddress }</div>
            </div>
            <MakeTransaction />
        </div>
    );
};

import * as React from "react";

interface NonPlayerWalletProps {
    name: string;
    currentBalance: number;
    address: number;
}

export const NonPlayerWallet = (props: NonPlayerWalletProps) => {
    return (
        <div className="nonPlayerWallet">
            <h2 className="walletHeader">{props.name}</h2>
            <div className="currentBalance">
                <div className="currentBalanceHeader">Saldo</div>
                <div className="currentBalanceText">
                    {props.currentBalance} BTC
                </div>
            </div>
            <div className="walletAddress">
                <div className="walletAddressHeader">Adres</div>
                <div className="walletAddressText">{props.address}</div>
            </div>
        </div>
    );
};

import * as React from 'react';
import { RoundManager } from "./RoundManager";
import { NonPlayerWallet } from "./NonPlayerWallet";
import { MoneyWonSoFar } from "./MoneyWonSoFar";
import { useSelector } from "react-redux";
import {
    selectCurrentGameId,
    selectGameHasFinished,
    selectNonPlayerWallets
} from "../store/bitcoinGame/bitcoinGameSlice";

export const GameManager = () => {
    const currentGameId = useSelector(selectCurrentGameId);
    const gameHasFinished = useSelector(selectGameHasFinished);
    const nonPlayerWallets = useSelector(selectNonPlayerWallets);

    if (currentGameId == null) {
        return (
            <div className="gameManager">
                <div className="gameStatus">
                    <div className="gameStatusHeader">Spelstatus</div>
                    <div className="gameStatusText error">Nog niet gestart</div>
                </div>
                <MoneyWonSoFar />
            </div>
        );
    }

    return (
        <div className="gameManager">
            <div className="gameStatus">
                <div className="gameStatusHeader">Spelstatus</div>
                { gameHasFinished
                    ? <div className="gameStatusText error">Afgelopen</div>
                    : <div className="gameStatusText">Gestart</div> }
            </div>
            <MoneyWonSoFar />
            <div className="nonPlayerWallets">
                {nonPlayerWallets.map((wallet, i) => <NonPlayerWallet
                    key = {i}
                    name = {wallet.name}
                    currentBalance = {wallet.currentBalance}
                    address = {wallet.address}
                />)}
            </div>
            { gameHasFinished ? null : <RoundManager /> }
        </div>
    );
}

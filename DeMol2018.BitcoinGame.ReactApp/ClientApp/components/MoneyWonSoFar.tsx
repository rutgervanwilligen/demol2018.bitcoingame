import * as React from "react";
import { useSelector } from "react-redux";
import {
    selectGameHasFinished,
    selectMoneyWonSoFar,
} from "../store/bitcoinGame/bitcoinGameSlice";

export const MoneyWonSoFar = () => {
    const gameHasFinished = useSelector(selectGameHasFinished);
    const moneyWonSoFar = useSelector(selectMoneyWonSoFar);

    const headerText = gameHasFinished
        ? "In totaal verdiend:"
        : "Tot nu toe verdiend:";

    const moneyClass = moneyWonSoFar < 0 ? " negative" : "";

    return (
        <div className="moneyWonSoFar">
            <div className="moneyWonSoFarHeader">{headerText}</div>
            <div className={"moneyWonSoFarText" + moneyClass}>
                &euro; {moneyWonSoFar}
            </div>
        </div>
    );
};

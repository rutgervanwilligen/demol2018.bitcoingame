import * as React from "react";
import { useSelector } from "react-redux";
import { useState } from "react";
import {
    makeTransaction,
    selectCurrentRoundNumber,
} from "../store/bitcoinGame/bitcoinGameSlice";
import { useAppDispatch } from "../configureStore";
import { selectPlayerGuid } from "../store/user/userSlice";

export const MakeTransaction = () => {
    const [amount, setAmount] = useState("");
    const [receiverAddress, setReceiverAddress] = useState("");

    const currentRoundNumber = useSelector(selectCurrentRoundNumber);
    const playerGuid = useSelector(selectPlayerGuid);
    const dispatch = useAppDispatch();

    const makeTransactionAndClearFields = (
        playerGuid?: string,
        amount?: number,
        receiverAddress?: number,
    ) => {
        if (!playerGuid || !amount || !receiverAddress) {
            return;
        }

        dispatch(
            makeTransaction({
                invokerId: playerGuid,
                amount: amount,
                receiverAddress: receiverAddress,
            }),
        );

        setReceiverAddress("");
        setAmount("");
    };

    return (
        <div className="makeTransaction">
            <h2 className="makeTransactionHeader">Overmaken</h2>
            <div className="amount">
                <label>Hoeveelheid BTC</label>
                <input
                    className="inputField"
                    placeholder="Hoeveelheid"
                    type={"number"}
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                />
            </div>
            <div className="receiverAddress">
                <label>Ontvangstadres</label>
                <input
                    className="inputField"
                    placeholder="Ontvangstadres"
                    type={"number"}
                    value={receiverAddress}
                    onChange={(e) => setReceiverAddress(e.target.value)}
                />
            </div>
            {currentRoundNumber !== undefined &&
            amount !== "" &&
            receiverAddress !== "" ? (
                <button
                    className="button"
                    onClick={() =>
                        makeTransactionAndClearFields(
                            playerGuid,
                            parseInt(amount),
                            parseInt(receiverAddress),
                        )
                    }
                >
                    Verstuur
                </button>
            ) : (
                <button className="button" disabled>
                    Verstuur
                </button>
            )}
        </div>
    );
};

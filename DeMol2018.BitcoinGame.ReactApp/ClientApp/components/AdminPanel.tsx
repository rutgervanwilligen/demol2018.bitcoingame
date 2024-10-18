import * as React from 'react';
import { useSelector } from "react-redux";
import { selectCurrentGameId, selectGameHasFinished, selectLastRoundNumber } from "../store/bitcoinGame/bitcoinGameSlice";
import { useAppDispatch } from "../configureStore";
import { finishCurrentGame, selectJokerWinners, startNewGame, startNewRound } from "../store/adminPanel/adminPanelSlice";
import { selectPlayerGuid } from "../store/user/userSlice";
import { useState } from "react";

export const AdminPanel = () => {
    const [newRoundLength, setNewRoundLength] = useState("");

    const playerGuid = useSelector(selectPlayerGuid);
    const currentGameId = useSelector(selectCurrentGameId);
    const gameHasFinished = useSelector(selectGameHasFinished);
    const lastRoundNumber = useSelector(selectLastRoundNumber);
    const jokerWinners = useSelector(selectJokerWinners);

    const dispatch = useAppDispatch();

    if (!currentGameId) {
        return (
            <div className="adminPanel">
                <h2>Adminpaneel</h2>
                <h3>Start nieuw spel</h3>
                <button
                    className="button"
                    onClick={() => dispatch(startNewGame({ invokerId: playerGuid! }))}
                >Start nieuw spel</button>
            </div>
        );
    }

    if (gameHasFinished) {
        return (
            <div className="adminPanel">
                <h2>Adminpaneel</h2>
                <h3>Game ID: {currentGameId}</h3>
                <h3>Aantal gespeelde rondes: {lastRoundNumber == undefined ? "0" : lastRoundNumber}</h3>
                <h2>Jokerwinnaars</h2>
                <table className="jokerWinnerTable">
                    <tbody>
                        {jokerWinners!.map((winner, i) =>
                            <tr key={i}>
                                <td className="jokerWinnerName">{ winner.name }</td>
                                <td className="jokerWinnerNumberOfJokersWon">{ winner.numberOfJokersWon } joker{ winner.numberOfJokersWon != 1 ? "s" : ""}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
                <h3>Start nieuw spel</h3>
                <button
                    className="button"
                    onClick={() => dispatch(startNewGame({ invokerId: playerGuid! }))}
                >Start nieuw spel</button>
            </div>
        );
    }

    return (
        <div className="adminPanel">
            <h2>Adminpaneel</h2>
            <h3>Game ID: {currentGameId}</h3>
            <h3>Start nieuwe ronde</h3>
            <input
                className="inputField"
                placeholder='Lengte van nieuwe ronde (min)'
                type={"number"}
                value={newRoundLength}
                onChange={e => setNewRoundLength(e.target.value)}/>
            <button
                className="button"
                onClick={() => dispatch(startNewRound({
                    invokerId: playerGuid!,
                    lengthOfNewRoundInMinutes: parseInt(newRoundLength),
                }))}
            >Start nieuwe ronde</button>
            <h3>Rond huidig spel af</h3>
            <button
                className="button"
                onClick={() => dispatch(finishCurrentGame({ invokerId: playerGuid! }))}
            >Rond huidig spel af</button>
        </div>
    );
}

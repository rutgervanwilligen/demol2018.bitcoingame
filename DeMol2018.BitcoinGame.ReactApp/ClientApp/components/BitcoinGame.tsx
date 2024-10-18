import * as React from 'react';
import { PlayerWallet } from "./PlayerWallet";
import { AdminPanel } from "./AdminPanel";
import { PlayerResult } from "./PlayerResult";
import { GameManager } from "./GameManager";
import { useSelector } from "react-redux";
import { selectIsAdmin, selectIsLoggedIn } from "../store/user/userSlice";
import { selectCurrentGameId, selectGameHasFinished } from "../store/bitcoinGame/bitcoinGameSlice";
import { Login } from "./Login";
import { ConnectionStatus } from "./ConnectionStatus";

export const BitcoinGame = () => {
    const isLoggedIn = useSelector(selectIsLoggedIn);
    const isAdmin = useSelector(selectIsAdmin);
    const currentGameId = useSelector(selectCurrentGameId);
    const gameHasFinished = useSelector(selectGameHasFinished);

    if (!isLoggedIn) {
        return (
        <>
            <Login />
            <ConnectionStatus />
        </>
        )
    }

    let gameContent;

    if (isAdmin) {
        gameContent = <AdminPanel />;
    } else if (currentGameId != null) {
        if (gameHasFinished) {
            gameContent = <PlayerResult />;
        } else {
            gameContent = <PlayerWallet />;
        }
    }

    return (
        <>
            <div className="bitcoinGame">
                <GameManager />
                { gameContent }
            </div>
            <div className="connectionStatus">
                <ConnectionStatus />
            </div>
        </>
    );
};

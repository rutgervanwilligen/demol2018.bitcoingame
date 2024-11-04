import * as React from "react";
import { PlayerWallet } from "./PlayerWallet";
import { AdminPanel } from "./AdminPanel";
import { PlayerResult } from "./PlayerResult";
import { GameManager } from "./GameManager";
import { useSelector } from "react-redux";
import { selectIsAdmin, selectIsLoggedIn } from "../store/user/userSlice";
import {
    selectCurrentGameId,
    selectGameHasFinished,
} from "../store/bitcoinGame/bitcoinGameSlice";
import { Login } from "./Login";
import { ConnectionStatus } from "./ConnectionStatus";
import { useEffect } from "react";
import { useAppDispatch } from "../configureStore";
import { connectWebsocket } from "../store/websocketConnection/websocketConnectionSlice";

export const BitcoinGame = () => {
    const isLoggedIn = useSelector(selectIsLoggedIn);
    const isAdmin = useSelector(selectIsAdmin);
    const currentGameId = useSelector(selectCurrentGameId);
    const gameHasFinished = useSelector(selectGameHasFinished);

    const dispatch = useAppDispatch();

    useEffect(() => {
        dispatch(connectWebsocket());
    });

    if (!isLoggedIn) {
        return (
            <>
                <Login />
                <ConnectionStatus />
            </>
        );
    }

    const gameContent = getGameContent(isAdmin, currentGameId, gameHasFinished);

    return (
        <>
            <div className="bitcoinGame">{gameContent}</div>
            <ConnectionStatus />
        </>
    );
};

const getGameContent = (
    isAdmin: boolean,
    currentGameId?: string,
    gameHasFinished?: boolean,
) => {
    if (isAdmin) {
        return (
            <>
                <GameManager />
                <AdminPanel />
            </>
        );
    }

    if (currentGameId == undefined) {
        return <></>;
    }

    if (gameHasFinished) {
        return (
            <>
                <GameManager />
                <PlayerResult />
            </>
        );
    }

    return (
        <>
            <GameManager />
            <PlayerWallet />
        </>
    );
};

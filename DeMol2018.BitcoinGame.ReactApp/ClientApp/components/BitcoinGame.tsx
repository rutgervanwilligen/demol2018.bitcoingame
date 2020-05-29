import * as React from 'react';
import {connect, ConnectedProps} from "react-redux";
import {ApplicationState} from "../store";
import Login from "./Login";
import PlayerWallet from "./PlayerWallet";
import AdminPanel from "./AdminPanel";
import GameManager from "./GameManager";
import PlayerResult from "./PlayerResult";

const connector = connect((state: ApplicationState) => state.bitcoinGame);
type BitcoinGameProps = ConnectedProps<typeof connector>

class BitcoinGame extends React.Component<BitcoinGameProps> {
    public render() {
        const isLoggedIn = this.props.isLoggedIn;
        const isAdmin = this.props.isAdmin;

        if (!isLoggedIn) {
            return (
                <Login />
            )
        }

        let gameContent;

        if (isAdmin) {
            gameContent = <AdminPanel />;
        } else if (this.props.currentGameId != null) {
            if (this.props.gameHasFinished) {
                gameContent = <PlayerResult />;
            } else {
                gameContent = <PlayerWallet />;
            }
        }

        return (
            <div className="bitcoinGame">
                <GameManager />
                { gameContent }
            </div>
        );
    }
}

export default connector(BitcoinGame);
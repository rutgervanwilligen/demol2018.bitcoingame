import * as React from 'react';
import * as BitcoinGameStore from "../store/BitcoinGame";
import {connect} from "react-redux";
import {ApplicationState} from "../store";
import Login from "./Login";
import PlayerWallet from "./PlayerWallet";
import AdminPanel from "./AdminPanel";
import GameManager from "./GameManager";
import PlayerResult from "./PlayerResult";

type BitcoinGameProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

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

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(BitcoinGame);// as typeof BitcoinGameRoot;
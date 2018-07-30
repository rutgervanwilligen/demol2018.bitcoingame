import * as React from 'react';
import * as BitcoinGameStore from "../store/BitcoinGame";
import {connect} from "react-redux";
import {ApplicationState} from "../store";
import Login from "./Login";
import Wallet from "./Wallet";
import MakeTransaction from "./MakeTransaction";
import AdminPanel from "./AdminPanel";
import RoundManager from "./RoundManager";
import GameManager from "./GameManager";

type BitcoinGameProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class BitcoinGame extends React.Component<BitcoinGameProps> {
    public render() {
        const isLoggedIn = this.props.isLoggedIn;
        const isAdmin = this.props.isAdmin;

        if (!isLoggedIn) {
            return (
                <div>
                    <Login />
                </div>
            )
        }
        
        if (isAdmin) {
            return (
                <div>
                    <GameManager />
                    <AdminPanel />
                </div>
            )
        } 

        return (
            <div>
                <GameManager />
                <Wallet />
                <MakeTransaction />
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(BitcoinGame);// as typeof BitcoinGameRoot;
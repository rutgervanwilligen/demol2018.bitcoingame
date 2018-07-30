import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import RoundManager from "./RoundManager";

type GameManagerProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class GameManager extends React.Component<GameManagerProps> {

    public render() {
        if (this.props.currentGameId != null) {
            return (
                <div className="gameManager">
                    <h2>Het spel is begonnen!</h2>
                    <h3>Game ID: {this.props.currentGameId!}</h3>
                    <RoundManager />
                </div>
            );
        } else {
            return (
                <div className="gameManager">
                     Er is nog geen spel gestart.
                </div>
            );
        }
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(GameManager);//as typeof RoundManager;
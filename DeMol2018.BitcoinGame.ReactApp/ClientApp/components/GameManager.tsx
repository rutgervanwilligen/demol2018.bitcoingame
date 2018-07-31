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
                    <div className="gameStatusHeader">Spelstatus</div>
                    <div className="gameStatusText">Gestart</div>
                    <RoundManager />
                </div>
            );
        } else {
            return (
                <div className="gameManager">
                    <div className="gameStatusHeader">Spelstatus</div>
                    <div className="gameStatusText error">Nog niet gestart</div>
                </div>
            );
        }
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(GameManager);// as typeof GameManager;
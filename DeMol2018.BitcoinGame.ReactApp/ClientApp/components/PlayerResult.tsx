import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

type PlayerResultProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class PlayerResult extends React.Component<PlayerResultProps> {
    public render() {
        return (
            <div className="playerResult">
                <h2 className="playerResultHeader">Mijn resultaat</h2>
                <div className="playerResultBalance">
                    <div className="playerResultBalanceHeader">Eindsaldo</div>
                    <div className="playerResultBalanceText">{ this.props.currentBalance } BTC</div>
                </div>
                <div className="jokersWon">
                    <div className="jokersWonHeader">Aantal gewonnen jokers</div>
                    <div className="jokersWonText">{ this.props.numberOfJokersWon }</div>
                </div>
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(PlayerResult);// as typeof PlayerResult;
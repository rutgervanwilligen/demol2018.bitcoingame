import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';

const connector = connect((state: ApplicationState) => state.bitcoinGame);
type PlayerResultProps = ConnectedProps<typeof connector>

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

export default connector(PlayerResult);
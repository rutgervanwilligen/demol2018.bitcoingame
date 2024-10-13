import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import RoundCountdownTimer from "./RoundCountdownTimer";

const connector = connect((state: ApplicationState) => state.bitcoinGame, BitcoinGameStore.actionCreators);
type RoundManagerProps = ConnectedProps<typeof connector>

class RoundManager extends React.Component<RoundManagerProps> {

    public render() {
        if (this.props.currentRoundNumber != null && this.props.currentRoundEndTime != null) {
            return (
                <div className="roundManager">
                    <div className="roundNumber">Ronde {this.props.currentRoundNumber}</div>
                    <div className="timeLeft">Transacties worden verwerkt over: <RoundCountdownTimer/></div>
                </div>
            );
        } else if (this.props.lastRoundNumber != null) {
            return (
                <div className="roundManager noRoundActive">
                    Geen ronde actief. Laatst gespeelde ronde: {this.props.lastRoundNumber}
                </div>
            );
        } else {
            return (
                <div className="roundManager noRoundActive">
                     Geen ronde actief. De eerste ronde begint binnenkort.
                </div>
            );
        }
    }
}

export default connector(RoundManager);
import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import RoundCountdownTimer from "./RoundCountdownTimer";

type RoundManagerProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class RoundManager extends React.Component<RoundManagerProps> {

    public render() {
        if (this.props.currentRoundNumber != null && this.props.currentRoundEndTime != null) {
            return (
                <div className="roundManager">
                    <div className="roundNumber">Ronde {this.props.currentRoundNumber}</div>
                    <div className="timeLeft">Tijd over: <RoundCountdownTimer/></div>
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

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(RoundManager);//as typeof RoundManager;
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
                    <h2>Ronde {this.props.currentRoundNumber}</h2>
                    <h3>Tijd over: <RoundCountdownTimer /></h3>
                </div>
            );
        } else {
            return (
                <div className="roundManager">
                     Geen ronde actief.
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
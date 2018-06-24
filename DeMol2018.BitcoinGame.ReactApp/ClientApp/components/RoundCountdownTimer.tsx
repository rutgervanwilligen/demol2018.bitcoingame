import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

type RoundCountdownTimerProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class RoundCountdownTimer extends React.Component<RoundCountdownTimerProps> {
    public render() {
        return  <div className="roundCountdownTimer">
                    <h2>Ronde { this.props.currentRoundNumber }</h2>
                    <h3>Tijd tot einde ronde: { this.props.currentRoundEndTime }</h3>
                    <button onClick={() => this.props.makeTransaction(10, 10)}>Hoi</button>
                </div>;
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(RoundCountdownTimer);//as typeof RoundCountdownTimer;
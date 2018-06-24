import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

type WalletProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class Wallet extends React.Component<WalletProps> {
    public render() {
        return  <div className="wallet">
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
)(Wallet);//as typeof RoundCountdownTimer;
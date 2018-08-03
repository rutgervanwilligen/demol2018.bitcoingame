import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import {BitcoinGameState} from "../store/BitcoinGame";

class MoneyWonSoFar extends React.Component<BitcoinGameState> {
    public render() {
        return (
            <div className="moneyWonSoFar">
                <h2 className="moneyWonSoFarHeader">Tot nu toe verdiend:</h2>
                <div className="moneyWonSoFarText">
                    &euro; {this.props.moneyWonSoFar}
                </div>
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(MoneyWonSoFar);// as typeof PlayerWallet;
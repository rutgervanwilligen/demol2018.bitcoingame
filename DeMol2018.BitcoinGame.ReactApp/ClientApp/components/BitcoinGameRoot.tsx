import * as React from 'react';
import RoundCountdownTimer from "./RoundCountdownTimer";
import * as BitcoinGameStore from "../store/BitcoinGame";
import {connect} from "react-redux";
import {ApplicationState} from "../store";
import Login from "./Login";

type BitcoinGameProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class BitcoinGameRoot extends React.Component<BitcoinGameProps> {
    constructor(props: any) {
        super(props);

        this.state = {
            currentRoundNumber: this.props.currentRoundNumber,
            currentRoundEndTime: this.props.currentRoundEndTime,
            currentBalance: this.props.currentBalance
        };
    }

    public render() {
        const isLoggedIn = this.props.isLoggedIn;

        if (!isLoggedIn) {
            return (
                <div>
                    <Login />
                </div>
            )
        }

        return (
            <div>
                <RoundCountdownTimer />
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(BitcoinGameRoot);// as typeof BitcoinGameRoot;
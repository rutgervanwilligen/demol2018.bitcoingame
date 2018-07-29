import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import * as AdminPanelStore from '../store/AdminPanel';

type AdminPanelProps =
    BitcoinGameStore.BitcoinGameState
    & typeof AdminPanelStore.actionCreators;

class AdminPanel extends React.Component<AdminPanelProps> {

    private newRoundLengthInput: HTMLInputElement;

    setNewRoundLengthInputRef = (element: HTMLInputElement) => {
        this.newRoundLengthInput = element;
    };

    constructor(props: AdminPanelProps) {
        super(props);

        this.setNewRoundLengthInputRef = this.setNewRoundLengthInputRef.bind(this);
    }
    
    public render() {
        return (
            <div className="adminPanel">
                <h2>Adminpaneel</h2>
                <h3>Start nieuw spel</h3>
                <button onClick={() => this.props.startNewGame(this.props.playerGuid)}>Start nieuw spel</button>
                <h3>Start nieuwe ronde</h3>
                <input className="ïnputField" placeholder='Lengte van nieuwe ronde (min)' ref={this.setNewRoundLengthInputRef} />
                <button onClick={() => this.props.startNewRound(this.props.playerGuid, +this.newRoundLengthInput.value)}>Start nieuwe ronde</button>
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    AdminPanelStore.actionCreators // Selects which action creators are merged into the component's props
)(AdminPanel);//as typeof RoundCountdownTimer;
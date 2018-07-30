import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

type MakeTransactionProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class Wallet extends React.Component<MakeTransactionProps> {
    private amountInput: HTMLInputElement;
    private receiverAddressInput: HTMLInputElement;

    setAmountInputRef = (element: HTMLInputElement) => {
        this.amountInput = element;
    };

    setReceiverAddressInputRef = (element: HTMLInputElement) => {
        this.receiverAddressInput = element;
    };

    constructor(props: MakeTransactionProps) {
        super(props);

        this.setAmountInputRef = this.setAmountInputRef.bind(this);
        this.setReceiverAddressInputRef = this.setReceiverAddressInputRef.bind(this);
    }
    
    makeTransactionAndClearFields = () => {
        let receiverAddress = +this.receiverAddressInput.value;
        let amount = +this.amountInput.value;

        this.props.makeTransaction(this.props.playerGuid!, receiverAddress, amount);

        this.receiverAddressInput.value = '';
        this.amountInput.value = '';
    };

    public render() {
        return (
            <div className="makeTransaction">
                <h2>Maak geld over</h2>
                <label>Hoeveelheid (heel getal)</label>
                <input className="ïnputField" placeholder='Hoeveelheid' ref={this.setAmountInputRef} />
                <label>Ontvangstadres</label>
                <input className="ïnputField" placeholder='Ontvangstadres' ref={this.setReceiverAddressInputRef} />
                <button onClick={this.makeTransactionAndClearFields}>Verstuur</button>
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(Wallet);//as typeof RoundCountdownTimer;
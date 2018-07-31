import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

type MakeTransactionProps =
    BitcoinGameStore.BitcoinGameState
    & typeof BitcoinGameStore.actionCreators;

class MakeTransaction extends React.Component<MakeTransactionProps> {
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
                <h2 className="makeTransactionHeader">Overmaken</h2>
                <label>Hoeveelheid BTC</label>
                <input className="ïnputField" placeholder='Hoeveelheid' ref={this.setAmountInputRef} />
                <label>Ontvangstadres</label>
                <input className="ïnputField" placeholder='Ontvangstadres' ref={this.setReceiverAddressInputRef} />
                {this.props.currentRoundNumber !== undefined
                    ? <button onClick={this.makeTransactionAndClearFields}>Verstuur</button>
                    : <button disabled>Verstuur</button>
                }
            </div>
        );
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    BitcoinGameStore.actionCreators // Selects which action creators are merged into the component's props
)(MakeTransaction);// as typeof MakeTransaction;
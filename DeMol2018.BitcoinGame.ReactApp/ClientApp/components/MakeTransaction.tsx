import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';

const connector = connect((state: ApplicationState) => state.bitcoinGame, BitcoinGameStore.actionCreators);
type MakeTransactionProps = ConnectedProps<typeof connector>

class MakeTransaction extends React.Component<MakeTransactionProps> {
    private readonly amountInput: React.RefObject<HTMLInputElement>;
    private readonly receiverAddressInput: React.RefObject<HTMLInputElement>;

    constructor(props: MakeTransactionProps) {
        super(props);

        this.amountInput = React.createRef();
        this.receiverAddressInput = React.createRef();
    }

    makeTransactionAndClearFields = () => {
        let receiverAddress = +this.receiverAddressInput.current!.value;
        let amount = +this.amountInput.current!.value;

        this.props.makeTransaction(this.props.playerGuid!, receiverAddress, amount);

        this.receiverAddressInput.current!.value = '';
        this.amountInput.current!.value = '';
    };

    public render() {
        return (
            <div className="makeTransaction">
                <h2 className="makeTransactionHeader">Overmaken</h2>
                <div className="amount">
                    <label>Hoeveelheid BTC</label>
                    <input className="inputField" placeholder='Hoeveelheid' ref={this.amountInput} />
                </div>
                <div className="receiverAddress">
                    <label>Ontvangstadres</label>
                    <input className="inputField" placeholder='Ontvangstadres' ref={this.receiverAddressInput} />
                </div>
                {this.props.currentRoundNumber !== undefined
                    ? <button className="button" onClick={this.makeTransactionAndClearFields}>Verstuur</button>
                    : <button className="button" disabled>Verstuur</button>
                }
            </div>
        );
    }
}

export default connector(MakeTransaction);
import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';

const connector = connect((state: ApplicationState) => state.bitcoinGame);
type MoneyWonSoFarProps = ConnectedProps<typeof connector>

class MoneyWonSoFar extends React.Component<MoneyWonSoFarProps> {
    public render() {
        let headerText = this.props.gameHasFinished
            ? "In totaal verdiend:"
            : "Tot nu toe verdiend:";

        let moneyClass = this.props.moneyWonSoFar < 0 ? " negative" : "";

        return (
            <div className="moneyWonSoFar">
                <div className="moneyWonSoFarHeader">{ headerText }</div>
                <div className={ "moneyWonSoFarText" + moneyClass }>
                    &euro; {this.props.moneyWonSoFar}
                </div>
            </div>
        );
    }
}

export default connector(MoneyWonSoFar)
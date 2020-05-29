import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import * as BitcoinGameStore from '../store/BitcoinGame';
import RoundManager from "./RoundManager";
import NonPlayerWallet from "./NonPlayerWallet";
import MoneyWonSoFar from "./MoneyWonSoFar";

const connector = connect((state: ApplicationState) => state.bitcoinGame, BitcoinGameStore.actionCreators);
type GameManagerProps = ConnectedProps<typeof connector>

class GameManager extends React.Component<GameManagerProps> {

    public render() {
        if (this.props.currentGameId == null) {
            return (
                <div className="gameManager">
                    <div className="gameStatus">
                        <div className="gameStatusHeader">Spelstatus</div>
                        <div className="gameStatusText error">Nog niet gestart</div>
                    </div>
                    <MoneyWonSoFar />
                </div>
            );
        }

        let gameStatusTag = this.props.gameHasFinished!
            ? <div className="gameStatusText error">Afgelopen</div>
            : <div className="gameStatusText">Gestart</div>;

        return (
            <div className="gameManager">
                <div className="gameStatus">
                    <div className="gameStatusHeader">Spelstatus</div>
                    { gameStatusTag }
                </div>
                <MoneyWonSoFar />
                <div className="nonPlayerWallets">
                    {this.props.nonPlayerWallets.map((wallet, i) => <NonPlayerWallet
                        key = {i}
                        name = {wallet.name}
                        currentBalance = {wallet.currentBalance}
                        address = {wallet.address}
                    />)}
                </div>
                { this.props.gameHasFinished ? null : <RoundManager /> }
            </div>
        );
    }
}

export default connector(GameManager);
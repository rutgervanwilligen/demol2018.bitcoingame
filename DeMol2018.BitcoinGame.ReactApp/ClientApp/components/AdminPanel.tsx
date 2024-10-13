import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import * as AdminPanelStore from '../store/AdminPanel';

const connector = connect((state: ApplicationState) => state.bitcoinGame, AdminPanelStore.actionCreators);
type AdminPanelProps = ConnectedProps<typeof connector>

class AdminPanel extends React.Component<AdminPanelProps> {
    private readonly newRoundLengthInput: React.RefObject<HTMLInputElement>;

    constructor(props: AdminPanelProps) {
        super(props);

        this.newRoundLengthInput = React.createRef();
    }

    public render() {
        if (!this.props.currentGameId) {
            return (
                <div className="adminPanel">
                    <h2>Adminpaneel</h2>
                    <h3>Start nieuw spel</h3>
                    <button className="button" onClick={() => this.props.startNewGame(this.props.playerGuid!)}>Start nieuw spel</button>
                </div>
            );
        }

        if (this.props.gameHasFinished) {
            return (
                <div className="adminPanel">
                    <h2>Adminpaneel</h2>
                    <h3>Game ID: {this.props.currentGameId}</h3>
                    <h3>Aantal gespeelde rondes: {this.props.lastRoundNumber == undefined ? "0" : this.props.lastRoundNumber}</h3>
                    <h2>Jokerwinnaars</h2>
                    <table className="jokerWinnerTable">
                        <tbody>
                            {this.props.jokerWinners!.map((winner, i) =>
                                <tr key={i}>
                                    <td className="jokerWinnerName">{ winner.name }</td>
                                    <td className="jokerWinnerNumberOfJokersWon">{ winner.numberOfJokersWon } joker{ winner.numberOfJokersWon != 1 ? "s" : ""}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                    <h3>Start nieuw spel</h3>
                    <button className="button" onClick={() => this.props.startNewGame(this.props.playerGuid!)}>Start nieuw spel</button>
                </div>
            );
        }

        return (
            <div className="adminPanel">
                <h2>Adminpaneel</h2>
                <h3>Game ID: {this.props.currentGameId}</h3>
                <h3>Start nieuwe ronde</h3>
                <input className="inputField" placeholder='Lengte van nieuwe ronde (min)' ref={this.newRoundLengthInput} />
                <button className="button" onClick={() => this.props.startNewRound(this.props.playerGuid!, +this.newRoundLengthInput.current!.value)}>Start nieuwe ronde</button>
                <h3>Rond huidig spel af</h3>
                <button className="button" onClick={() => this.props.finishCurrentGame(this.props.playerGuid!)}>Rond huidig spel af</button>
            </div>
        );
    }
}

export default connector(AdminPanel);
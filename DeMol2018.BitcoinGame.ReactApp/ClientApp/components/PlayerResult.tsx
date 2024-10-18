import { useSelector } from "react-redux";
import { selectCurrentBalance, selectNumberOfJokersWon } from "../store/bitcoinGame/bitcoinGameSlice";

export const PlayerResult = () => {
    const currentBalance = useSelector(selectCurrentBalance);
    const numberOfJokersWon = useSelector(selectNumberOfJokersWon);

    return (
        <div className="playerResult">
            <h2 className="playerResultHeader">Mijn resultaat</h2>
            <div className="playerResultBalance">
                <div className="playerResultBalanceHeader">Eindsaldo</div>
                <div className="playerResultBalanceText">{ currentBalance } BTC</div>
            </div>
            <div className="jokersWon">
                <div className="jokersWonHeader">Aantal gewonnen jokers</div>
                <div className="jokersWonText">{ numberOfJokersWon }</div>
            </div>
        </div>
    );
}

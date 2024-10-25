import * as React from "react";
import { RoundCountdownTimer } from "./RoundCountdownTimer";
import { useSelector } from "react-redux";
import {
    selectCurrentRoundEndTime,
    selectCurrentRoundNumber,
    selectLastRoundNumber,
} from "../store/bitcoinGame/bitcoinGameSlice";

export const RoundManager = () => {
    const lastRoundNumber = useSelector(selectLastRoundNumber);
    const currentRoundNumber = useSelector(selectCurrentRoundNumber);
    const currentRoundEndTime = useSelector(selectCurrentRoundEndTime);

    if (currentRoundNumber != null && currentRoundEndTime != null) {
        return (
            <div className="roundManager">
                <div className="roundNumber">Ronde {currentRoundNumber}</div>
                <div className="timeLeft">
                    Transacties worden verwerkt over: <RoundCountdownTimer />
                </div>
            </div>
        );
    } else if (lastRoundNumber != null) {
        return (
            <div className="roundManager noRoundActive">
                Geen ronde actief. Laatst gespeelde ronde: {lastRoundNumber}
            </div>
        );
    } else {
        return (
            <div className="roundManager noRoundActive">
                Geen ronde actief. De eerste ronde begint binnenkort.
            </div>
        );
    }
};

import * as React from 'react';
import { useSelector } from "react-redux";
import {
    selectMinutesLeft,
    selectSecondsLeft,
    updateTimeLeft
} from "../store/roundCountdownTimer/roundCountdownTimerSlice";
import { fetchNewGameState, selectCurrentRoundEndTime } from "../store/bitcoinGame/bitcoinGameSlice";
import { useAppDispatch } from "../configureStore";
import { selectPlayerGuid } from "../store/user/userSlice";

export const RoundCountdownTimer = () => {
    let timer: number = 0;

    const minutesLeft = useSelector(selectMinutesLeft);
    const secondsLeft = useSelector(selectSecondsLeft);
    const currentEndTime = useSelector(selectCurrentRoundEndTime);
    const playerGuid = useSelector(selectPlayerGuid);

    const dispatch = useAppDispatch();

    const getUpdatedClockValues = () => {
        let timeDiff = (new Date(currentEndTime!).getTime() - new Date().getTime());
        let totalSecondsLeft = Math.ceil(timeDiff / 1000);

        let minuteDivisor = totalSecondsLeft % (60 * 60);
        let minutesLeft = Math.floor(minuteDivisor / 60);

        let secondDivisor = minuteDivisor % 60;
        let secondsLeft = Math.ceil(secondDivisor);

        return {
            totalSecondsLeft: totalSecondsLeft,
            minutesLeft: minutesLeft,
            secondsLeft: secondsLeft
        };
    };

    const countDown = () => {
        let updatedClockValues = getUpdatedClockValues();

        dispatch(updateTimeLeft({
            minutesLeft: updatedClockValues.minutesLeft,
            secondsLeft: updatedClockValues.secondsLeft
        }));

        if (updatedClockValues.totalSecondsLeft <= 0) {
            clearInterval(timer);

            setTimeout(function () {
                dispatch(fetchNewGameState({
                    playerGuid: playerGuid!
                }));
            }, 1000);
        }
    };

    const startTimer = () => {
        this.timer = window.setInterval(countDown, 1000);
    }

    countDown();
    startTimer();

    return (
        <span className="roundCountdownTimer">
            {
                minutesLeft > 9 ? minutesLeft : '0' + minutesLeft
            }:{
                secondsLeft > 9 ? secondsLeft : '0' + secondsLeft
            }
        </span>
    );
}

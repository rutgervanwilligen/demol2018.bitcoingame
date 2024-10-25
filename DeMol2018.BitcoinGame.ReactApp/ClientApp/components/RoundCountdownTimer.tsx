import * as React from "react";
import { useSelector } from "react-redux";
import {
    selectMinutesLeft,
    selectSecondsLeft,
    updateTimeLeft,
} from "../store/roundCountdownTimer/roundCountdownTimerSlice";
import {
    fetchNewGameState,
    selectCurrentRoundEndTime,
} from "../store/bitcoinGame/bitcoinGameSlice";
import { useAppDispatch } from "../configureStore";
import { selectPlayerGuid } from "../store/user/userSlice";
import { useEffect } from "react";

export const RoundCountdownTimer = () => {
    let interval: number = 0;

    const minutesLeft = Math.max(useSelector(selectMinutesLeft), 0);
    const secondsLeft = Math.max(useSelector(selectSecondsLeft), 0);
    const currentEndTime = useSelector(selectCurrentRoundEndTime);
    const playerGuid = useSelector(selectPlayerGuid);

    const dispatch = useAppDispatch();

    useEffect(() => {
        interval = setInterval(countDown, 1000);
        return () => clearInterval(interval);
    }, []);

    const getUpdatedClockValues = () => {
        const timeDiff =
            new Date(currentEndTime!).getTime() - new Date().getTime();
        const totalSecondsLeft = Math.ceil(timeDiff / 1000);

        const minutesLeft = Math.floor(totalSecondsLeft / 60);
        const secondsLeft = Math.ceil(totalSecondsLeft % 60);

        return {
            totalSecondsLeft: totalSecondsLeft,
            minutesLeft: minutesLeft,
            secondsLeft: secondsLeft,
        };
    };

    const countDown = () => {
        const updatedClockValues = getUpdatedClockValues();

        dispatch(
            updateTimeLeft({
                minutesLeft: updatedClockValues.minutesLeft,
                secondsLeft: updatedClockValues.secondsLeft,
            }),
        );

        if (updatedClockValues.totalSecondsLeft <= 0) {
            clearInterval(interval);

            setTimeout(function () {
                dispatch(
                    fetchNewGameState({
                        playerGuid: playerGuid!,
                    }),
                );
            }, 1000);
        }
    };

    return (
        <span className="roundCountdownTimer">
            {minutesLeft > 9 ? minutesLeft : "0" + minutesLeft}:
            {secondsLeft > 9 ? secondsLeft : "0" + secondsLeft}
        </span>
    );
};

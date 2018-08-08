import * as React from 'react';
import { connect } from 'react-redux';
import * as RoundCountdownTimerStore from '../store/RoundCountdownTimer';
import {ApplicationState} from "../store";

type RoundCountdownTimerProps =
    RoundCountdownTimerStore.RoundCountdownTimerState
    & typeof RoundCountdownTimerStore.actionCreators;

class RoundCountdownTimer extends React.Component<RoundCountdownTimerProps> {
    private timer: number;

    constructor(props: RoundCountdownTimerProps) {
        super(props);

        this.timer = 0;

        this.countDown();

        this.startTimer();
    }

    public render() {
        return (
            <span className="roundCountdownTimer">
                {
                    this.props.minutesLeft > 9 ? this.props.minutesLeft : '0' + this.props.minutesLeft
                }:{
                    this.props.secondsLeft > 9 ? this.props.secondsLeft : '0' + this.props.secondsLeft
                }
            </span>
        );
    }

    getUpdatedClockValues = () => {

        let timeDiff = (this.props.currentEndTime.getTime() - new Date().getTime());
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

    countDown = () => {
        let updatedClockValues = this.getUpdatedClockValues();

        this.props.updateTimeLeft(updatedClockValues.minutesLeft, updatedClockValues.secondsLeft);

        if (updatedClockValues.totalSecondsLeft <= 0) {
            clearInterval(this.timer);

            let fetchNewGameState = this.props.fetchNewGameState;
            let playerGuid = this.props.playerGuid;

            setTimeout(function () {
                fetchNewGameState(playerGuid)
            }, 1000);
        }
    };

    startTimer = () => {
        this.timer = setInterval(this.countDown, 1000);
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.roundCountdownTimer, // Selects which state properties are merged into the component's props
    RoundCountdownTimerStore.actionCreators // Selects which action creators are merged into the component's props
)(RoundCountdownTimer);//as typeof RoundCountdownTimer;
import * as signalR from "@aspnet/signalr";

import { ApplicationState } from "./index";
import { Store } from "redux";

// Declare connection
let connection = new signalR.HubConnection("http://localhost:63426/bitcoinGameHub");
//let connection = new signalR.HubConnection("https://bitcoin.demol2018.nl/bitcoinGameHub");

export function signalRInvokeMiddleware() {
    return (next: any) => async (action: any) => {

        switch (action.type) {
            case "MAKE_TRANSACTION":
                connection.invoke("MakeTransaction", action.receiverId, action.amount, action.amount).then(function () {
                    console.log("make transaction fulfilled");
                }).catch(function () {
                    console.log("make transaction rejected");
                });
                break;

            case "LOGIN":
                connection.invoke("Login", action.name, action.code).then(function () {
                    console.log("login fulfilled");
                }).catch(function () {
                    console.log("login rejected");
                });
                break;
                
            case "START_NEW_ROUND":
                connection.invoke("StartNewRound", action.invokerId, action.lengthOfNewRoundInMinutes).then(function () {
                    
                }).catch(function () {
                    
                });
                break;
            default:
                console.log("Unknown action (" + action.type + "). SignalR hub is not invoked.");
                break;
        }

        return next(action);
    }
}

export function signalRRegisterCommands(store: Store<ApplicationState>) {

    connection.on('LoginResult', data => {
       
        console.log('login result:');
        console.log(data);
        
        store.dispatch({
            type: 'RECEIVE_LOGIN_RESULT',
            loginSuccessful: data.loginSuccessful,
            playerGuid: data.playerGuid,
            usersWalletAddress: data.usersWalletAddress,
            usersCurrentBalance: data.usersCurrentBalance,
            isAdmin: data.isAdmin
        });
    });
    
    connection.on('StartNewRoundResult', data => {

        console.log('start new round result');
        
        store.dispatch({
            type: 'RECEIVE_NEW_ROUND_RESULT',
            newRoundNumber: data.newRoundNumber,
            newRoundEndTime: data.newRoundEndTime
        });
    });

    connection.start();

}

import * as signalR from "@aspnet/signalr";

import { ApplicationState, reducers } from "./index";
import { Store } from "redux";
import {CaseInsensitiveMap} from "awesome-typescript-loader/dist/checker/fs";

// Declare connection
let connection = new signalR.HubConnection("http://localhost:63426/bitcoinGameHub");
//let connection = new signalR.HubConnection("https://bitcoin.demol2018.nl/bitcoinGameHub");

export function signalRInvokeMiddleware(store: any) {
    return (next: any) => async (action: any) => {

        console.log(action);
        console.log('middleware update -- ' + action.type);
        
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
        }

        return next(action);
    }
}

export function signalRRegisterCommands(store: Store<ApplicationState>) {
    
    connection.on('IncrementCounter', data => {
    
        console.log('increment');
        
    });
    
    connection.on('LoginResult', data => {
       
        console.log('login result: ' + data);
        
        store.dispatch({
            type: 'RECEIVE_LOGIN_RESULT',
            loginSuccessful: data.loginSuccessful,
            playerGuid: data.playerGuid
        });
    });
    
    connection.on('RoundUpdate', data => {

        console.log('middleware dispatch');
        
        store.dispatch({
            type: 'ROUND_UPDATE',
            roundData: data
        });
    });

    connection.start();

}

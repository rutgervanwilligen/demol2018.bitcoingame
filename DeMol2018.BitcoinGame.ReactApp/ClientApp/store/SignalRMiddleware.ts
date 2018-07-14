import * as signalR from "@aspnet/signalr";

import { ApplicationState, reducers } from "./index";
import { Store } from "redux";

// Declare connection
let connection = new signalR.HubConnection("http://localhost:63426/bitcoinGameHub");
//let connection = new signalR.HubConnection("https://bitcoin.demol2018.nl/bitcoinGameHub");

export function signalRInvokeMiddleware(store: any) {
    return (next: any) => async (action: any) => {

        console.log(action);
        console.log('middleware update -- ' + action.type);
        
        switch (action.type) {
        case "MAKE_TRANSACTION":
            console.log("lekker invoken");
            connection.invoke("MakeTransaction", action.receiverId, action.amount, action.amount).then(function () {
                console.log("fulfilled");
            }).catch(function () {
                console.log("rejected");
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
    
    connection.on('RoundUpdate', data => {

        console.log('middleware dispatch');
        
        store.dispatch({
            type: 'ROUND_UPDATE',
            roundData: data
        });
    });

    connection.start();

}

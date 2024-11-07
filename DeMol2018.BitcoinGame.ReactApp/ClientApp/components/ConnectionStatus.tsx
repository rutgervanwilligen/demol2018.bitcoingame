import * as React from "react";
import { useSelector } from "react-redux";
import {
    selectConnectionStatus,
    WebsocketConnectionStatus,
} from "../store/websocketConnection/websocketConnectionSlice";

export const ConnectionStatus = () => {
    const connectionStatus = useSelector(selectConnectionStatus);
    const readableStatus = getReadableStatusElement(connectionStatus);

    return <div className="connectionStatus">{readableStatus}</div>;
};

const getReadableStatusElement = (
    status: WebsocketConnectionStatus,
): React.JSX.Element => {
    switch (status) {
        case WebsocketConnectionStatus.Reconnecting:
            return <span className={"reconnecting"}>Opnieuw verbinden...</span>;
        case WebsocketConnectionStatus.Disconnected:
            return <span className={"disconnected"}>Verbinding verbroken</span>;
        case WebsocketConnectionStatus.ConnectionError:
            return <span className={"disconnected"}>Fout bij verbinden met server</span>;
        case WebsocketConnectionStatus.Connected:
            return <span className={"connected"}>Verbonden met server</span>;
        case WebsocketConnectionStatus.NotConnected:
        default:
            return (
                <span className={"disconnected"}>
                    Niet verbonden met server
                </span>
            );
    }
};

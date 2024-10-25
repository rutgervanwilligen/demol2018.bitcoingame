import * as React from "react";
import { useSelector } from "react-redux";
import { selectIsConnected } from "../store/websocketConnection/websocketConnectionSlice";

export const ConnectionStatus = () => {
    const isConnected = useSelector(selectIsConnected);

    return (
        <div className="connectionStatus">
            {isConnected ? (
                <span className={"connected"}>Verbonden met server</span>
            ) : (
                <span className={"disconnected"}>Verbinding verbroken</span>
            )}
        </div>
    );
};

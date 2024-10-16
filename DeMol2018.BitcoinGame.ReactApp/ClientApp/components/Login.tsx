import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import * as LoginStore from '../store/Login';
import { useState } from "react";
import { useAppDispatch } from "../configureStore";

const connector = connect((state: ApplicationState) => state.bitcoinGame, LoginStore.actionCreators);
type LoginProps = ConnectedProps<typeof connector>

const Login2 = (loginProps: LoginProps) => {
    const [username, setUsername] = useState('');
    const [code, setCode] = useState('');
    const dispatch = useAppDispatch();

    return <div className="login">
        <h2>Login</h2>
        <input className="inputField" placeholder='Naam' value={username} onChange={e => setUsername(e.target.value)} />
        <input className="inputField" placeholder='Code' value={code} onChange={e => setCode(e.target.value)} />
        <button onClick={() => dispatch(login(username,  code))}>Login</button>
    </div>;
}

export default connector(Login2);

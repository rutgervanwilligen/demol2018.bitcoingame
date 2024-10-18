import * as React from 'react';
import { useState } from "react";
import { useAppDispatch } from "../configureStore";
import { login } from "../store/user/userSlice";

export const Login = () => {
    const [username, setUsername] = useState('');
    const [code, setCode] = useState('');
    const dispatch = useAppDispatch();

    return <div className="login">
        <h2>Login</h2>
        <input className="inputField" placeholder='Naam' value={username} onChange={e => setUsername(e.target.value)} />
        <input className="inputField" placeholder='Code' type={"number"} value={code} onChange={e => setCode(e.target.value)} />
        <button onClick={() => dispatch(login({ name: username, code: parseInt(code) }))}>Login</button>
    </div>;
}

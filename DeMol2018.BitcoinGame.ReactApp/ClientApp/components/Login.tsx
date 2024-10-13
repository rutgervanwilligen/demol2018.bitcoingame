import * as React from 'react';
import {connect, ConnectedProps} from 'react-redux';
import { ApplicationState }  from '../store';
import * as LoginStore from '../store/Login';

const connector = connect((state: ApplicationState) => state.bitcoinGame, LoginStore.actionCreators);
type LoginProps = ConnectedProps<typeof connector>

class Login extends React.Component<LoginProps> {
    private readonly setNameInput: React.RefObject<HTMLInputElement>;
    private readonly setCodeInput: React.RefObject<HTMLInputElement>;

    constructor(props: LoginProps) {
        super(props);

        this.setNameInput = React.createRef();
        this.setCodeInput = React.createRef();
    }

    public render() {
        return <div className="login">
                    <h2>Login</h2>
                    <input className="inputField" placeholder='Naam' ref={this.setNameInput} />
                    <input className="inputField" placeholder='Code' ref={this.setCodeInput} />
                    <button onClick={() => this.props.login(this.setNameInput.current!.value, this.setCodeInput.current!.value)}>Login</button>
                </div>;
    }
}

export default connector(Login);
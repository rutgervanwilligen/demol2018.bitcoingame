import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState }  from '../store';
import * as LoginStore from '../store/Login';
import * as BitcoinGameStore from '../store/BitcoinGame';

type LoginProps =
    BitcoinGameStore.BitcoinGameState
    & typeof LoginStore.actionCreators;

class Login extends React.Component<LoginProps> {
    private nameInput: HTMLInputElement;
    private codeInput: HTMLInputElement;

    setNameInputRef = (element: HTMLInputElement) => {
        this.nameInput = element;
    };

    setCodeInputRef = (element: HTMLInputElement) => {
        this.codeInput = element;
    };

    constructor(props: LoginProps) {
        super(props);

        this.setNameInputRef = this.setNameInputRef.bind(this);
        this.setCodeInputRef = this.setCodeInputRef.bind(this);
    }

    public render() {
        return  <div className="login">
                    <h2>Login</h2>
                    <input className="inputField" placeholder='Naam' ref={this.setNameInputRef} />
                    <input className="inputField" placeholder='Code' ref={this.setCodeInputRef} />
                    <button onClick={() => this.props.login(this.nameInput.value, this.codeInput.value)}>Login</button>
                </div>;
    }
}

// Wire up the React component to the Redux store
export default connect(
    (state: ApplicationState) => state.bitcoinGame, // Selects which state properties are merged into the component's props
    LoginStore.actionCreators // Selects which action creators are merged into the component's props
)(Login);//as typeof Login;
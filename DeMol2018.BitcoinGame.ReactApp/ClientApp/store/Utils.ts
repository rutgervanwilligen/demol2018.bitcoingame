import { JokerWinner, NonPlayerWalletState } from "./bitcoinGame/bitcoinGameSlice";

export const sortWallets = (wallets: NonPlayerWalletState[]): NonPlayerWalletState[] => {
    return wallets.sort((a: NonPlayerWalletState, b: NonPlayerWalletState) => {
        return a.address - b.address
    });
}

export const sortJokerWinners = (wallets: JokerWinner[]): JokerWinner[] => {
    return wallets.sort((a: JokerWinner, b: JokerWinner) => {
        let nameA = a.name.toLowerCase();
        let nameB = b.name.toLowerCase();

        if (nameA < nameB) {
            return -1;
        }

        if (nameB < nameA) {
            return 1;
        }

        return 0;
    });
}

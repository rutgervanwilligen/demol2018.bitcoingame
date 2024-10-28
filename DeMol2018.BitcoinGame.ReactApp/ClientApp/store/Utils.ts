import { JokerWinner, NonPlayerWallet } from "./bitcoinGame/bitcoinGameSlice";

export const sortWallets = (wallets: NonPlayerWallet[]): NonPlayerWallet[] => {
    return wallets.sort((a: NonPlayerWallet, b: NonPlayerWallet) => {
        return a.address - b.address;
    });
};

export const sortJokerWinners = (wallets: JokerWinner[]): JokerWinner[] => {
    return wallets.sort((a: JokerWinner, b: JokerWinner) => {
        const nameA = a.name.toLowerCase();
        const nameB = b.name.toLowerCase();

        if (nameA < nameB) {
            return -1;
        }

        if (nameB < nameA) {
            return 1;
        }

        return 0;
    });
};

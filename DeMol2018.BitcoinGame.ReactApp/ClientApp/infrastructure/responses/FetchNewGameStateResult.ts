import {
    JokerWinner,
    NonPlayerWallet,
} from "../../store/bitcoinGame/bitcoinGameSlice";

export const FetchNewGameStateResultHubMethod = "FetchNewGameStateResult";

export interface FetchNewGameStateResult {
    callSuccessful: boolean;
    updatedState: UpdatedStateResult;
}

export interface UpdatedStateResult {
    currentGameId: string;
    lastRoundNumber?: number;
    currentRoundNumber?: number;
    currentRoundEndTime?: string;
    currentBalance?: number;
    userWalletAddress?: number;
    userCurrentBalance?: number;
    nonPlayerWallets: NonPlayerWalletResult[];
    jokerWinners: JokerWinnerResult[];
    moneyWonSoFar: number;
    gameHasFinished: boolean;
    numberOfJokersWon: number;
}

export interface NonPlayerWalletResult {
    name: string;
    address: number;
    currentBalance: number;
}

export interface JokerWinnerResult {
    name: string;
    numberOfJokersWon: number;
}

export const MapNonPlayerWalletToDomain = (
    result: NonPlayerWalletResult,
): NonPlayerWallet => {
    return {
        address: result.address,
        name: result.name,
        currentBalance: result.currentBalance,
    };
};

export const MapJokerWinnerToDomain = (
    result: JokerWinnerResult,
): JokerWinner => {
    return {
        name: result.name,
        numberOfJokersWon: result.numberOfJokersWon,
    };
};

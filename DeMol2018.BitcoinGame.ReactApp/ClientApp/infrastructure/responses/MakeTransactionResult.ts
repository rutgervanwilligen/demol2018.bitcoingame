export const MakeTransactionResultHubMethod = "MakeTransactionResult";

export interface MakeTransactionResult {
    transactionSuccessful: boolean;
    userCurrentBalance: number;
}

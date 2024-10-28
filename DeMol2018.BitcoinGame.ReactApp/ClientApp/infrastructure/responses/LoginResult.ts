export const LoginResultHubMethod = "LoginResult";

export interface LoginResult {
    loginSuccessful: boolean;
    playerGuid?: string;
    isAdmin: boolean;
}

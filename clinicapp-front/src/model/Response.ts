export interface ResponseModel {
    isSuccess: boolean,
    error: string | null,
}

export interface ValidationResponseModel extends ResponseModel {
    validationErrors?: {[key: string]: string[]}
}

export interface LoginResponse extends ResponseModel {
    userId: string,
    claims: string[],
    authToken: string
}
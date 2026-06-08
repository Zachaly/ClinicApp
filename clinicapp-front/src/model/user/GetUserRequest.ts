import type PagedRequest from "../PagedRequest";

export default interface GetUserRequest extends PagedRequest {
    LastName?: string,
    FirstName?: string,
    UserName?: string
}
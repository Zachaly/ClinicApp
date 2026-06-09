import type PagedRequest from "../PagedRequest";

export default interface GetPatientRequest extends PagedRequest {
    PeselNumber?: string,
    LastName?: string
}
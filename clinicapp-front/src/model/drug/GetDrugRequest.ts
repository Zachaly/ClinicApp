import type PagedRequest from "../PagedRequest";

export default interface GetDrugRequest extends PagedRequest {
    MaxPrice?: number,
    BrandName?: string,
    BrandNameExact?: string,
    GenericName?: string,
    ClassId?: string
}
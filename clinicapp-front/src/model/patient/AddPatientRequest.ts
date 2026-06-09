export default interface AddPatientRequest {
    peselNumber: string,
    firstName: string,
    lastName: string,
    birthDate: string,
    address: string,
    postalCode: string,
    city: string
}
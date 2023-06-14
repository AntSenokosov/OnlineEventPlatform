export interface User{
    id : number;
    email : string;
    token : string;
    tokenValidTo : Date;
    firstName : string;
    lastName : string;
    isAdmin : boolean;
    isSuperAdmin : boolean;
    hasTwoFactoryAuthEnable : boolean;
}
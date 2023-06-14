import { inject } from "@angular/core";
import { ApiService } from "../core/services/api.service";
import { Observable, map } from "rxjs";
import { User } from "../auth/user.model";
import { GenearateTwoFactory } from "./dtos/generatetwofactory.request";
import { VerifyTwoFactoryRequest } from "./dtos/verifytwofactory.request";
import { DisableTwoFactoryRequest } from "./dtos/disabletwofactory.request";
import { GenearateTwoFactoryResponse } from "./dtos/generatetwofactore.response";

export class ProfileService{
    private readonly endpoint = "profile";

    private readonly apiService = inject(ApiService);

    getProfile() : Observable<User>
    {
        return this.apiService.get(this.endpoint);
    }

    updateProfile(user : User) : Observable<User>
    {
        return this.apiService.put(`${this.endpoint}/update`, user);
    }

    deleteUser() : Observable<any>
    {
        return this.apiService.delete(`${this.endpoint}/delete`);
    }

    generateTwoFactoryAuth(request : GenearateTwoFactory) : Observable<GenearateTwoFactoryResponse>
    {
        return this.apiService.post(`${this.endpoint}/generateTwoFactory`, request);
    }

    verifyTwoFactory(request: VerifyTwoFactoryRequest) : Observable<User>
    {
        return this.apiService.post(`${this.endpoint}/verify`, request);
    }

    disableTwoFactory(request : DisableTwoFactoryRequest) : Observable<User>
    {
        return this.apiService.put(`${this.endpoint}/disable`, request);
    }
}
import { inject } from "@angular/core";
import { Observable, map } from "rxjs";
import { RegisterRequest } from "src/app/auth/dtos/registerrequest";
import { User } from "src/app/auth/user.model";
import { ApiService } from "src/app/core/services/api.service";

export class UsersService{
    private readonly apiService = inject(ApiService);
    private readonly endpoint = "users";

    getUsers() : Observable<User[]>
    {
        return this.apiService.get(this.endpoint).pipe(
            map((response: any) => response.items as User[])
          );
    }

    createUser(registerRequest : RegisterRequest) : Observable<User>
    {
        return this.apiService.post(`${this.endpoint}/create`, registerRequest);
    }

    updateToAdmin(id : number) : Observable<User>
    {
        return this.apiService.get(`${this.endpoint}/${id}/toAdmin`)
    }

    updateToUser(id : number) : Observable<User>
    {
        return this.apiService.get(`${this.endpoint}/${id}/toUser`)
    }

    deleteUser(id : number) : Observable<any>
    {
        return this.apiService.delete(`${this.endpoint}/${id}/delete`);
    }
}
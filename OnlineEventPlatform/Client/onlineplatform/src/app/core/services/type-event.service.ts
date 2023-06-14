import { Injectable, inject } from "@angular/core";
import { ApiService } from "./api.service";
import { Observable, map } from "rxjs";
import { TypeEvent } from "../dtos/type-event.model";

@Injectable()
export class TypeEventService {
    private readonly apiService = inject(ApiService);
    private readonly endpoint = "types";

    getTypes() : Observable<TypeEvent[]>
    {
        return this.apiService.get(this.endpoint).pipe(
            map((response : any) => response.items as TypeEvent[])
        );
    }
}
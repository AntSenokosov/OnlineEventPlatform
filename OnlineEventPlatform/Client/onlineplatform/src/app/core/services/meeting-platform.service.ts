import { Injectable, inject } from "@angular/core";
import { ApiService } from "./api.service";
import { Observable, map } from "rxjs";
import { MeetingPlatform } from "../dtos/meeting-platform.model";

@Injectable()
export class MeetingPlatformService{
    private readonly apiService = inject(ApiService);
    private readonly endpoint = "platforms";


    public getEvents() : Observable<MeetingPlatform[]>
    {
        return this.apiService.get(this.endpoint).pipe(
            map((response : any) => response.items as MeetingPlatform[])
        );
    }
}
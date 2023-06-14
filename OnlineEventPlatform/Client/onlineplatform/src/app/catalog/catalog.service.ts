import { OnInit, inject } from "@angular/core";
import { ApiService } from "../core/services/api.service";
import { Observable, map } from "rxjs";
import { OnlineEvent } from "../adminpanel/onlineevent/onlineevent.model";
import { GetEvent } from "./dtos/getevent.response";

export class CatalogService{
    private readonly endpoint = "catalog";
    private apiService = inject(ApiService);

    getCatalog(): Observable<OnlineEvent[]> {
        return this.apiService.get(this.endpoint).pipe(
          map((response: any) => response as OnlineEvent[])
        );
      }

      getEvent(id: number): Observable<GetEvent> {
        return this.apiService.get(`${this.endpoint}/${id}`).pipe(
          map((response: any) => response.item as GetEvent)
        );
      }
}
import { Injectable, inject } from "@angular/core";
import { ApiService } from "../core/services/api.service";
import { Observable, map, throwError, catchError, tap } from "rxjs";
import { OnlineEvent } from "../adminpanel/onlineevent/onlineevent.model";
import { UserEventRequest } from "./userevent.request";

@Injectable()
export class UserEventsService {
    private readonly endpoint = "userevents";
  
    constructor(private readonly apiService: ApiService) {}
  
    getEvents(): Observable<OnlineEvent[]> {
      return this.apiService.get(this.endpoint).pipe(
        map((response: any) => response.items as OnlineEvent[])
      );
    }
  
    registerEvent(eventId: number): Observable<number> {
        const request: UserEventRequest = {
            eventId: eventId
          };
      return this.apiService.post(`${this.endpoint}/add`, request).pipe(
        map((response: any) => {
          if (response && response.id) {
            return response.id as number;
          } else {
            throw new Error('Invalid response format');
          }
        })
      );
    }
  
    deleteUserEvent(eventId: number): Observable<any> {
        const request: UserEventRequest = {
            eventId: eventId
          };
      return this.apiService.post(`${this.endpoint}/delete`, request);
    }
  
    checkEventForUser(eventId: number): Observable<boolean> {
        const request: UserEventRequest = {
          eventId: eventId
        };
        return this.apiService.post(`${this.endpoint}/check`, request).pipe(
          tap((response: any) => {
            console.log('Response from server:', response);
          }),
          map((response: any) => {
            if (typeof response === 'boolean') {
              return response as boolean;
            } else {
              throw new Error('Invalid response format');
            }
          }),
          catchError((error: any) => {
            console.error('Error from server:', error);
            throw new Error('Internal Server Error');
          })
        );
      }
  }
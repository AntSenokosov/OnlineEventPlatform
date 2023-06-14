import { inject } from "@angular/core";
import { Observable, map } from "rxjs";
import { ApiService } from "src/app/core/services/api.service";
import { OnlineEvent } from "./onlineevent.model";

export class OnlineEventService{
    private readonly endpoint = "onlineevents";
    private apiService = inject(ApiService);

    getEvents() : Observable<OnlineEvent[]>
    {
        return this.apiService.get(this.endpoint).pipe(
            map((response : any) => response.items as OnlineEvent[])
        );
    }

    getEvent(id : number) : Observable<OnlineEvent>{
        return this.apiService.get(`${this.endpoint}/${id}`);
    }

    addEvent(onlineevent : OnlineEvent) : Observable<OnlineEvent>
    {
        console.error(`
        Service:
        type: ${onlineevent?.type}
        name: ${onlineevent.name}
        desc: ${onlineevent.description}
        date: ${onlineevent.date}
        time: ${onlineevent.time}
        about: ${onlineevent.aboutEvent}
        photo: ${onlineevent.photo}
        speakers: ${onlineevent.speakers}
        platform: ${onlineevent.platform}
        link: ${onlineevent.link}
        meetingId: ${onlineevent.meetingId}
        password: ${onlineevent.password}
      `);
        return this.apiService.post(`${this.endpoint}/create`, onlineevent);
    }
    
    updateEvent(id : number, onlineevent : OnlineEvent) : Observable<OnlineEvent>
    {
        return this.apiService.put(`${this.endpoint}/${id}/update`, onlineevent);
    }

    deleteEvent(id : number) : Observable<any>
    {
        return this.apiService.delete(`${this.endpoint}/${id}/remove`);
    }
}
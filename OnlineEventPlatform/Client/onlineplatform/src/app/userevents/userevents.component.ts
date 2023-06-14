import { Component, OnInit, inject } from '@angular/core';
import { OnlineEvent } from '../adminpanel/onlineevent/onlineevent.model';
import { UserEventsService } from './userevents.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-userevents',
  templateUrl: './userevents.component.html',
  styleUrls: ['./userevents.component.css']
})
export class UsereventsComponent implements OnInit {
  events : OnlineEvent[] = [];

  private readonly userEventsService = inject(UserEventsService);
  private readonly router = inject(Router);
  ngOnInit(): void {
    this.getEvents();
  }

  getEvents() : void
  {
    this.userEventsService.getEvents().subscribe(
      (events: OnlineEvent[]) =>{
        this.events = events;
      },
      (error: any) => {
        console.error("Failed to load events", error);
      }
    );
  }

  onEventClicked(id : number)
  {
    this.router.navigate(['catalog/event', id]);
  }

  deleteUserEvent(id : number)
  {
    this.userEventsService.deleteUserEvent(id).subscribe(
      () =>{
        this.getEvents()
      },
      (error: any) => {
      console.error('Failed to delete event:', error);
      }
      );
  }
}

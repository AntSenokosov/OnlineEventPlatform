import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CatalogService } from '../catalog.service';
import { GetEvent } from '../dtos/getevent.response';
import { UserEventsService } from 'src/app/userevents/userevents.service';
import { AuthService } from 'src/app/auth/auth.service';
import { catchError, tap } from 'rxjs';

@Component({
  selector: 'app-event-page',
  templateUrl: './event-page.component.html',
  styleUrls: ['./event-page.component.css']
})
export class EventPageComponent implements OnInit {
  eventId!: number;
  eventDetail!: GetEvent;
  isLogged: boolean = false;
  isUserRegister: boolean = false;

  constructor(
    private catalogService: CatalogService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private userEventService: UserEventsService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.eventId = params['id'];
      this.loadDetail();
      this.isLogged = this.isLoggedIn();
      this.checkUserEvent(this.eventId);
    });
  }

  loadDetail(): void {
    this.catalogService.getEvent(this.eventId).subscribe(
      (event: GetEvent) => {
        this.eventDetail = event;
      },
      (error: any) => {
        console.error('Failed to load event details', error);
      }
    );
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  registerEvent(id: number): void {
    this.userEventService.registerEvent(id).subscribe(
      (response: number) => {
        this.ngOnInit();
        console.log("Id = ", response);
      },
      (error: any) => {
        console.error('Failed to register event', error);
      }
    );
  }

  checkUserEvent(id: number): void {
    this.userEventService.checkEventForUser(id).subscribe(
      (exists: boolean) => {
        this.isUserRegister = exists;
      },
      (error: any) => {
        console.error('Failed to check user event', error);
      }
    );
  }

  unregisterEvent(id: number): void {
    this.userEventService.deleteUserEvent(id).subscribe(
      (response: number) => {
        this.ngOnInit();
        console.log("Id = ", response);
      },
      (error: any) => {
        console.error('Failed to register event', error);
      }
    );
  }
}
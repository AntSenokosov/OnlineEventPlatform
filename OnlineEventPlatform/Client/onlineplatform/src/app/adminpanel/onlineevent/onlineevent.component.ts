import { Component, OnInit, inject } from '@angular/core';
import { OnlineEvent } from './onlineevent.model';
import { OnlineEventService } from './onlineevent.service';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Speaker } from '../speaker/speaker.model';
import { MeetingPlatform } from 'src/app/core/dtos/meeting-platform.model';
import { MeetingPlatformService } from 'src/app/core/services/meeting-platform.service';
import { SpeakerService } from '../speaker/speaker.service';
import { TypeEvent } from 'src/app/core/dtos/type-event.model';
import { TypeEventService } from 'src/app/core/services/type-event.service';

@Component({
  selector: 'app-onlineevent',
  templateUrl: './onlineevent.component.html',
  styleUrls: ['./onlineevent.component.css']
})
export class OnlineeventComponent implements OnInit{
  public gridData!: Observable<OnlineEvent[]>;

  public eventToUpdate: OnlineEvent | null = null;
  public eventToDelete: OnlineEvent | null = null;

  public isDeleteDialog = false;
  public isCreateDialog = false;
  public isSingleDialog = false;
  public isMultipleDialog = false;

  public eventForm: FormGroup | undefined;

  public speakersData: Speaker[] = [];

  public platformsData: MeetingPlatform[] = [];
  public selectedPlatform: MeetingPlatform | null = null;

  public typesData: TypeEvent[] = [];
  public selectedType: TypeEvent | null = null;

  public selectedSpeakers: Speaker[] = [];

  constructor(
    private eventService: OnlineEventService,
    private platformService: MeetingPlatformService,
    private speakerService: SpeakerService,
    private typesService: TypeEventService
  ) {}

  ngOnInit(): void {
    this.gridData = this.eventService.getEvents();
    this.loadData();
  }

  public onPlatformChange(event: any) {
    this.selectedPlatform = event;
  }

  public onTypeChange(event: any) {
    this.selectedType = event;
  }

  loadData(): void {
    
    this.platformService.getEvents().subscribe(
      (platforms: MeetingPlatform[]) => {
        this.platformsData = platforms;
        this.selectedPlatform = this.platformsData[0];
      },
      (error: any) => {
        console.error('Failed to load meeting platforms:', error);
      }
    );

    this.speakerService.getSpeakers().subscribe(
      (speakers: Speaker[]) => {
        this.speakersData = speakers;
      },
      (error: any) => {
        console.error('Failed to load speakers:', error);
      }
    );

    this.typesService.getTypes().subscribe(
      (types: TypeEvent[]) => {
        this.typesData = types;
        this.selectedType = this.typesData[0];
      },
      (error: any) => {
        console.error('Failed to load event types:', error);
      }
    );
    
/*
    this.platformsData = [
      {id:1, name: "Test1", url:""},
      {id:2, name: "Test2", url:""},
      {id:3, name: "Test3", url:""},
      {id:4, name: "Test4", url:""},
      {id:5, name: "Test5", url:""},
    ];

    this.typesData = [
      {id:1, name: "Type1"},
      {id:2, name: "Type2"},
      {id:3, name: "Type3"}
    ];

    this.speakersData = [
      {id : 1, firstName: "One1", lastName: "Two1", position: null, shortDescription: null, longDescription: null},
      {id : 2, firstName: "One2", lastName: "Two2", position: null, shortDescription: null, longDescription: null},
      {id : 3, firstName: "One3", lastName: "Two3", position: null, shortDescription: null, longDescription: null},
      {id : 4, firstName: "One4", lastName: "Two4", position: null, shortDescription: null, longDescription: null},
      {id : 5, firstName: "One5", lastName: "Two5", position: null, shortDescription: null, longDescription: null}
    ];*/
  }

  addHandler() {
    this.eventForm = new FormGroup({
      type: new FormControl(null),
      name: new FormControl(null),
      description: new FormControl(null),
      date: new FormControl(new Date()),
      time: new FormControl(null),
      aboutEvent: new FormControl(null),
      photo: new FormControl(null),
      speakers: new FormControl(null),
      platform: new FormControl(null),
      link: new FormControl(null),
      meetingId: new FormControl(null),
      password: new FormControl(null)
    });

    this.isCreateDialog = true;
  }

  updateHandler(event: { dataItem: OnlineEvent }) {
    this.eventToUpdate = event.dataItem;
    this.eventForm = new FormGroup({
      type: new FormControl(this.eventToUpdate.type, Validators.required),
      name: new FormControl(this.eventToUpdate.name, Validators.required),
      description: new FormControl(this.eventToUpdate.description, Validators.required),
      date: new FormControl(this.eventToUpdate.date, Validators.required),
      time: new FormControl(this.eventToUpdate.time, Validators.required),
      aboutEvent: new FormControl(this.eventToUpdate.aboutEvent, Validators.required),
      photo: new FormControl(this.eventToUpdate.photo),
      speakers: new FormControl(this.eventToUpdate.speakers),
      platform: new FormControl(this.eventToUpdate.platform, Validators.required),
      link: new FormControl(this.eventToUpdate.link),
      meetingId: new FormControl(this.eventToUpdate.meetingId),
      password: new FormControl(this.eventToUpdate.password)
    });

    this.isCreateDialog = true;
  }

  removeHandler(event: { dataItem: OnlineEvent }) {
    this.isDeleteDialog = true;
    this.eventToDelete = event.dataItem;
  }

  handlePhotoUpload(event: Event) {}

  closeDeleteDialog() {
    this.isDeleteDialog = false;
  }

  closeCreateDialog() {
    this.isCreateDialog = false;
  }

  onFormSubmit() {
    if (this.eventForm && this.eventForm.valid) {
      const eventModel: OnlineEvent = this.eventForm.value;
      
const ids : number[] = [];
this.selectedSpeakers.forEach((speaker) => {
  ids.push(speaker.id);
});

eventModel.type = this.selectedType!.id;
eventModel.speakers = ids;
eventModel.time = (new Date(eventModel.time).toLocaleTimeString('en-US', { hour12: false }));
eventModel.platform = this.selectedPlatform!.id;

      console.warn(`
        selectedPlatform: ${this.selectedPlatform!.id}
      `);

      if (!this.eventToUpdate) {
        
        this.eventService.addEvent(eventModel).subscribe(
          () => {
            this.ngOnInit();
          },
          (error: any) => {
            console.error('Failed to add event:', error);
          }
        );
      } else {
        const id = this.eventToUpdate.id;
        this.eventService.updateEvent(id, eventModel).subscribe(
          () => {
            this.ngOnInit();
          },
          (error: any) => {
            console.error('Failed to update event:', error);
          }
        );
      }
    }

    this.isCreateDialog = false;
    this.eventToUpdate = null;
    this.eventForm = undefined;
  }

  deleteEvent() {
    if (this.eventToDelete) {
      this.eventService.deleteEvent(this.eventToDelete.id).subscribe(
        () => {
          this.ngOnInit();
        },
        (error: any) => {
          console.error('Failed to delete event:', error);
        }
      );
    }

    this.isDeleteDialog = false;
  }
}
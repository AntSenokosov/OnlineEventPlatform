import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Speaker } from './speaker.model';
import { SpeakerService } from './speaker.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject, map, takeUntil } from 'rxjs';

@Component({
selector: 'app-speaker',
templateUrl: './speaker.component.html',
styleUrls: ['./speaker.component.css']
})
export class SpeakerComponent implements OnInit {

  public gridData!: Observable<Speaker[]>;

  public speakerToUpdate: Speaker | null = null;
  public speakerToDelete: Speaker | null = null;

  public isDeleteDialog = false;
  public isCreateDialog = false;

  speakerForm: FormGroup | undefined;

  constructor(private readonly speakerService:SpeakerService) {}

  ngOnInit(): void {
    this.gridData = this.speakerService.getSpeakers();
  }

addHandler(){
    this.speakerForm = new FormGroup({
        firstName : new FormControl('', Validators.required),
        lastName : new FormControl('', Validators.required),
        position : new FormControl(''),
        shortDescription : new FormControl(''),
        longDescription : new FormControl('')
    });

    this.isCreateDialog = true;
}

updateHandler(event: { dataItem: Speaker; }){
    this.speakerToUpdate = event.dataItem;
    this.speakerForm = new FormGroup({
        firstName : new FormControl(this.speakerToUpdate.firstName, Validators.required),
        lastName : new FormControl(this.speakerToUpdate.lastName, Validators.required),
        position : new FormControl(this.speakerToUpdate.position),
        shortDescription : new FormControl(this.speakerToUpdate.shortDescription),
        longDescription : new FormControl(this.speakerToUpdate.longDescription)
    });

    this.isCreateDialog = true;
}

removeHandler(event: { dataItem: Speaker; })
{
    this.isDeleteDialog = true;
    this.speakerToDelete = event.dataItem;
}

deleteSpeaker()
{
    this.speakerService.deleteSpeaker(this.speakerToDelete!.id).subscribe(
        () =>{
            this.ngOnInit();
        },
        (error: any) => {
            console.error('Failed to delete speaker:', error);
        }
    );
    this.isDeleteDialog = false;
}

closeSpeakerCreation()
{
    this.isCreateDialog = false;
    this.speakerForm = undefined;
}

closeDeletionDialog()
{
    this.isDeleteDialog = false;
}

onFormSubmit()
{
    if (this.speakerForm!.valid)
    {
        const speakerModel : Speaker = this.speakerForm!.value;

        if (!this.speakerToUpdate)
        {
            this.speakerService.addSpeaker(speakerModel).subscribe(
                () =>{
                    this.ngOnInit();
                },
                (error: any) => {
                    console.error('Failed to add speaker:', error);
                });
        }
        else
        {
            const id = this.speakerToUpdate.id;
            this.speakerService.updateSpeaker(id, speakerModel).subscribe(
                () =>{
                    this.ngOnInit();
                },
                (error: any) => {
                    console.error('Failed to update speaker:', error);
                });
        }
    }

    this.isCreateDialog = false;
    this.speakerToUpdate = null;
    this.speakerForm = undefined;
}
}
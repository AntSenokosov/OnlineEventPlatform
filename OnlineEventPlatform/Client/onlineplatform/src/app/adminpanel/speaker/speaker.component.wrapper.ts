import { Component, OnDestroy, OnInit, ViewChild, inject } from "@angular/core";
import { Observable, Subject, catchError, takeUntil } from "rxjs";
import { SpeakerService } from "./speaker.service";
import { Speaker } from "./speaker.model";
import { ErrorMessageDialogComponent } from "src/app/shared/components/errors/error-message-dialog/error-message-dialog.component";

@Component({
  selector: 'app-speaker-wrapper',
  template: `
  <app-speaker
  ></app-speaker>
  <app-error-message-dialog #errorDialog></app-error-message-dialog>`
})

export class SpeakerComponentWrapper implements OnInit, OnDestroy
{
    @ViewChild('errorDialog') errorDialog!: ErrorMessageDialogComponent;
    
    public speakers$!: Observable<Speaker[]>;

    constructor(private speakerService: SpeakerService) {}
    private destroy$: Subject<boolean> = new Subject<boolean>();

    ngOnInit(): void {
      this.loadData();
    }
    
    ngOnDestroy(): void {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }

    loadData() : void{
      console.error("SpeakerWrapper");
      this.speakers$ = this.speakerService.getSpeakers().pipe(
        catchError(err => {
          this.errorDialog.openError(err.message);
          throw new Error(err);
        })
      );
    }
    
    onSpeakerCreated(speaker : Speaker)
    {
        this.speakerService.addSpeaker(speaker).pipe(
            takeUntil(this.destroy$),
            catchError(err => {
              this.errorDialog.openError(err.message);
              throw new Error(err);
            })
          ).subscribe();
    }

    onSpeakerUpdate(speaker: {speakerModel : Speaker, id : number})
    {
        this.speakerService.updateSpeaker(speaker.id, speaker.speakerModel).pipe(
            takeUntil(this.destroy$),
            catchError(err => {
              this.errorDialog.openError(err.message);
              throw new Error(err);
            })
          ).subscribe();
    }

    onSpeakerDelete(id : number)
    {
        this.speakerService.deleteSpeaker(id).pipe(
            takeUntil(this.destroy$),
            catchError(err => {
              this.errorDialog.openError(err.message);
              throw new Error(err);
            })
          ).subscribe();
    }
}
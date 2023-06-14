import { Component, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Component({
    selector: 'app-error-message',
    template: `
        <kendo-dialog title="Planner Error" *ngIf="opened" (close)="close('cancel')" [minWidth]="250" [width]="450">
            <p style="margin: 30px; text-align: center;">{{customError}}</p>
            <kendo-dialog-actions>
                <button kendoButton size="medium" style="max-width: 50px" (click)="close('yes')">Okay</button>
            </kendo-dialog-actions>
        </kendo-dialog>
    `
})
export class ErrorMessageComponent {
    @Input()
    customError: string | null = null;
    @Input()
    public opened = false;

    @Output()
    closed = new EventEmitter();

    public close(status: string): void {
        console.log(`Dialog result: ${status}`);
        this.opened = false;
        this.customError = null;
        this.closed.emit();
    }

    public open(): void {
        this.opened = true;
    }
}
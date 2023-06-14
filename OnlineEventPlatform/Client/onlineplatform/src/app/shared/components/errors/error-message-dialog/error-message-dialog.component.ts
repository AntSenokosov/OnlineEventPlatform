import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-error-message-dialog',
    templateUrl: './error-message-dialog.component.html'
})

export class ErrorMessageDialogComponent {
    errorDescription: any;
    opened = false;
    dialogHeader = 'Помилка';
    closeFunction: () => void = () => {};
  
    closeError(status: any) {
      this.opened = false;
      if (this.closeFunction) {
        this.closeFunction();
      }
    }
  
    openError(message: any, dialogHeader = 'Помилка', closeFunc: () => void = () => {}) {
      this.opened = true;
      this.errorDescription = message;
      this.dialogHeader = dialogHeader;
      this.closeFunction = closeFunc;
    }
  }
  
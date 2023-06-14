import {
    Component,
    TemplateRef,
    ViewChild,
    forwardRef,
    Input,
    Output,
    EventEmitter
  } from '@angular/core';
  import { ToolBarToolComponent } from '@progress/kendo-angular-toolbar';
  
  @Component({
    selector: 'toolbar-dropdown-list',
    template: `
      <ng-template #toolbarTemplate>
        <kendo-label class="k-checkbox-label mr-1" [for]="dropdown" [text]="label"></kendo-label>
        <kendo-dropdownlist
          #dropdown
          [data]="data"
          [textField]="textField"
          [valueField]="valueField"
          [(ngModel)]="selected"
          [disabled]="disabled"
          (valueChange)="emitNewValue($event)"
        ></kendo-dropdownlist>
      </ng-template>
    `,
    providers: [{ provide: ToolBarToolComponent, useExisting: forwardRef(() => ToolBarDropdownListComponent) }]
  })
  export class ToolBarDropdownListComponent extends ToolBarToolComponent {
    @ViewChild('toolbarTemplate', { static: true }) public override toolbarTemplate!: TemplateRef<any>;
    @ViewChild('dropdown') public dropdown!: TemplateRef<any>;
  
    @Input() label!: string;
    @Input() textField!: string;
    @Input() valueField!: string;
    @Input() data: any[] = [];
  
    @Input() selected: any;
    @Input() disabled: boolean = false;
  
    @Output() valueChanged = new EventEmitter<any>();
  
    public tabindex = -1;
  
    constructor() {
      super();
    }
  
    emitNewValue(event: any) {
      this.valueChanged.emit(event);
    }
  }
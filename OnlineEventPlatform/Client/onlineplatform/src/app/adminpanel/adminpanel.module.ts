import { NgModule } from "@angular/core";
import { AdminpanelComponent } from "./adminpanel.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

import { GridModule } from '@progress/kendo-angular-grid';
import { InputsModule, TextBoxModule } from "@progress/kendo-angular-inputs";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SpeakerComponent } from "./speaker/speaker.component";
import { SpeakerService } from "./speaker/speaker.service";
import { OnlineEventService } from "./onlineevent/onlineevent.service";
import { OnlineeventComponent } from "./onlineevent/onlineevent.component";
import { UsersComponent } from './users/users.component';
import { UsersService } from "./users/users.service";
import { MailtemplateComponent } from './mailtemplate/mailtemplate.component';
import { ButtonsModule } from "@progress/kendo-angular-buttons";
import { DialogModule } from "@progress/kendo-angular-dialog";
import { AnimationBuilder } from "@angular/animations";
import { ScrollViewModule } from "@progress/kendo-angular-scrollview";
import { SharedModule } from "../shared/shared.module";
import { SpeakerComponentWrapper } from "./speaker/speaker.component.wrapper";
import { AdminpanelHeaderComponent } from "./adminpanel-header/adminpanel-header.component";
import { DropDownListModule, MultiSelectModule } from "@progress/kendo-angular-dropdowns";
import { DateInputModule, DatePickerModule, TimePickerModule } from "@progress/kendo-angular-dateinputs";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { MailTemplateService } from "./mailtemplate/mailtemplate.service";

@NgModule({
    imports:[
        CommonModule,
        GridModule,
        FormsModule,
        TextBoxModule,
        ButtonsModule,
        DialogModule,
        InputsModule,
        DateInputModule,
        ScrollViewModule,
        ReactiveFormsModule,
        DropDownListModule,
        MultiSelectModule,
        DatePickerModule,
        TimePickerModule,
        SharedModule,
        RouterModule.forChild([
            { path: '', component: AdminpanelComponent },
            { path: 'speakers', component: SpeakerComponent},
            { path: 'events', component: OnlineeventComponent},
            { path: 'users', component: UsersComponent},
            { path: 'templates', component: MailtemplateComponent}
          ])
    ],
    declarations:[
        AdminpanelComponent,
        AdminpanelHeaderComponent,
        SpeakerComponentWrapper,
        SpeakerComponent,
        OnlineeventComponent,
        UsersComponent,
        MailtemplateComponent
    ],
    exports:[
    ],
    providers:[
        SpeakerService,
        OnlineEventService,
        UsersService,
        MailTemplateService,
        [AnimationBuilder]
    ]
})

export class AdminPanelModule{}
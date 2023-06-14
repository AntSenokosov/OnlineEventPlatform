import { NgModule } from "@angular/core";
import { DialogModule } from "@progress/kendo-angular-dialog";
import { ErrorMessageComponent } from "./components/errors/error-message.component";
import { ErrorMessageDialogComponent } from "./components/errors/error-message-dialog/error-message-dialog.component";
import { LabelModule } from "@progress/kendo-angular-label";
import { ToolBarDropdownListComponent } from "./components/toolbar-dropdown-list.component";
import { DropDownsModule } from "@progress/kendo-angular-dropdowns";
import { FormsModule } from "@angular/forms";
import { AuthService } from "../auth/auth.service";
import { HeaderComponent } from "./layouts/header/header.component";
import { FooterComponent } from "./layouts/footer/footer.component";

@NgModule({
    imports:[
        FormsModule,
        DialogModule,
        LabelModule,
        DropDownsModule
    ],
    declarations:[
        ErrorMessageComponent,
        ErrorMessageDialogComponent,
        ToolBarDropdownListComponent
    ],
    exports:[
        ErrorMessageDialogComponent
    ],
    providers:[
        AuthService
    ]
})

export class SharedModule {}
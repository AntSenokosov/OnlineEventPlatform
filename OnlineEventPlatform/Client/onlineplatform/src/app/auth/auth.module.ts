import { NgModule } from "@angular/core";
import { LoginComponent } from "./login/login.component";
import { RegistrationComponent } from "./registration/registration.component";
import { AuthService } from "./auth.service";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';

@NgModule({
    imports:[
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            {path: '', component: LoginComponent},
            {path: 'register', component: RegistrationComponent},
            {path: 'forgot', component: ForgotpasswordComponent}
        ])
    ],
    declarations:[
        LoginComponent,
        RegistrationComponent,
        ForgotpasswordComponent
    ],
    exports:[
        FormsModule,
        ReactiveFormsModule
    ],
    providers:[
        AuthService
    ]
})

export class AuthModule{}
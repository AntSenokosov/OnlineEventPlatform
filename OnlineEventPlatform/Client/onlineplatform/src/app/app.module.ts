import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ProfileService } from "./profile/profile.service";
import { UserEventsService } from "./userevents/userevents.service";
import { JwtService } from "./core/services/jwt.service";
import { ApiService } from "./core/services/api.service";
import { CatalogModule } from "./catalog/catalog.module";
import { AuthModule } from "./auth/auth.module";
import { AdminPanelModule } from "./adminpanel/adminpanel.module";
import { BrowserModule } from "@angular/platform-browser";
import { AppComponent } from "./app.component";
import { RouterModule } from "@angular/router";
import { HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from "./app-routing.module";
import { ProfileComponent } from "./profile/profile.component";
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from "./shared/shared.module";
import { MeetingPlatformService } from "./core/services/meeting-platform.service";
import { TypeEventService } from "./core/services/type-event.service";
import { HeaderComponent } from "./shared/layouts/header/header.component";
import { FooterComponent } from "./shared/layouts/footer/footer.component";
import { AuthService } from "./auth/auth.service";
import { DialogModule } from "@progress/kendo-angular-dialog";
import { ButtonModule } from "@progress/kendo-angular-buttons";

@NgModule({
    imports:[
        BrowserModule,
        CatalogModule,
        AuthModule,
        HttpClientModule,
        AdminPanelModule,
        RouterModule,
        AppRoutingModule,
        NoopAnimationsModule,
        SharedModule,
        BrowserAnimationsModule,
        DialogModule,
        ButtonModule
    ],
    declarations:[
        AppComponent,
        ProfileComponent,
        HeaderComponent,
        FooterComponent
    ],
    providers:[
        JwtService,
        AuthService,
        ApiService,
        ProfileService,
        UserEventsService,
        MeetingPlatformService,
        TypeEventService
    ],
    bootstrap: [AppComponent]
})

export class AppModule {}
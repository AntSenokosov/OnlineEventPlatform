import { NgModule } from "@angular/core";
import { CatalogComponent } from "./catalog.component";
import { EventPageComponent } from './event-page/event-page.component';
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { CatalogService } from "./catalog.service";
import { PanelBarModule } from "@progress/kendo-angular-layout";

@NgModule({
    imports:[
        CommonModule,
        PanelBarModule,
        RouterModule.forChild([
            {path: '', component: CatalogComponent},
            {path: 'event/:id', component: EventPageComponent}
        ])
    ],
    declarations:[
        CatalogComponent,
        EventPageComponent
    ],
    exports:[

    ],
    providers:[
        CatalogService
    ]
})

export class CatalogModule {}
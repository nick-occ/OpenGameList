///<reference path="../../typings/index.d.ts"/> 
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms"; 
import { BrowserModule } from "@angular/platform-browser";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router"; 
import "rxjs/Rx";

import { AppComponent } from "./app.component";
import { ItemListComponent } from "./item-list.component";
import { ItemDetailEditComponent } from "./item-detail-edit.component"; 
import { ItemDetailViewComponent } from "./item-detail-view.component"; 
import { LoginComponent } from "./login.component";
import { PageNotFoundComponent } from "./page-not-found.component";
import { AppRouting } from "./app.routing"; 
import { AuthHttp } from "./auth.http";
import { AuthService } from "./auth.service";
import { ItemService } from "./item.service"; 

@NgModule({
    // directives, components, and pipes 
    declarations: [
        AppComponent,
        ItemListComponent,
        ItemDetailEditComponent,
        ItemDetailViewComponent,
        LoginComponent,
        PageNotFoundComponent  
    ],
    // modules 
    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        ReactiveFormsModule, 
        RouterModule,
        AppRouting 
    ],
    // providers 
    providers: [
        AuthHttp,
        AuthService,
        ItemService
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule { } 
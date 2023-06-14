import { inject } from "@angular/core";
import { Observable } from "rxjs";
import { ApiService } from "./api.service";

export class MailSendService{
    private readonly endpoint = "mailsends";
    private readonly apiService = inject(ApiService);

}
import { inject } from "@angular/core";
import { Observable, map } from "rxjs";
import { ApiService } from "src/app/core/services/api.service";
import { MailTemplate } from "./mailtemplate.model";

export class MailTemplateService{
    private readonly endpoint = "mailtemplate";
    private readonly apiService = inject(ApiService);

    getTemplates() : Observable<MailTemplate[]>
    {
        return this.apiService.get(this.endpoint).pipe(
            map((response : any) => response.items as MailTemplate[])
        );
    }

    createTemplate(template : MailTemplate) : Observable<MailTemplate>
    {
        return this.apiService.post(`${this.endpoint}/create`, template);
    }

    updateTemplate(id : number, template : MailTemplate) : Observable<MailTemplate>
    {
        return this.apiService.put(`${this.endpoint}/${id}/update`, template);
    }

    deleteTemplate(id : number) : Observable<MailTemplate>
    {
        return this.apiService.delete(`${this.endpoint}/${id}/delete`);
    }
}
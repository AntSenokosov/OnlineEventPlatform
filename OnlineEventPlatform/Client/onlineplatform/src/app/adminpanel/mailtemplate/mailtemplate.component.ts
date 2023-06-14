import { Component, inject } from '@angular/core';
import { MailTemplateService } from './mailtemplate.service';
import { MailTemplate } from './mailtemplate.model';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-mailtemplate',
  templateUrl: './mailtemplate.component.html',
  styleUrls: ['./mailtemplate.component.css']
})
export class MailtemplateComponent {
  private readonly templateService = inject(MailTemplateService);

  public gridData!: Observable<MailTemplate[]>;

  public templateToUpdate: MailTemplate | null = null;
  public templateToDelete: MailTemplate | null = null;

  public isDeleteDialog = false;
  public isCreateDialog = false;

  templateForm: FormGroup | undefined;

  ngOnInit(): void {
    this.gridData = this.templateService.getTemplates();
  }

addHandler(){
    this.templateForm = new FormGroup({
        name : new FormControl('', Validators.required),
        html : new FormControl(''),
        css : new FormControl('')
    });

    this.isCreateDialog = true;
}

updateHandler(event: { dataItem: MailTemplate; }){
    this.templateToUpdate = event.dataItem;
    this.templateForm = new FormGroup({
        name : new FormControl(this.templateToUpdate.name),
        html : new FormControl(this.templateToUpdate.html),
        css : new FormControl(this.templateToUpdate.css)
    });

    this.isCreateDialog = true;
}

removeHandler(event: { dataItem: MailTemplate; })
{
    this.isDeleteDialog = true;
    this.templateToDelete = event.dataItem;
}

deleteTemplate()
{
    this.templateService.deleteTemplate(this.templateToDelete!.id).subscribe(
        () =>{
            this.ngOnInit();
        },
        (error: any) => {
            console.error('Failed to delete speaker:', error);
        }
    );
    this.isDeleteDialog = false;
}

closeSpeakerCreation()
{
    this.isCreateDialog = false;
    this.templateForm = undefined;
}

closeDeletionDialog()
{
    this.isDeleteDialog = false;
}

onFormSubmit()
{
    if (this.templateForm!.valid)
    {
        const speakerModel : MailTemplate = this.templateForm!.value;

        if (!this.templateToUpdate)
        {
            this.templateService.createTemplate(speakerModel).subscribe(
                () =>{
                    this.ngOnInit();
                },
                (error: any) => {
                    console.error('Failed to add speaker:', error);
                });
        }
        else
        {
            const id = this.templateToUpdate.id;
            this.templateService.updateTemplate(id, speakerModel).subscribe(
                () =>{
                    this.ngOnInit();
                },
                (error: any) => {
                    console.error('Failed to update speaker:', error);
                });
        }
    }

    this.isCreateDialog = false;
    this.templateToUpdate = null;
    this.templateForm = undefined;
}
}

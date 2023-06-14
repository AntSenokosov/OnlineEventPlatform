import { Component, OnInit, inject } from '@angular/core';
import { UsersService } from './users.service';
import { User } from 'src/app/auth/user.model';
import { RegisterRequest } from 'src/app/auth/dtos/registerrequest';
import { Observable } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit{
  private readonly usersService = inject(UsersService);

  public gridData! : Observable<User[]>;

  public userToDelete: User | null = null;

  public isDeleteDialog = false;
  public isCreateDialog = false;

  userForm: FormGroup | undefined;

  ngOnInit(): void {
      this.gridData = this.usersService.getUsers();
  }

  deleteUser(){
    this.usersService.deleteUser(this.userToDelete!.id).subscribe(
      () =>{
          this.ngOnInit();
      },
      (error: any) => {
          console.error('Failed to delete user:', error);
      }
  );
  this.isDeleteDialog = false;
  }

  updateUserRole(id : number, isAdmin : boolean) : void{
    if (isAdmin)
    {
      this.usersService.updateToUser(id).subscribe(
        () =>{
          this.ngOnInit();
        },
        (error: any) => {
          console.error('Failed to update user:', error);
          }
      );
    }
    else{
      this.usersService.updateToAdmin(id).subscribe(
        () =>{
          this.ngOnInit();
        },
        (error: any) => {
          console.error('Failed to update user:', error);
          }
      );
    }
  }

  addHandler()
  {
    this.userForm = new FormGroup({
      firstName : new FormControl('', Validators.required),
      lastName : new FormControl('', Validators.required),
      email : new FormControl('', Validators.required),
      password : new FormControl('', Validators.required)
    });

    this.isCreateDialog = true;
  }

  removeHandler(event: {dataItem : User})
  {
    this.isDeleteDialog = true;
    this.userToDelete = event.dataItem;
  }

  closeCreateDialog()
  {
    this.isCreateDialog = false;
    this.userForm = undefined;
  }

  closeDeletionDialog()
  {
    this.isDeleteDialog = false;
  }

  onFormSubmit()
  {
    if (this.userForm?.valid) {
      const userModel: RegisterRequest = this.userForm.value as RegisterRequest;
  
      this.usersService.createUser(userModel).subscribe(
        () => {
          this.ngOnInit();
        },
        (error: any) => {
          console.error('Failed to add user:', error);
        }
      );
    }
  
    this.isCreateDialog = false;
    this.userForm = undefined;
  }
}

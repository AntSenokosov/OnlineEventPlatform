import { Component, inject } from '@angular/core';
import { RegisterRequest } from '../dtos/registerrequest';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  registerRequest: RegisterRequest = {firstName: '', lastName: '', email: '', password: ''}

  registrationConfirmPassword! : string;

  registrationError: boolean = false;
  invalidEmail: boolean = false; 
  passwordMismatch : boolean = false;

  userId: number = 0;
  registrationSuccess : boolean = false;

  register(): void {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!emailRegex.test(this.registerRequest.email)) {
      this.invalidEmail = true;
      return;
    }

    if (this.registerRequest.password != this.registrationConfirmPassword)
    {
      this.passwordMismatch = true;
      return;
    }
    this.authService.register(this.registerRequest).subscribe(
      (response : any) =>{
        this.registrationSuccess = true;
        this.userId = response.Id;
        this.router.navigateByUrl('auth');
      },
      (error) => {
        this.registrationError = true;
      }
    );
  }
}

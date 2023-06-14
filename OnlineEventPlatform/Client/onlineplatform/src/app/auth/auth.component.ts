import { Component } from '@angular/core';
import { User } from './user.model';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { RegisterRequest } from './dtos/registerrequest';
import { LoginRequest } from './dtos/loginrequest';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  loginRequest : LoginRequest | null = null;
  email: string = '';
  password: string = '';
  loginError: boolean = false;
  currentUser: User | null = null;

  registrationFirstName! : string;
  registrationLastName! : string;
  registrationEmail!: string;
  registrationPassword!: string;
  registrationConfirmPassword! : string;
  registrationError: boolean = false;
  
  invalidEmail: boolean;
  passwordMismatch : boolean = false;

  isRegistrationFormVisible: boolean;

  constructor(private authService: AuthService, private route : Router) {
    this.isRegistrationFormVisible = false;
    this.invalidEmail = false;
  }

  login(): void {
    alert(
      `email: ${this.loginRequest?.email}\n` +
      `password: ${this.loginRequest?.password}`
    );
    let userLogin : LoginRequest = {
      email:this.email,
      password:this.password
    };
    this.authService.login(userLogin);
    if (this.authService.isLoggedIn$) {
      this.loginError = false;
      //this.authService.getCurrentUser().subscribe(user => {this.currentUser = user;});
      this.route.navigateByUrl('/');
    } else {
      this.loginError = true;
    }
  }

  register(): void {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    /*if (!emailRegex.test(this.registrationEmail)) {
      this.invalidEmail = true;
      return;
    }
*/
    if (this.registrationPassword != this.registrationConfirmPassword)
    {
      this.passwordMismatch = true;
      return;
    }

    const user : RegisterRequest = {
      firstName:this.registrationFirstName,
      lastName:this.registrationLastName,
      email: this.registrationEmail,
      password: this.registrationPassword
    };
    alert(
      `firstName: ${user.firstName}\n` +
      `lastName: ${user.lastName}\n` +
      `email: ${user.email}\n` +
      `password: ${user.password}`
    );
    this.authService.register(user);
    /*
    if (isRegistered) {
      this.registrationError = false;
      this.email = this.registrationEmail; // Заповнити поле електронної пошти автоматично після реєстрації
      this.password = this.registrationPassword; // Заповнити поле пароля автоматично після реєстрації
      this.login(); // Автоматичний вхід після реєстрації
    } else {
      this.registrationError = true;
    }
    */
  }

  logout(): void {
    this.authService.logout();
    this.currentUser = null;
  }

  get isLoggedIn(): boolean {
    return !!this.currentUser;
  }
}

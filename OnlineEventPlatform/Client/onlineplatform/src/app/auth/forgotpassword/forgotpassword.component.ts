import { Component, inject } from '@angular/core';
import { AuthService } from '../auth.service';
import { RecoveryPasswordRequest } from '../dtos/recoverypassword.request';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  emailRequest: RecoveryPasswordRequest = { email: ''};
  loginError: boolean = false;

  forgotpassword() : void{
    this.authService.forgotpassword(this.emailRequest).subscribe(
      (response : any) =>{
        this.router.navigateByUrl('auth');
      }
    );
  }
}

import { Component, Injectable, inject } from '@angular/core';
import { LoginRequest } from '../dtos/loginrequest';
import { AuthService } from '../auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtService } from 'src/app/core/services/jwt.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
@Injectable()
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly jwt = inject(JwtService);

  loginRequest: LoginRequest = { email: '', password: '' };
  loginError: boolean = false;

  login(): void {
    this.authService.login(this.loginRequest).subscribe((loggedIn: boolean) => {
      if (loggedIn) {
        this.authService.isLoggedInSubject.next(true);
        //this.router.navigate([''], {  queryParams: { transition: 'slideLeft' },}).then(()=> window.location.reload());
        //this.router.navigate(['']).then(()=> window.location.reload());
        this.router.navigateByUrl(this.getReturnUrl()).then(() => window.location.reload());
      } else {
        this.loginError = true;
      }
    });
  }

  private getReturnUrl(): string {
    const queryParams = this.route.snapshot.queryParams;
    return queryParams['returnUrl'] || '/';
  }
}

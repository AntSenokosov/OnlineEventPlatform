import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Injectable()
export class AdminGuard implements CanActivate {
  constructor(private router: Router, private authService : AuthService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Виконайте перевірку наявності аутентифікованого користувача і його ролі
    const isAuthenticated = this.authService.isLoggin(); // Перевірка аутентифікації користувача
    const isAdmin = this.authService.isAdmin(); // Перевірка ролі користувача

    if (isAuthenticated && isAdmin) {
      return true;
    } else {
      // Редирект або обробка помилки, якщо користувач не має доступу
      this.router.navigateByUrl('/auth');
      return false;
    }
  }
}
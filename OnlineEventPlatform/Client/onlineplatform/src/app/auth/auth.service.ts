import { EventEmitter, Injectable } from '@angular/core';
import { User } from './user.model';
import { ApiService } from '../core/services/api.service';
import { LoginRequest } from './dtos/loginrequest';
import { RegisterRequest } from './dtos/registerrequest';
import { JwtService } from '../core/services/jwt.service';
import { BehaviorSubject, Observable, catchError, map, of, shareReplay, tap } from 'rxjs';
import { LoginResponse } from './dtos/loginresponse';
import { ProfileService } from '../profile/profile.service';
import { RecoveryPasswordRequest } from './dtos/recoverypassword.request';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private endpoint = 'auth';
  private currentUser: User | null = null;
  currentUserSubject = new BehaviorSubject<User | null>(null);
  public isLoggedInSubject: BehaviorSubject<boolean>;

  constructor(
    private apiService: ApiService,
    private jwtService: JwtService,
    private profileService : ProfileService
  ) {
    this.isLoggedInSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
  }

  logged = new BehaviorSubject<User | null>(null);

  get isLoggedIn$(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  get currentUser$(): Observable<User | null> {
    return this.currentUserSubject.asObservable();
  }

  login(loginRequest: LoginRequest): Observable<boolean> {
    return this.apiService.post(`${this.endpoint}/login`, loginRequest).pipe(
      map((response: User) => {
        this.jwtService.saveToken(response.token);
        
        this.currentUser = response;
        this.currentUserSubject.next(response);
        this.isLoggedInSubject.next(true);
        console.error(`${response.email}`);
        //this.logged.next(response);
        //this.router.navigate(['']);
        return true;
      }),
      catchError((error) => {
        this.logout(); // Викликаємо logout() при помилці входу
        return of(false);
      })
    );
  }

  logout(): void {
    this.jwtService.destroyToken();
    this.logged.next(null);
    this.currentUser = null;
    this.currentUserSubject.next(null);
    this.isLoggedInSubject.next(false);

    localStorage.clear();
  }

  forgotpassword(email: RecoveryPasswordRequest): Observable<boolean> {
    return this.apiService.post(`${this.endpoint}/recovery`, email);
  }

  register(registerRequest: RegisterRequest): Observable<any> {
    return this.apiService.post(`${this.endpoint}/register`, registerRequest);
  }

  checkGoogleAuth(email: string): Observable<boolean> {
    return this.apiService.get(`${this.endpoint}/check/` + email);
  }

  getCurrentUser(): Observable<User | null> {
    if (this.isLoggedIn()) {
      return this.profileService.getProfile();
    } else {
      console.error('Поточний користувач недоступний');
      return of(null);
    }
  }

  isLoggedIn(): boolean {
    const token = this.jwtService.getToken();
    const isLoggedIn = !!token;
    return isLoggedIn;
  }
}
import { Component, OnInit, OnDestroy, DoCheck } from '@angular/core';
import { Observable, Subscription, of } from 'rxjs';
import { AuthService } from '../../../auth/auth.service';
import { User } from 'src/app/auth/user.model';
import { ProfileService } from 'src/app/profile/profile.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy{
  isLoggedIn: boolean = false;
  currentUser : User | null = null;
  isAdmin: boolean = false;
  isSuperAdmin: boolean = false;
  private loggedInSubscription!: Subscription;


  private subscriptions : Subscription[] = [];
  loggedIn : boolean = false;
  userName = '';

  constructor(private authService: AuthService, private profileService : ProfileService) {}

  ngOnInit(): void {
    /*
    this.subscriptions.push(this.authService.currentUser$.subscribe((value) =>{
      this.onLogged(value!)
    }));
    */
    
    this.loggedInSubscription = this.authService.isLoggedIn$.subscribe((isLoggedIn: boolean) => {
      this.isLoggedIn = isLoggedIn;
    });

    if (this.isLoggedIn)
    {
      /*
      this.authService.currentUser$.subscribe((user: User | null) => {
        this.currentUser = user;
      });

      this.userName = `${this.currentUser?.firstName} ${this.currentUser?.lastName}`;

      console.error(`${this.currentUser?.email} ${this.currentUser?.isAdmin}`)

      this.isAdmin = this.currentUser!.isAdmin;
      this.isSuperAdmin = this.currentUser!.isSuperAdmin;
      */
      this.profileService.getProfile().subscribe((user) => {
        if (user) {
          this.currentUser = {} as User;
          Object.assign(this.currentUser, user);
          this.userName = `${this.currentUser?.firstName} ${this.currentUser?.lastName}`;
          this.isAdmin = this.currentUser!.isAdmin;
      this.isSuperAdmin = this.currentUser!.isSuperAdmin;
        } else {
          console.error('Current user is not available');
        }
      });
    }
  }

  onLogged(user: User)
  {
      this.isLoggedIn = true;
      this.userName = `${user.firstName} ${user.lastName}`;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
    this.loggedInSubscription.unsubscribe();
  }

  logout(): void {
    this.isLoggedIn = false;
    this.authService.logout();
  }
}
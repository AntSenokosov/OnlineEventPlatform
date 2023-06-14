import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { UsereventsComponent } from './userevents/userevents.component';
import { AuthGuard } from './auth/auth-guard.service';

const routes: Routes = [
  {
    path : '',
    loadChildren : () => import('./catalog/catalog.module').then((x) => x.CatalogModule)
  },
  {
    path : 'auth',
    loadChildren: () => import('./auth/auth.module').then((x) => x.AuthModule)
  },
  {
    path : 'adminpanel',
    loadChildren: () => import('./adminpanel/adminpanel.module').then((x) => x.AdminPanelModule),
    canActivate : [AuthGuard]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'userevents',
    component: UsereventsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      // preload all modules; optionally we could
      // implement a custom preloading strategy for just some
      // of the modules (PRs welcome 😉)
      preloadingStrategy: PreloadAllModules,
    }),
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

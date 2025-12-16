import { Routes } from '@angular/router';
import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';
import { MainLayout } from './pages/main/main-layout/main-layout';
import { Timetable } from './pages/main/timetable/timetable';

export const routes: Routes = [
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  // Auth
  { path: 'auth/login', component: Login },
  { path: 'auth/register', component: Register },

  // Main space
  {
    path: 'main',
    component: MainLayout,
    children: [
      { path: '', redirectTo: 'timetable', pathMatch: 'full' },
      { path: 'timetable', component: Timetable },
      // Можно добавить другие страницы здесь
    ],
  },

  { path: '**', redirectTo: '/auth/login' },
];

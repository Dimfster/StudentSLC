import { Routes } from '@angular/router';

import { Login } from './pages/auth/login/login';
import { Register } from './pages/auth/register/register';

import { MainLayout } from './pages/main/main-layout/main-layout';
import { Timetable } from './pages/main/main-layout/timetable/timetable';

import { TimetableGroups } from './pages/main/main-layout/timetable/groups/groups';
import { TimetableTeachers } from './pages/main/main-layout/timetable/teachers/teachers';
import { TimetableRooms } from './pages/main/main-layout/timetable/rooms/rooms';

export const routes: Routes = [
  { path: '', redirectTo: '/auth/login', pathMatch: 'full' },

  /* AUTH */
  { path: 'auth/login', component: Login },
  { path: 'auth/register', component: Register },

  /* MAIN LAYOUT */
  {
    path: 'main',
    component: MainLayout,
    children: [
      {
        path: 'timetable',
        component: Timetable,
        children: [
          { path: '', redirectTo: 'groups', pathMatch: 'full' },
          { path: 'groups', component: TimetableGroups },
          { path: 'teachers', component: TimetableTeachers },
          { path: 'rooms', component: TimetableRooms },
        ],
      },
    ],
  },

  { path: '**', redirectTo: '/auth/login' },
];

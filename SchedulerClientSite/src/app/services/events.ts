import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EventCreateModel {
  name: string;
  startTime: string; // ISO
  endTime: string;   // ISO
  roomName: string;
  participantId: number; // один студент
  keyHolderId: number;   // один преподаватель
}

export interface User {
  userCode: number;
  firstName: string;
  lastName: string;
  patronymic?: string;
  role: string;
}

export interface Room {
  name: string;
  type: string;
}

@Injectable({ providedIn: 'root' })
export class EventsService {
  private API_EVENTS = 'http://localhost:5171/api/events';
  private API_USERS = 'http://localhost:5171/api/users';
  private API_ROOMS = 'http://localhost:5171/api/rooms';

  constructor(private http: HttpClient) {}

  createEvent(event: EventCreateModel): Observable<any> {
    return this.http.post(this.API_EVENTS, event);
  }

  getAllStudents(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API_USERS}/GetAllUsers`);
  }

  getAllTeachers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.API_USERS}/GetAllTeachers`);
  }

  getAllRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(`${this.API_ROOMS}/GetAllRooms`);
  }
}

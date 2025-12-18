import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Group {
  id: string;
  name: string;
}

export interface Teacher {
  userCode: number;
  firstName: string;
  lastName: string;
  patronymic: string;
  role: string;
}

export interface Room {
  name: string;
  type: string;
}

@Injectable({ providedIn: 'root' })
export class AdminService {
  private API = 'http://localhost:5171/api';

  constructor(private http: HttpClient) {}

  getGroups(): Observable<Group[]> {
    return this.http.get<Group[]>(`${this.API}/groups/GetAllGroups`);
  }

  getTeachers(): Observable<Teacher[]> {
    return this.http.get<Teacher[]>(`${this.API}/users/GetAllTeachers`);
  }

  getRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(`${this.API}/rooms/GetAllRooms`);
  }

  updateGroup(group: Group): Observable<any> {
    return this.http.put(`${this.API}/groups/${encodeURIComponent(group.name)}`, { name: group.name });
  }

  updateTeacher(teacher: Teacher): Observable<any> {
    return this.http.put(`${this.API}/users/${teacher.userCode}`, teacher);
  }

  updateRoom(room: Room): Observable<any> {
    return this.http.put(`${this.API}/rooms/${encodeURIComponent(room.name)}`, { type: room.type });
  }

  deleteGroup(name: string): Observable<any> {
    return this.http.delete(`${this.API}/groups/${encodeURIComponent(name)}`);
  }

  deleteTeacher(userCode: number): Observable<any> {
    return this.http.delete(`${this.API}/users/${userCode}`);
  }

  deleteRoom(name: string): Observable<any> {
    return this.http.delete(`${this.API}/rooms/${encodeURIComponent(name)}`);
  }
}

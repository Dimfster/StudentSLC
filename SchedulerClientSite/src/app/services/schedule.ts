import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ScheduleEvent {
  name: string;
  startTime: string;
  endTime: string;
  roomName: string;
  keyHolderNames: string[];
}

export interface GroupDto {
  id: string;
  name: string;
}

export interface TeacherDto {
  userCode: number;
  firstName: string;
  lastName: string;
  patronymic: string;
  role: string;
}

export interface RoomDto {
  name: string;
  type: string;
}

@Injectable({ providedIn: 'root' })
export class ScheduleService {
  private API = 'http://localhost:5171/api';

  constructor(private http: HttpClient) {}

  // ===== ГРУППЫ =====
  getGroupSchedule(group: string, weekStart: Date): Observable<ScheduleEvent[]> {
    const date = weekStart.toISOString().split('T')[0];
    return this.http.get<ScheduleEvent[]>(
      `${this.API}/schedule/groups/${encodeURIComponent(group)}?weekStart=${date}`
    );
  }

  // ===== ПРЕПОДАВАТЕЛИ =====
  getTeacherSchedule(
    teacherId: number,
    weekStart: Date
  ): Observable<ScheduleEvent[]> {
    const date = weekStart.toISOString().split('T')[0];
    return this.http.get<ScheduleEvent[]>(
      `${this.API}/schedule/keyholders/${teacherId}?weekStart=${date}`
    );
  }

  getAllGroups() {
    return this.http.get<GroupDto[]>(
      'http://localhost:5171/api/groups/GetAllGroups'
    );
  }

  getAllTeachers(): Observable<TeacherDto[]> {
    return this.http.get<TeacherDto[]>(
      `${this.API}/users/GetAllTeachers`
    );
  }

  // ===== КОМНАТЫ =====
  getAllRooms(): Observable<RoomDto[]> {
    return this.http.get<RoomDto[]>(
      `${this.API}/rooms/GetAllRooms`
    );
  }

  getRoomSchedule(
    roomName: string,
    weekStart: Date
  ): Observable<ScheduleEvent[]> {
    const date = weekStart.toISOString().split('T')[0];
    return this.http.get<ScheduleEvent[]>(
      `${this.API}/schedule/rooms/${encodeURIComponent(roomName)}?weekStart=${date}`
    );
  }
}

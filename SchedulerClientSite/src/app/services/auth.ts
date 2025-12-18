import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

interface LoginRequest {
  userCode: number;
  password: string;
}

interface LoginResponse {
  userCode: number;
  role: string;
  token: string;
  expiresIn: number;
}
interface RegisterRequest {
  firstName: string;
  lastName: string;
  patronymic: string;
  password: string;
}

interface RegisterResponse {
  userCode: number;
  firstName: string;
  lastName: string;
  patronymic: string;
  role: string;
}

export interface User {
  userCode: number;
  firstName: string;
  lastName: string;
  patronymic?: string;
  role: string;
}


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly API_URL = 'http://localhost:5171/api/auth';

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<LoginResponse & User> {
    return this.http.post<LoginResponse & User>(
      `${this.API_URL}/login`,
      data
    ).pipe(
      tap(user => {
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user));
      })
    );
  }

  register(data: RegisterRequest) {
    return this.http.post<RegisterResponse>(
      `${this.API_URL}/register`,
      data
    );
  }

  getUser(): User | null {
    const raw = localStorage.getItem('user');
    return raw ? JSON.parse(raw) : null;
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

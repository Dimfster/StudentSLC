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

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly API_URL = 'http://localhost:5171/api/auth';

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.API_URL}/login`, data)
      .pipe(
        tap((response) => {
          localStorage.setItem('token', response.token);
        })
      );
  }

  register(data: RegisterRequest) {
    return this.http.post<RegisterResponse>(
      `${this.API_URL}/register`,
      data
    );
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrls: ['./register.css'], 
})

export class Register implements OnInit {
  form!: FormGroup;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      patronymic: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(5)]],
    });
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.authService.register(this.form.value).subscribe({
      next: (res) => {
        this.successMessage =
          `Регистрация прошла успешно. Ваш номер пропуска: ${res.userCode}`;
        this.loading = false;
        this.form.reset();
      },
      error: (err) => {
        this.errorMessage =
          err?.error?.message || 'Ошибка регистрации';
        this.loading = false;
      },
    });
  }
}

import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  ScheduleService,
  ScheduleEvent,
  TeacherDto
} from '../../../../../services/schedule';
import { ScheduleGrid } from '../schedule-grid/schedule-grid';

@Component({
  selector: 'app-timetable-teachers',
  standalone: true,
  imports: [CommonModule, FormsModule, ScheduleGrid],
  templateUrl: './teachers.html',
  styleUrls: ['./teachers.css']
})
export class TimetableTeachers {
  teachers: TeacherDto[] = [];
  selectedTeacherId: number | null = null;

  events: ScheduleEvent[] = [];
  loading = false;

  selectedWeek: string = this.getCurrentWeekInput();

  constructor(
    private schedule: ScheduleService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadTeachers();
  }

  // ===== ЗАГРУЗКА ПРЕПОДАВАТЕЛЕЙ =====
  loadTeachers() {
    this.schedule.getAllTeachers().subscribe({
      next: (data) => {
        this.teachers = data;
        this.cd.markForCheck();
      },
      error: (err) => console.error('Ошибка загрузки преподавателей', err)
    });
  }

  // ===== ЛОГИКА НЕДЕЛИ (1 В 1 КАК В GROUPS) =====

  weekInputToDate(weekInput: string): Date {
    const [yearStr, weekStr] = weekInput.split('-W');
    const year = Number(yearStr);
    const week = Number(weekStr);

    const simple = new Date(year, 0, 1 + (week - 1) * 7);
    const day = simple.getDay() || 7;
    if (day !== 1) simple.setDate(simple.getDate() - (day - 1));

    // фикс UTC-сдвига
    simple.setHours(1, 0, 0, 0);
    return simple;
  }

  private getCurrentWeekInput(): string {
    const now = new Date();
    const year = now.getFullYear();
    const week = this.getWeekNumber(now);
    return `${year}-W${week.toString().padStart(2, '0')}`;
  }

  private getWeekNumber(d: Date): number {
    const date = new Date(d.getTime());
    date.setHours(0, 0, 0, 0);
    date.setDate(date.getDate() + 4 - (date.getDay() || 7));
    const yearStart = new Date(date.getFullYear(), 0, 1);
    return Math.ceil((((date.getTime() - yearStart.getTime()) / 86400000) + 1) / 7);
  }

  // ===== ЗАГРУЗКА РАСПИСАНИЯ =====
  loadSchedule() {
    if (!this.selectedTeacherId) return;

    this.loading = true;
    this.events = [];

    const weekStart = this.weekInputToDate(this.selectedWeek);

    // ⚠️ важно: отправляем не воскресенье
    const requestDate = new Date(weekStart);
    requestDate.setDate(requestDate.getDate() + 1);

    console.log(
      'Запрос расписания преподавателя:',
      this.selectedTeacherId,
      requestDate
    );

    this.schedule
      .getTeacherSchedule(this.selectedTeacherId, requestDate)
      .subscribe({
        next: (data) => {
          console.log('Ответ сервера:', data);
          this.events = data;
          this.loading = false;
          this.cd.markForCheck();
        },
        error: (err) => {
          console.error(err);
          this.events = [];
          this.loading = false;
          this.cd.markForCheck();
        },
      });
  }

  // для template
  getTeacherName(t: TeacherDto): string {
    return `${t.firstName} ${t.lastName}`;
  }
}

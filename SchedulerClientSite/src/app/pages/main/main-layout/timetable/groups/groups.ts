import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  ScheduleService,
  ScheduleEvent,
  GroupDto
} from '../../../../../services/schedule';
import { ScheduleGrid } from '../schedule-grid/schedule-grid';

@Component({
  selector: 'app-timetable-groups',
  standalone: true,
  imports: [CommonModule, FormsModule, ScheduleGrid],
  templateUrl: './groups.html',
  styleUrls: ['./groups.css']
})
export class TimetableGroups {
  groups: GroupDto[] = [];
  selectedGroupName: string | null = null;

  events: ScheduleEvent[] = [];
  loading = false;

  selectedWeek: string = this.getCurrentWeekInput();

  constructor(
    private schedule: ScheduleService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadGroups();
  }

  // ===== ГРУППЫ =====
  loadGroups() {
    this.schedule.getAllGroups().subscribe({
      next: (data) => {
        this.groups = data;
        this.cd.markForCheck();
      },
      error: (err) => console.error('Ошибка загрузки групп', err),
    });
  }

  // ===== ЛОГИКА НЕДЕЛИ (ЕДИНАЯ) =====
  weekInputToDate(weekInput: string): Date {
    const [yearStr, weekStr] = weekInput.split('-W');
    const year = Number(yearStr);
    const week = Number(weekStr);

    const simple = new Date(year, 0, 1 + (week - 1) * 7);
    const day = simple.getDay() || 7;
    if (day !== 1) simple.setDate(simple.getDate() - (day - 1));

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
    return Math.ceil(
      (((date.getTime() - yearStart.getTime()) / 86400000) + 1) / 7
    );
  }

  // ===== РАСПИСАНИЕ =====
  loadSchedule() {
    if (!this.selectedGroupName) return;

    this.loading = true;
    this.events = [];

    const weekStart = this.weekInputToDate(this.selectedWeek);
    const requestDate = new Date(weekStart);
    requestDate.setDate(requestDate.getDate() + 1);

    console.log(
      'Запрос расписания группы:',
      this.selectedGroupName,
      requestDate
    );

    this.schedule
      .getGroupSchedule(this.selectedGroupName, requestDate)
      .subscribe({
        next: (data) => {
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
}

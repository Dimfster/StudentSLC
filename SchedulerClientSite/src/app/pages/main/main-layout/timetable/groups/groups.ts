import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ScheduleService, ScheduleEvent } from '../../../../../services/schedule';
import { ScheduleGrid } from '../schedule-grid/schedule-grid';

@Component({
  selector: 'app-timetable-groups',
  standalone: true,
  imports: [CommonModule, FormsModule, ScheduleGrid],
  templateUrl: './groups.html',
  styleUrls: ['./groups.css']
})
export class TimetableGroups {
  groups = ['Group A', 'Group B', 'Group C'];
  selectedGroup = '';
  events: ScheduleEvent[] = [];
  loading = false;

  selectedWeek: string = this.getCurrentWeekInput();

  constructor(private schedule: ScheduleService, private cd: ChangeDetectorRef) {}

  ngOnInit() {
    if (this.selectedGroup) {
      this.loadSchedule();
    }
  }

  weekInputToDate(weekInput: string): Date {
    const [yearStr, weekStr] = weekInput.split('-W');
    const year = Number(yearStr);
    const week = Number(weekStr);

    const simple = new Date(year, 0, 1 + (week - 1) * 7);
    const day = simple.getDay() || 7;
    if (day !== 1) simple.setDate(simple.getDate() - (day - 1));
    simple.setHours( 1,0,0,0);

    return simple;
  }

  private getCurrentWeekInput(): string {
    const now = new Date();
    const year = now.getFullYear();
    const week = this.getWeekNumber(now);
    return `${year}-W${week.toString().padStart(2,'0')}`;
  }

  private getWeekNumber(d: Date): number {
    const date = new Date(d.getTime());
    date.setHours(0,0,0,0);
    date.setDate(date.getDate() + 4 - (date.getDay()||7));
    const yearStart = new Date(date.getFullYear(),0,1);
    return Math.ceil((((date.getTime()-yearStart.getTime())/86400000)+1)/7);
  }

  loadSchedule() {
    if (!this.selectedGroup) return;

    this.loading = true;
    this.events = [];

    const weekStart = this.weekInputToDate(this.selectedWeek);
    console.log('Понедельник??:', weekStart);
    const nextDay = new Date(weekStart); // создаём копию
    nextDay.setDate(nextDay.getDate() + 1); // добавляем 1 день
    console.log('След:', nextDay);

    this.schedule.getGroupSchedule(this.selectedGroup, nextDay).subscribe({
      next: (data) => {
        console.log('Ответ сервера:', data);
        this.events = data; // заменяем массив, чтобы OnPush сработал
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

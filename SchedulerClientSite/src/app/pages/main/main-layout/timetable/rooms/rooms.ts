import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  ScheduleService,
  ScheduleEvent,
  RoomDto
} from '../../../../../services/schedule';
import { ScheduleGrid } from '../schedule-grid/schedule-grid';

@Component({
  selector: 'app-timetable-rooms',
  standalone: true,
  imports: [CommonModule, FormsModule, ScheduleGrid],
  templateUrl: './rooms.html',
  styleUrls: ['./rooms.css']
})
export class TimetableRooms {
  rooms: RoomDto[] = [];
  filteredRooms: RoomDto[] = [];

  selectedRoomName: string | null = null;
  selectedType: string = 'all';

  events: ScheduleEvent[] = [];
  loading = false;

  selectedWeek: string = this.getCurrentWeekInput();

  constructor(
    private schedule: ScheduleService,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadRooms();
  }

  // ===== КОМНАТЫ =====
  loadRooms() {
    this.schedule.getAllRooms().subscribe({
      next: (data) => {
        this.rooms = data;
        this.applyFilters();
        this.cd.markForCheck();
      },
      error: (err) => console.error('Ошибка загрузки комнат', err)
    });
  }

  applyFilters() {
    this.filteredRooms =
      this.selectedType === 'all'
        ? this.rooms
        : this.rooms.filter(r => r.type === this.selectedType);

    // если выбранная комната пропала из фильтра
    if (
      this.selectedRoomName &&
      !this.filteredRooms.some(r => r.name === this.selectedRoomName)
    ) {
      this.selectedRoomName = null;
      this.events = [];
    }
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
    return Math.ceil((((date.getTime() - yearStart.getTime()) / 86400000) + 1) / 7);
  }

  // ===== РАСПИСАНИЕ =====
  loadSchedule() {
    if (!this.selectedRoomName) return;

    this.loading = true;
    this.events = [];

    const weekStart = this.weekInputToDate(this.selectedWeek);
    const requestDate = new Date(weekStart);
    requestDate.setDate(requestDate.getDate() + 1);

    console.log(
      'Запрос расписания комнаты:',
      this.selectedRoomName,
      requestDate
    );

    this.schedule
      .getRoomSchedule(this.selectedRoomName, requestDate)
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

  getRoomTypes(): string[] {
    return Array.from(new Set(this.rooms.map(r => r.type)));
  }
}

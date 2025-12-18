import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScheduleEvent } from '../../../../../services/schedule';

@Component({
  selector: 'app-schedule-grid',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './schedule-grid.html',
  styleUrls: ['./schedule-grid.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ScheduleGrid {
  @Input() events: ScheduleEvent[] = [];
  @Input() loading = false;
  @Input() weekStart!: Date;

  weekDays = [
    { name: 'Пн', index: 1 },
    { name: 'Вт', index: 2 },
    { name: 'Ср', index: 3 },
    { name: 'Чт', index: 4 },
    { name: 'Пт', index: 5 },
    { name: 'Сб', index: 6 },
    { name: 'Вс', index: 7 },
  ];

  timeSlots = [
    { start: '08:30', end: '10:00' },
    { start: '10:15', end: '11:45' },
    { start: '12:00', end: '13:30' },
    { start: '14:00', end: '15:30' },
    { start: '15:45', end: '17:15' },
  ];

  private timeToMinutes(time: string): number {
    const [h, m] = time.split(':').map(Number);
    return h * 60 + m;
  }

  getEventForCell(dayIndex: number, slot: { start: string, end: string }): ScheduleEvent | null {
    if (!this.weekStart) return null;
    const slotStartMinutes = this.timeToMinutes(slot.start);
    const slotEndMinutes = this.timeToMinutes(slot.end);

    return this.events.find(event => {
      const eventDate = new Date(event.startTime);
      const eventDay = eventDate.getDay() || 7;
      const eventMinutes = eventDate.getHours() * 60 + eventDate.getMinutes();

      // проверяем день недели
      if (eventDay !== dayIndex) return false;

      // проверяем, что событие попадает в слот
      return eventMinutes >= slotStartMinutes && eventMinutes < slotEndMinutes;
    }) || null;
  }
}

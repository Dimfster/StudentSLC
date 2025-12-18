import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EventsService, EventCreateModel, User, Room } from '../../../../../services/events';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-create',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create.html',
})
export class AdminCreate implements OnInit {
  model: any = {};
  students: User[] = [];
  teachers: User[] = [];
  rooms: Room[] = [];

  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private eventsService: EventsService, private cd: ChangeDetectorRef) {}

  ngOnInit() {
    this.eventsService.getAllStudents().subscribe({
      next: (users) => {
        this.students = users.filter(u => u.role === 'student');
        this.cd.markForCheck();
      },
      error: (err) => console.error('Ошибка загрузки студентов:', err)
    });

    this.eventsService.getAllTeachers().subscribe({
      next: (users) => {
        this.teachers = users;
        this.cd.markForCheck();
      },
      error: (err) => console.error('Ошибка загрузки преподавателей:', err)
    });

    this.eventsService.getAllRooms().subscribe({
      next: (rooms) => {
        this.rooms = rooms;
        this.cd.markForCheck();
      },
      error: (err) => console.error('Ошибка загрузки комнат:', err)
    });
  }

  create() {
    this.errorMessage = null;
    this.successMessage = null;

    if (!this.model.name || !this.model.date || !this.model.start || !this.model.end || !this.model.roomName || !this.model.participantId || !this.model.keyHolderId) {
      this.errorMessage = 'Заполните все обязательные поля';
      return;
    }

    const startTime = new Date(`${this.model.date}T${this.model.start}`).toISOString();
    const endTime = new Date(`${this.model.date}T${this.model.end}`).toISOString();

    const event: EventCreateModel = {
      name: this.model.name,
      startTime,
      endTime,
      roomName: this.model.roomName,
      participantId: this.model.participantId,
      keyHolderId: this.model.keyHolderId,
    };

    this.eventsService.createEvent(event).subscribe({
      next: () => {
        this.successMessage = 'Событие успешно создано!';
        this.model = {};
        this.cd.markForCheck();
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.error?.message || 'Ошибка создания события';
        this.cd.markForCheck();
      }
    });
  }
}

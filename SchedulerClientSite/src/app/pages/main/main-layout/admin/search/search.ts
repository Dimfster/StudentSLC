import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminService, Group, Teacher, Room } from '../../../../../services/admin';

@Component({
  selector: 'app-admin-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.html',
  styleUrls: ['./search.css'],
})
export class AdminSearch implements OnInit {
  type: 'groups' | 'teachers' | 'rooms' = 'groups';

  groups: Group[] = [];
  teachers: Teacher[] = [];
  rooms: Room[] = [];

  editingItem: Group | Teacher | Room | null = null;

  constructor(private adminService: AdminService, private cd: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    if (this.type === 'groups') {
      this.adminService.getGroups().subscribe(data => { this.groups = data; this.cd.markForCheck(); });
    } else if (this.type === 'teachers') {
      this.adminService.getTeachers().subscribe(data => { this.teachers = data; this.cd.markForCheck(); });
    } else if (this.type === 'rooms') {
      this.adminService.getRooms().subscribe(data => { this.rooms = data; this.cd.markForCheck(); });
    }
  }

  onTypeChange(newType: 'groups' | 'teachers' | 'rooms') {
    this.type = newType;
    this.editingItem = null;
    this.loadData();
  }

  editItem(item: Group | Teacher | Room) {
    this.editingItem = { ...item };
  }

  saveItem() {
    if (!this.editingItem) return;

    if ('id' in this.editingItem) {
      this.adminService.updateGroup(this.editingItem).subscribe(() => { this.loadData(); this.editingItem = null; });
    } else if ('userCode' in this.editingItem) {
      this.adminService.updateTeacher(this.editingItem).subscribe(() => { this.loadData(); this.editingItem = null; });
    } else if ('type' in this.editingItem) {
      this.adminService.updateRoom(this.editingItem).subscribe(() => { this.loadData(); this.editingItem = null; });
    }
  }

  cancelEdit() {
    this.editingItem = null;
  }

  deleteItem(item: Group | Teacher | Room) {
    if ('id' in item) {
      if (confirm(`Удалить группу ${item.name}?`)) {
        this.adminService.deleteGroup(item.name).subscribe(() => this.loadData());
      }
    } else if ('userCode' in item) {
      if (confirm(`Удалить преподавателя ${item.firstName} ${item.lastName}?`)) {
        this.adminService.deleteTeacher(item.userCode).subscribe(() => this.loadData());
      }
    } else if ('type' in item) {
      if (confirm(`Удалить аудиторию ${item.name}?`)) {
        this.adminService.deleteRoom(item.name).subscribe(() => this.loadData());
      }
    }
  }
}

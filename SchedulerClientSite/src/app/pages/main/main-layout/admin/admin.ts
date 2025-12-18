import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminSearch } from './search/search';
import { AdminCreate } from './create/create';

@Component({
  selector: 'app-admin-page',
  standalone: true,
  imports: [CommonModule, AdminSearch, AdminCreate],
  templateUrl: './admin.html',
  styleUrls: ['./admin.css'],
})
export class AdminPage {
  tab: 'search' | 'create' = 'search';
}
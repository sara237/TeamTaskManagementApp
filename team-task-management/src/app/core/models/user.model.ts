export type UserRole = 'Admin' | 'Member';

export interface User {
  id: string;
  name: string;
  role: UserRole;
}

export const TEAM_MEMBERS: User[] = [
  { id: '100', name: 'AdminUser', role: 'Admin' }, // The admin
  { id: '1', name: 'Alice', role: 'Member' },
  { id: '2', name: 'Bob', role: 'Member' },
  { id: '3', name: 'Charlie', role: 'Member' }
];
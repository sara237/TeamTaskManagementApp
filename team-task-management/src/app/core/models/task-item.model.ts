import { Priority } from "../enums/priority.enum";
import { TaskStatus } from "../enums/task-status.enum";

export interface TaskItem {
  id: string;
  title: string;
  description: string;
  status: TaskStatus;
  priority: Priority;
  assignedUserId: string;
  dueDate?: string; // ISO string from API
  createdAt?: string;
  updatedAt?: string | null;
}
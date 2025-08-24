export interface ChatMessage {
  id: string;
  senderId: string;
  content: string;
  timestamp: string;
  senderName: string;
  senderRole: 'Admin' | 'Member';
}
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject, BehaviorSubject, tap } from 'rxjs';
import { ChatMessage } from '../models/chat-message.model';
import { environment } from '../../../environments/environment.development';
import { CurrentUserService } from './current-user.service';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private hub?: signalR.HubConnection;

private readonly _connected$ = new BehaviorSubject<boolean>(false);
  connected$ = this._connected$.asObservable();

  private _messages: ChatMessage[] = [];
  private _messages$ = new BehaviorSubject<ChatMessage[]>([]);
  messages$ = this._messages$.asObservable();

  constructor(
    private currentUserService: CurrentUserService,
    private http: HttpClient
  ) {}

  start(): void {
     if (this.hub) return;

  this.hub = new signalR.HubConnectionBuilder()
    .withUrl(environment.hubUrl, { withCredentials: true })
    .withAutomaticReconnect()
    .build();

  this.hub.on('ReceiveMessage', (msg: ChatMessage) => {
    const user = this.currentUserService.currentUser;
    if (!user) return;

    if (user.role === 'Admin' || msg.senderId === user.id) {
      this.addMessage(msg);
    }
  });

  this.hub.start()
    .then(() => {
      this._connected$.next(true);
      console.log('SignalR connected');

      // ✅ fetch persisted history from API on start
      const user = this.currentUserService.currentUser;
      if (user) {
        this.fetchMessages(user.id).subscribe();
      }
    })
    .catch(err => console.error('SignalR start error:', err));
}

  sendMessage(content: string): Promise<void> {
    const user = this.currentUser;
    if (!this.hub) throw new Error('Hub not started');
    return this.hub.invoke('SendMessage', user.id, content);
  }

  get currentUser() {
    const user = this.currentUserService.currentUser;
    if (!user) throw new Error('No current user set');
    return user;
  }

  /** Filter messages according to user role */
  private filterMessages(messages: ChatMessage[]): ChatMessage[] {
    const user = this.currentUser;
    if (user.role === 'Admin') {
      return messages; // ✅ Admin sees all
    }
    return messages.filter(m => m.senderId === user.id); // ✅ Member sees only their own
  }

  /** Add new message */
  addMessage(msg: ChatMessage) {
    this._messages.push(msg);
    this._messages$.next(this.filterMessages(this._messages));
  }

  /** Replace entire message history */
  setMessages(messages: ChatMessage[]) {
    this._messages = messages;
    this._messages$.next(this.filterMessages(this._messages));
  }

  /** Snapshot of current filtered messages */
  getMessages(): ChatMessage[] {
    return this.filterMessages(this._messages);
  }

  /** Load from API */
  fetchMessages(userId: string) {
     return this.http
    .get<ChatMessage[]>(`${environment.apiBaseUrl}/chat?userId=${userId}`)
    .pipe(
      tap(messages => this.setMessages(messages))
    );
  }
}

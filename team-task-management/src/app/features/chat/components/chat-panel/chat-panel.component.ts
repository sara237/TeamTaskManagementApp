import { Component, OnDestroy, OnInit } from '@angular/core';
import { ChatMessage } from '../../../../core/models/chat-message.model';
import { ChatService } from '../../../../core/services/chat.service';
import { CurrentUserService } from '../../../../core/services/current-user.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-chat-panel',
  standalone: false,
  templateUrl: './chat-panel.component.html',
  styleUrls: ['./chat-panel.component.scss']
})
export class ChatPanelComponent implements OnInit, OnDestroy {
  messages: ChatMessage[] = [];
  newMessage = '';
  private subs: Subscription[] = [];

  constructor(public chatService: ChatService) {}

  ngOnInit(): void {
    this.chatService.start();
    const user = this.chatService.currentUser
     this.messages = this.chatService.getMessages();

  // Subscribe to updates
  const sub = this.chatService.messages$.subscribe(msgs => {
    this.messages = msgs;
    setTimeout(() => {
      const container = document.getElementById('chat-container');
      container?.scrollTo({ top: container.scrollHeight, behavior: 'smooth' });
    }, 0);
  });
  this.subs.push(sub);

  // Fetch from API only if empty
  if (this.messages.length === 0) {
    this.chatService.fetchMessages(user.id).subscribe();
  }
}

  protected send(): void {
    if (!this.newMessage.trim()) return;

    this.chatService.sendMessage(this.newMessage)
      .then(() => this.newMessage = '')
      .catch(err => console.error('Send message error:', err));
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }
}

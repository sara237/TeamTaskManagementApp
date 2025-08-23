import { Component, OnInit } from '@angular/core';
import { ChatMessage } from '../../../../core/models/chat-message.model';
import { ChatService } from '../../../../core/services/chat.service';


@Component({
  selector: 'app-chat-panel',
  standalone: false,
  templateUrl: './chat-panel.component.html',
  styleUrls: ['./chat-panel.component.scss']
})
export class ChatPanelComponent implements OnInit {
  messages: ChatMessage[] = [];
  input = '';
  me = 'User-' + Math.floor(Math.random() * 10000);

  connected = false;

  constructor(private chat: ChatService) {}

  ngOnInit(): void {
    this.chat.connected$.subscribe(c => this.connected = c);
    this.chat.messages$.subscribe(m => this.messages.push(m));
    this.chat.start();
  }

  send() {
    debugger;
    const text = this.input.trim();
    if (!text) return;
    this.chat.sendMessage(this.me, text).catch(err => console.error(err));
    this.input = '';
  }
}

import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { MessageParams } from '../_models/messageParams';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  messages?: Message[];
  pagination?: Pagination;
  container = 'Unread';
  messageParams?: MessageParams;
  loading = false;

  constructor(private messageService: MessageService) {
    this.messageParams = this.messageService.getMessageParams();
  }

  ngOnInit(): void {
    console.log(this.pagination);
    this.loadMessages();
  }

  loadMessages() {
    this.loading = true;

    if (this.messageParams) {
      this.messageParams.container = this.container;
      this.messageService.setMessageParams(this.messageParams);
      this.messageService
        .getMessages(
          this.messageParams
        )
        .subscribe({
          next: (response) => {
            if (response.result && response.pagination) {
              this.messages = response.result;
              this.pagination = response.pagination;
              this.loading = false;
            }
          },
        });
    }
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => this.messages?.splice(this.messages.findIndex(x => x.id === id), 1)
    });
  }

  pageChanged(event: any) {
    if (this.messageParams && this.messageParams?.pageNumber !== event.page) {
      this.messageParams.pageNumber = event.page;
      this.messageService.setMessageParams(this.messageParams);
      this.loadMessages();
    }
  }
}

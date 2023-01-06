import { User } from "./user";

export class MessageParams {
  container = 'Unread';

  pageNumber = 1;
  pageSize = 5;
  orderBy = 'messageSent';

  constructor() {}
}
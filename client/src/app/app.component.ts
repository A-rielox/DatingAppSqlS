import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
   selector: 'app-root',
   templateUrl: './app.component.html',
   styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
   users: any;

   constructor(
      private http: HttpClient,
      private accountService: AccountService
   ) {}

   ngOnInit(): void {
      this.setCurrentUser();
      this.getUsers();
   }

   getUsers() {
      this.http.get('https://localhost:5001/api/Users').subscribe({
         next: (users) => {
            this.users = users;
         },
         error: (err) => console.log(err),
      });
   }

   setCurrentUser() {
      const userStr = localStorage.getItem('user');

      if (!userStr) return;

      const user: User = JSON.parse(userStr);
      this.accountService.setCurrentUser(user);
   }
}

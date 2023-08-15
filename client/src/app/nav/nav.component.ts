import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
   selector: 'app-nav',
   templateUrl: './nav.component.html',
   styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
   model: any = {};

   constructor(public accontService: AccountService) {}

   ngOnInit(): void {}

   login() {
      this.accontService.login(this.model).subscribe({
         next: (res) => {
            console.log(res);
         },
         error: (err) => console.log(err),
      });
   }

   logout() {
      this.accontService.logout();
   }
}

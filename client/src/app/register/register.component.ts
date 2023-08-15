import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';

@Component({
   selector: 'app-register',
   templateUrl: './register.component.html',
   styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
   @Output() cancelRegister = new EventEmitter();
   @Input() usersFromHomeComp: User[] = [];

   model: any = {};

   constructor(
      private accountService: AccountService, // private toastr: ToastrService
      private toastr: ToastrService
   ) {}

   ngOnInit(): void {}

   register() {
      console.log(this.model);

      this.accountService.register(this.model).subscribe({
         next: (res) => {
            console.log(res);
            this.cancel(); // cierro el register form
         },
         error: (err) => {
            this.toastr.error(err.error + '  💩');
         },
      });
   }

   cancel() {
      this.cancelRegister.emit(false);
   }
}

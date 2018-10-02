import { AuthService } from './../../service/auth.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup,Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import {DisableControlDirective} from '../../DisableControlDirective'
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
userForm:FormGroup
  user:string
  loading = false;
submitted = false;
userdisabled=true;
disableControl:DisableControlDirective
error = '';
  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router,
    private authservice:AuthService) { 
      this.userForm = this.formBuilder.group({
        id:[],
        firstname: ['', Validators.required],
        lastname: ['', Validators.required],
        username: ['', Validators.required]
    });
    if(localStorage.getItem('user'))
    this.user=localStorage.getItem('user')
      this.authservice.getUserById()
       .subscribe(data=>{
         this.userForm.setValue(data)
       },
       error=>{
         this.error=error
       })
    }
  ngOnInit() {
  }
    get f() { return this.userForm.controls; }
    onSubmit() {
      this.submitted = true;
      this.loading = true;
    }
}

import { Component, OnInit } from '@angular/core';
import {FormBuilder,FormGroup,Validators} from '@angular/forms'
import { ProductService } from '../../service/product.service';
import { Router } from '@angular/router';
import { AmazingTimePickerService } from 'amazing-time-picker';
@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  addForm:FormGroup
  user:string
  message:string
  loading = false;
  error=''
    public maxDate: Date = new Date ();
  constructor(private formBuilder:FormBuilder,private service:ProductService,private router:Router,private atp: AmazingTimePickerService) {
    this.user=localStorage.getItem('user')
   }

  ngOnInit() {
    this.addForm=this.formBuilder.group({
      id:[Math.random().toString(36).substring(2, 15),null],
      name:['',Validators.required],
      watch:[true,null],
      detail:[null,null],
      date:[null,null],
      amountlost:[null,null],
      location:[null,null],
      eventno:['',Validators.required],
      time:[null,null]
    })
  }
  onSubmit(){
    this.loading = true;
    this.service.addProduct(this.addForm.value)
    .subscribe(data=>{
      this.message=data['name'] +'added'
      this.router.navigate([''])
    },
    error => {
        this.error = error;
        this.loading = false;
        this.router.navigate(['/login'])
      })
  }
  open(ev: any) {
    const amazingTimePicker = this.atp.open();
    amazingTimePicker.afterClose().subscribe(time => {
      console.log(time);
    });
  }
back(){
  this.router.navigate([''])
}
}

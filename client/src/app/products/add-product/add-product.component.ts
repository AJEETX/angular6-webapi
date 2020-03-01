import { Component, OnInit } from '@angular/core';
import {FormBuilder,FormGroup,Validators} from '@angular/forms'
import { ProductService } from '../../service/product.service';
import { Router } from '@angular/router';
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
  constructor(private formBuilder:FormBuilder,private service:ProductService,private router:Router) {
    this.user=localStorage.getItem('user')
   }

  ngOnInit() {
    this.addForm=this.formBuilder.group({
      id:[Math.random().toString(36).substring(2, 15),null],
      name:['',Validators.required],
      watch:[true,null],
      detail:[null,null],
      time:[null,null],
      amountlost:[null,null],
      location:[null,null],
      eventno:['',Validators.required]
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
back(){
  this.router.navigate([''])
}
}

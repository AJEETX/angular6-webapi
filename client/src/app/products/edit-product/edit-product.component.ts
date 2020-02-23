import { ProductService } from '../../service/product.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {
formEdit:FormGroup
user:string
loading=false
  constructor(private formBuilder:FormBuilder,private service:ProductService,private router:Router) { 
    this.user=localStorage.getItem('user')
  }

  ngOnInit() {
    let id=localStorage.getItem('id')
    this.formEdit=this.formBuilder.group({
      id:[],
      name:['',Validators.required],
      watch:[false,null],
      detail:['',Validators.required]
    })
    this.service.getProductById(+id)
    .subscribe(data=>{
      this.formEdit.setValue(data)
    })
  }
  onSubmit(){
    this.loading = true;

    this.service.editProduct(this.formEdit.value)
    .subscribe(data=>{
      this.router.navigate([''])
    })
  }
  back(){
    this.router.navigate([''])
  }
}

import { ProductService } from '../../service/product.service';
import { Product } from './../../product';
import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products:Product[]
  user:string
  loading=false
  error=''
  searchField: FormControl
  searches: string[] = [];
  constructor(private router:Router, private service:ProductService) {
    this.user=localStorage.getItem('user')
    this.service.getProducts()
    .subscribe(data=>{
      this.products=data
    })
   }
  ngOnInit() {
    this.searchField = new FormControl()
    this.searchField.valueChanges
    .pipe(
      debounceTime(400),
      distinctUntilChanged()
    )
    .subscribe(term => {
      console.log(term)
      this.service.getProducts(term)
      .subscribe(data=>{
        console.log(data)
        this.products=data
      })
    });
  }
  addProduct():void{
    this.loading=true
    this.router.navigate(['add-product'])
  }
  editProduct(product: Product): void {
    this.loading=true
    console.log(product.id)
    localStorage.removeItem("id");
    localStorage.setItem("id", product.id.toString());
    this.router.navigate(['edit-product']);
  };
  deleteProduct(product: Product): void {
    this.loading=true

    this.service.delete(product.id)
      .subscribe( data => {
      this.loading=false

        this.products = this.products.filter(u => u !== product);
      },
      error => {
          this.error = error;
          this.loading = false;
      })
  };
}

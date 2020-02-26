import { Product } from '../product';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ProductService {
baseUrl:string="http://localhost:5000/products"
  constructor(private http:HttpClient) { }
getProducts(query?:string){
  return this.http.get<Product[]>(this.baseUrl+ '?q='+query);
}
getProductById(id:number){
  return this.http.get<Product>(this.baseUrl+ '/' + id)
}
addProduct(product:Product){
  console.log(product)
  return this.http.post(this.baseUrl,product)
}
editProduct(product:Product){
  return this.http.put(this.baseUrl + '/' +product.id,product)
}
delete(id: number) {
  return this.http.delete(this.baseUrl + '/' + id);
}
}

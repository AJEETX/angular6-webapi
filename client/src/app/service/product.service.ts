import { Product } from '../product';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ProductService {
baseUrl:string="http://localhost:5000/products"
  constructor(private http:HttpClient) { }
getProducts(){
  return this.http.get<Product[]>(this.baseUrl);
}
getProductById(id:number){
  return this.http.get<Product>(this.baseUrl+ '/' + id)
}
addProduct(product:Product){
  return this.http.post(this.baseUrl,product)
}
editProduct(product:Product){
  return this.http.put(this.baseUrl + '/' +product.id,product)
}
delete(id: number) {
  return this.http.delete(this.baseUrl + '/' + id);
}
}

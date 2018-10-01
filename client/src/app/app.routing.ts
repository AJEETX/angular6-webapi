import { AuthGuard } from './auth/auth.guard';
import { EditProductComponent } from './products/edit-product/edit-product.component';
import { AddProductComponent } from './products/add-product/add-product.component';
import {RouterModule, Routes} from '@angular/router'
import {ProductListComponent} from './products/product-list/product-list.component'
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { UserComponent } from './user/user/user.component';

const routes:Routes=[
    {path:'register', component:RegisterComponent},
    {path:'login',  component:LoginComponent },
    { path:'', component:ProductListComponent, canActivate: [AuthGuard] },
    { path:'add-product', component:AddProductComponent, canActivate: [AuthGuard]},
    { path:'edit-product', component:EditProductComponent, canActivate: [AuthGuard] },
    { path:'user', component:UserComponent , canActivate: [AuthGuard] },
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];
export const routing=RouterModule.forRoot(routes)
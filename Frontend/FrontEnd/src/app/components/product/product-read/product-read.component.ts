import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog'; // ✅ import corrigido
import { DialogComponent } from 'src/app/views/dialog/dialog.component';
import { Product } from '../product.model';
import { ProductService } from '../product.service';

@Component({
  selector: 'app-product-read',
  templateUrl: './product-read.component.html',
  styleUrls: ['./product-read.component.css']
})
export class ProductReadComponent implements OnInit {

  product!: Product;
  products: Product[] = [];
  displayedColumns = ['id', 'name', 'price', 'action'];
  
  constructor(private productService: ProductService, public dialog: MatDialog) { }

  ngOnInit() {
    this.productService.read().subscribe(products => {
      this.products = products;
      console.log(this.products);
    });
  }

  openDialog(id: number): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '20%',     
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      console.log('The dialog was closed');
    });

    this.productService.readByID(id).subscribe(product => {
      this.product = product;

      this.productService.getID.emit(id);  
    });
  }
}

import { Component, OnInit, Input } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Product } from 'src/app/components/product/product.model';
import { ProductService } from 'src/app/components/product/product.service';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {
  product!: Product;
  
  @Input() productId!: number;

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    private productService: ProductService, 
    private router: Router
  ) { }

  ngOnInit(): void {
    if (this.productId) {
      this.loadProduct(this.productId);
    }
  }

  private loadProduct(id: number): void {
    this.productService.readByID(id).subscribe({
      next: (product) => {
        this.product = product;
      },
      error: (err) => {
        this.productService.showMenssage('Erro ao carregar produto!', true);
      }
    });
  }

  deleteProduct(): void {
    if (this.product?.id) {
      this.productService.delete(this.product.id).subscribe({
        next: () => {
          this.productService.showMenssage('Produto excluído com sucesso!!');
          this.dialogRef.close(true); 
         },
        error: (err) => {
          this.productService.showMenssage('Erro ao excluir produto!', true);
        }
      });
    }
  }

  cancel(): void {
    this.dialogRef.close(false); 
  }
}
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../product.model';
import { ProductService } from './../product.service';

@Component({
  selector: 'app-product-update',
  templateUrl: './product-update.component.html',
  styleUrls: ['./product-update.component.css']
})
export class ProductUpdateComponent implements OnInit {

  product: Product = {} as Product;

  constructor(
    private productService: ProductService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.productService.readByID(+id).subscribe(product => {
        this.product = product;
      });
    }
  }

  updateProduct(): void {  
    this.productService.update(this.product).subscribe({
      next: () => {
        this.productService.showMenssage('Produto atualizado com sucesso!!');
        this.router.navigate(['/products']);
      },
      error: (err) => {
        this.productService.showMenssage('Erro ao atualizar produto!', true);
      }
    });
  }

  cancelProduct(): void {
    this.router.navigate(['/products']);
  }
}
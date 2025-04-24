import { Directive, OnInit,Input,ViewContainerRef,TemplateRef } from '@angular/core';

@Directive({
  selector: '[itsFor]'
})
export class ForDirective implements OnInit {

  @Input('itsForEm') numbers: number[] = [];

  constructor( 
    private container: ViewContainerRef, 
    private template: TemplateRef<any>
    ) { }
  ngOnInit():void{
    for(let number of this.numbers){
      this.container.createEmbeddedView(this.template)
    }
    
  }
}

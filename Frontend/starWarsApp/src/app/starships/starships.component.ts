import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, Input, OnInit, inject } from '@angular/core';
import { ModalCommunicationService } from '../modal-communication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-starships',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './starships.component.html',
  styleUrl: './starships.component.css'
})
export class StarshipsComponent implements OnInit {
  httpClient = inject(HttpClient);

  constructor(private modalService: ModalCommunicationService, private router: Router) {}
 starships:any= [];
 
    ngOnInit(): void {
      this.modalService.getStarShipIds().subscribe((data) => {
        if(data){
          this.fetchDataBasedOnIDs(data);
        }
     else{
       this.fetchAllData();
     }

      });
    }
      
  fetchAllData(){
      this.httpClient.get('http://localhost:7000/proxyapi/starships')
      .subscribe((data:any)=>{
        this.starships= data;
      });
    }
  
  fetchDataBasedOnIDs(ids:number[]){
    this.httpClient.post('http://localhost:7000/proxyapi/starships', ids)
    .subscribe((model:any)=>{
      this.starships = model;
    });
  }

  navigateToFilm(ids: number[])
  {
    this.modalService.setStarShipIds(ids);
    this.router.navigate(['/films', {queryParams:{ films: ids }}]);
  }
}
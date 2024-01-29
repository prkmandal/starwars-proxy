import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit, Output, inject } from '@angular/core';
import { ModalCommunicationService } from '../modal-communication.service';
import { StarshipsComponent } from '../starships/starships.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-films',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './films.component.html',
  styleUrl: './films.component.css'
})
export class FilmsComponent implements OnInit {
  httpClient = inject(HttpClient);
data:any=[];

constructor(private modalService: ModalCommunicationService, private route: ActivatedRoute, private router: Router){}

  ngOnInit(): void {

    this.modalService.getFilmsIds().subscribe((data) => {
      if(data){
        this.fetchDataBasedOnIDs(data);
      }
   else{
     this.fetchAllData();
   }
  });
  }

  fetchDataBasedOnIDs(ids:number[]){
    this.httpClient.post('http://localhost:7000/proxyapi/films', ids)
    .subscribe((model:any)=>{
      this.data = model;
    });
  }

       // Retrieve route parameters
      //  this.route.params.subscribe((queryParams) => {
      //   var receivedParam = queryParams['films'];
      //   if(receivedParam)
      //   {

      //   }
      //   else{
      //     this.fetchAllData();
      //   }
        
      // });

  

    fetchAllData(){
    this.httpClient.get('http://localhost:7000/proxyapi/films')
    .subscribe((data:any)=>{
      this.data= data
      console.log(data);
    });
  }

  navigateToStarship(ids: number[])
  {
    this.modalService.setStarShipIds(ids);
    this.router.navigate(['/starships']);
  }
}

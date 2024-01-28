import { Routes } from '@angular/router';
import { FilmsComponent } from './films/films.component';
import { StarshipsComponent } from './starships/starships.component';

export const routes: Routes = [
    {
        'path':'',component:FilmsComponent
    },
    {
        'path':'films',component:FilmsComponent
    },
    {
        'path':'starships',component:StarshipsComponent
    }
];

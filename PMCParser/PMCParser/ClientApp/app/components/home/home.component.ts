import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    public http: Http;

    public baseUrl: string;

    public exportToExcel: boolean;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.http = http;
        this.baseUrl = baseUrl;
        this.exportToExcel = true;
    }

    analyzeArticles() {
        this.exportToExcel = true;
        this.http.get(this.baseUrl + 'api/PM/AnalyzeArticles').subscribe(result => {
            this.exportToExcel = result.json() as boolean;
        }, error => console.error(error));
    }

    exportData() {
        this.exportToExcel = true;
        this.http.get(this.baseUrl + 'api/PM/ExportData').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }
}

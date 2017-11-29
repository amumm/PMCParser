import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    public http: Http;

    public baseUrl: string;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.http = http;
        this.baseUrl = baseUrl;
    }

    analyzeArticles() {
        this.http.get(this.baseUrl + 'api/PM/AnalyzeArticles').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }

    exportData() {
        this.http.get(this.baseUrl + 'api/PM/ExportData').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }
}

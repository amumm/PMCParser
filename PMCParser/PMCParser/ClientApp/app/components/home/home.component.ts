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

    public results: JournalPaper[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.http = http;
        this.baseUrl = baseUrl;
        this.exportToExcel = true;
    }

    analyzeArticles() {
        this.exportToExcel = true;
        this.http.get(this.baseUrl + 'api/PM/AnalyzeArticles').subscribe(result => {
            this.results = result.json() as JournalPaper[];
        }, error => console.error(error));
    }

    exportData() {
        this.exportToExcel = true;
        this.http.get(this.baseUrl + 'api/PM/ExportData').subscribe(result => {
            var output = result;
        }, error => console.error(error));
    }


}

interface JournalPaper {
    journalTitle: string;
    paperTitle: string;
    issue: string;
    volume: string;
    date: string;
    pmcid: string;
    authors: string[];
    correspondingAuthors: string[];
    dataTypes: DataType[];
}

interface DataType {
        name: string;
        keywords: string[];
}

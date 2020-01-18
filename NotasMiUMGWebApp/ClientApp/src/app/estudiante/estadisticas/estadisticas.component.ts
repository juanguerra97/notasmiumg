import { Component, OnInit } from '@angular/core';
import { EstadisticaService } from '../../services/estadistica.service';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import ServerResponse from '../../model/ServerResponse';

@Component({
  selector: 'app-estadisticas',
  templateUrl: './estadisticas.component.html',
  styleUrls: ['./estadisticas.component.css']
})
export class EstadisticasComponent implements OnInit {

  public promedioGeneral = 0;
  public totalCreditos = 0;
  public numCursos = 0;
  public cursosAprobados = 0;
  public cursosReprobados = 0;

  public maxPrimerParcial = 0;
  public minPrimerParcial = 0;
  public cursosMaxPrimerParcial: any[] = [];
  public cursosMinPrimerParcial: any[] = [];

  public maxSegundoParcial = 0;
  public minSegundoParcial = 0;
  public cursosMaxSegundoParcial: any[] = [];
  public cursosMinSegundoParcial: any[] = [];

  promedioAnualChartOptions: ChartOptions = {
    responsive: true,
  };
  promedioAnualChartLabels: Label[] = [];
  promedioAnualChartType: ChartType = 'bar';
  promedioAnualChartLegend = true;
  promedioAnualChartPlugins = [];
  promedioAnualChartData: ChartDataSets[] = [
  ];
  public mostrarChartPromedioAnual = false;

  promedioSemestralChartOptions: ChartOptions = {
    responsive: true,
  };
  promedioSemestralChartLabels: Label[] = [];
  promedioSemestralChartType: ChartType = 'line';
  promedioSemestralChartLegend = true;
  promedioSemestralChartPlugins = [];
  promedioSemestralChartData: ChartDataSets[] = [];
  public mostrarChartPromedioSemestral = false;

  constructor(private estadisticaService: EstadisticaService) { }

  ngOnInit() {
    this.estadisticaService.getResumen()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.promedioGeneral = res.data.promedio;
          this.totalCreditos = res.data.creditos;
          this.numCursos = res.data.cursos.total;
          this.cursosAprobados = res.data.cursos.aprobados;
          this.cursosReprobados = res.data.cursos.reprobados;
        }
      }, console.error);

    this.estadisticaService.getPromedios()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.crearChartPromedioAnual(res.data.anual);
          this.crearChartPromedioSemestral(res.data.semestral);
        }
      }, console.error);

    this.estadisticaService.getPrimerosParciales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.maxPrimerParcial = res.data.max.val;
          this.cursosMaxPrimerParcial = res.data.max.cursos;
          this.minPrimerParcial = res.data.min.val;
          this.cursosMinPrimerParcial = res.data.min.cursos;
        }
      }, console.error);

    this.estadisticaService.getSegundosParciales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.maxSegundoParcial = res.data.max.val;
          this.cursosMaxSegundoParcial = res.data.max.cursos;
          this.minSegundoParcial = res.data.min.val;
          this.cursosMinSegundoParcial = res.data.min.cursos;
        }
      }, console.error);

  }

  private crearChartPromedioAnual(promediosAnual: any[]): void {
    this.promedioAnualChartLabels = promediosAnual.map(p => p.ano);
    this.promedioAnualChartData = [{data: promediosAnual.map(p => p.promedio), label: 'Promedio anual'}];
    this.mostrarChartPromedioAnual = true;
  }

  private crearChartPromedioSemestral(promediosSemestral: any[]): void {
    promediosSemestral = promediosSemestral.map(r => r.semestres.map(p =>{ return { ano: r.ano, semestre: p.semestre, promedio: p.promedio }})).reduce((acc, curr) => { acc.push(...curr); return acc;}, []);
    this.promedioSemestralChartLabels = promediosSemestral.map(p => `${p.semestre}/${p.ano}`);
    this.promedioSemestralChartData = [ {data: promediosSemestral.map(p => p.promedio), label: 'Promedio semestral'}];
    this.mostrarChartPromedioSemestral = true;
  }


}

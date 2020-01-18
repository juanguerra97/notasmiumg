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

  public maxExamenFinal = 0;
  public minExamenFinal = 0;
  public cursosMaxExamenFinal: any[] = [];
  public cursosMinExamenFinal: any[] = [];

  public maxPrimerParcial = 0;
  public minPrimerParcial = 0;
  public cursosMaxPrimerParcial: any[] = [];
  public cursosMinPrimerParcial: any[] = [];

  public maxSegundoParcial = 0;
  public minSegundoParcial = 0;
  public cursosMaxSegundoParcial: any[] = [];
  public cursosMinSegundoParcial: any[] = [];

  public maxActividades = 0;
  public minActividades = 0;
  public cursosMaxActividades: any[] = [];
  public cursosMinActividades: any[] = [];

  public maxZona = 0;
  public minZona = 0;
  public cursosMaxZona: any[] = [];
  public cursosMinZona: any[] = [];

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

    this.estadisticaService.getExamenesFinales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.maxExamenFinal = res.data.max.val;
          this.cursosMaxExamenFinal = res.data.max.cursos;
          this.minExamenFinal = res.data.min.val;
          this.cursosMinExamenFinal = res.data.min.cursos;
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

    this.estadisticaService.getActividades()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.maxActividades = res.data.max.val;
          this.cursosMaxActividades = res.data.max.cursos;
          this.minActividades = res.data.min.val;
          this.cursosMinActividades = res.data.min.cursos;
        }
      }, console.error);

    this.estadisticaService.getZonas()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.maxZona = res.data.max.val;
          this.cursosMaxZona = res.data.max.cursos;
          this.minZona = res.data.min.val;
          this.cursosMinZona = res.data.min.cursos;
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

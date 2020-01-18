import { Component, OnInit } from '@angular/core';
import { EstadisticaService } from '../../services/estadistica.service';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import ServerResponse from '../../model/ServerResponse';

// interfaz para representar las estadisticas
// de las nota(nota final, zona, parciales, etc.)
interface EstadisticaNota {
  max: number;
  maxCursos: any[];
  min: number;
  minCursos: any[];
}

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

  public notasFinales: EstadisticaNota = null;
  public zonas: EstadisticaNota = null;
  public primerosParciales: EstadisticaNota = null;
  public segundosParciales: EstadisticaNota = null;
  public examenesFinales: EstadisticaNota = null;
  public actividades: EstadisticaNota = null;

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

    this.estadisticaService.getNotasFinales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.notasFinales = this.responseToEstadistica(res.data);
        }
      }, console.error);

    this.estadisticaService.getZonas()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.zonas = this.responseToEstadistica(res.data);
        }
      }, console.error);

    this.estadisticaService.getPrimerosParciales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.primerosParciales = this.responseToEstadistica(res.data);
        }
      }, console.error);

    this.estadisticaService.getSegundosParciales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.segundosParciales = this.responseToEstadistica(res.data);
        }
      }, console.error);

    this.estadisticaService.getActividades()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.actividades = this.responseToEstadistica(res.data);
        }
      }, console.error);

    this.estadisticaService.getExamenesFinales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.examenesFinales = this.responseToEstadistica(res.data);
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

  // funcion que transforma los datos de la respuesta de un endpoint para una estadistica
  // individual de una nota en un objeto del tipo EstadisticaNota
  private responseToEstadistica(data: any): EstadisticaNota {
    return {
      max: data.max.val,
      maxCursos: data.max.cursos,
      min: data.min.val,
      minCursos: data.min.cursos
    };
  }

}

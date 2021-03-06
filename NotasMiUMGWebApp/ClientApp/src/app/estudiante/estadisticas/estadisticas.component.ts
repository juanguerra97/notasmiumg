import { Component, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { faTimes } from '@fortawesome/free-solid-svg-icons';

import { EstadisticaService } from '../../services/estadistica.service';
import ServerResponse from '../../model/ServerResponse';

// interfaz para representar las estadisticas
// de las nota(nota final, zona, parciales, etc.)
interface EstadisticaNota {
  nombre: string; // nombre de la estadistica
  max: number;
  maxCursos: CursoEstadisticaNota[];
  min: number;
  minCursos: CursoEstadisticaNota[];
}

// datos de los cursos que aparecen en las estadisticas de las notas
interface CursoEstadisticaNota {
  nombreCurso: string;
  ano: number;
  aprobado: boolean;
}

@Component({
  selector: 'app-estadisticas',
  templateUrl: './estadisticas.component.html',
  styleUrls: ['./estadisticas.component.css']
})
export class EstadisticasComponent implements OnInit {

  faTimes = faTimes;

  public promedioGeneral = 0;
  public totalCreditos = 0;
  public numCursos = 0;
  public cursosAprobados = 0;
  public cursosReprobados = 0;

  // array con las estadisticas de las notas
  // cada estadistica es null miestras no se hallan cargado sus datos
  public estadisticasNotas: EstadisticaNota[] = [ null, null, null, null, null, null];

  public cursosModal: CursoEstadisticaNota[] = [];

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

  constructor(
    private estadisticaService: EstadisticaService,
    private modalService: NgbModal
  ) { }

  ngOnInit() {

    // indices de cada estadistica en el array
    const indexNotasFinales = 0;
    const indexZonas = indexNotasFinales + 1;
    const indexPrimerosParciales = indexZonas + 1;
    const indexSegundosParciales = indexPrimerosParciales + 1;
    const indexExamenesFinales = indexSegundosParciales + 1;
    const indexActividades = indexExamenesFinales + 1;

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
          this.estadisticasNotas[indexNotasFinales] = this.responseToEstadistica(res.data, 'Nota Final');
        }
      }, console.error);

    this.estadisticaService.getZonas()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.estadisticasNotas[indexZonas] = this.responseToEstadistica(res.data, 'Zona');
        }
      }, console.error);

    this.estadisticaService.getPrimerosParciales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.estadisticasNotas[indexPrimerosParciales] = this.responseToEstadistica(res.data, 'Primer Parcial');
        }
      }, console.error);

    this.estadisticaService.getSegundosParciales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.estadisticasNotas[indexSegundosParciales] = this.responseToEstadistica(res.data, 'Segundo Parcial');
        }
      }, console.error);

    this.estadisticaService.getActividades()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.estadisticasNotas[indexActividades] = this.responseToEstadistica(res.data, 'Actividades');
        }
      }, console.error);

    this.estadisticaService.getExamenesFinales()
      .subscribe((res: ServerResponse) => {
        if(res.status == 200) {
          this.estadisticasNotas[indexExamenesFinales] = this.responseToEstadistica(res.data, 'Examen Final');
        }
      }, console.error);

  }

  public openModal(content, cursos: CursoEstadisticaNota[]): void {

    this.cursosModal = cursos;

    this.modalService.open(content, {
      centered: true,
      size: 'lg',
      windowClass: 'animated bounceIn'
    });

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
  private responseToEstadistica(data: any, nombre: string): EstadisticaNota {
    return {
      nombre,
      max: data.max.val,
      maxCursos: data.max.cursos,
      min: data.min.val,
      minCursos: data.min.cursos
    };
  }

}

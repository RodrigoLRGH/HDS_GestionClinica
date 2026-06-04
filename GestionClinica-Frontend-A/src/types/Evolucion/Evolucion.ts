import { type Doctor } from "../Doctor/Doctor";
import { type HistoriaClinica } from "../HistoriaClinica/HistoriaClinica";

export interface Evolucion {
    id: number;
    idHistoriaClinica: number;
    idDoctor: number;
    fecha: string;
    diagnostico: string;
    tratamiento: string;
    notas?: string;
    activo: boolean;
    doctor?: Doctor;
    historiaClinica?: HistoriaClinica;
}
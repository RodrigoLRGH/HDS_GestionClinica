import { type Doctor } from "../Doctor/Doctor";
import { type Paciente } from "../Paciente/Paciente";

export interface Cita {
    id: number;
    idPaciente: number;
    idDoctor: number;
    fechaHora: string;
    motivo: string;
    notas?: string;
    estado: number;
    paciente?: Paciente;
    doctor?: Doctor;
}

export interface CitaRespuestaB {
    id: number;
    idPaciente: number;
    nombrePaciente: string;
    idDoctor: number;
    nombreDoctor: string;
    nombreEspecialidad: string;
    fechaHora: string;
    motivo: string;
    notas?: string;
    estado: number;
    fechaCreacion: string;
    fechaModificacion?: string | null;
    fechaEliminacion?: string | null;
    eliminado: boolean;
}
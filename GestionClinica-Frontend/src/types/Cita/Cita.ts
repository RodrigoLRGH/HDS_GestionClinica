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
import { type Especialidad } from "../Especialidad/Especialidad";

export interface Doctor {
    id: number;
    nombres: string;
    apellidos: string;
    especialidad?: Especialidad;
    idEspecialidad: number;
    activo: boolean;
    nombreCompleto: string;
}
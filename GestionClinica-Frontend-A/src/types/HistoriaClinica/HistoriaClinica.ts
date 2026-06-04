import {type Paciente } from "../Paciente/Paciente";

export interface HistoriaClinica {
    id: number;
    idPaciente: number;
    fechaApertura: string;
    alergias?: string;
    antecedentesFamiliares?: string;
    antecedentesPersonales?: string;
    activo: boolean;
    paciente?: Paciente;
}

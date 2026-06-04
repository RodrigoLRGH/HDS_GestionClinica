export interface ActualizarCitaDTO {
    id: number;
    idPaciente: number;
    idDoctor: number;
    fechaHora: string;
    motivo: string;
    notas?: string;
    estado: number;
}
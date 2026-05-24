export interface CrearCitaDTO {
    idPaciente: number;
    idDoctor: number;
    fechaHora: string;
    motivo: string;
    notas?: string;
    estado: number;
}
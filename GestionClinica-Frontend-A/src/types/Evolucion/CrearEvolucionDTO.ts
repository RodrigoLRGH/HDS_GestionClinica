export interface CrearEvolucionDTO {
    idHistoriaClinica: number;
    idDoctor: number;
    fecha: string;
    diagnostico: string;
    tratamiento: string;
    notas?: string;
}
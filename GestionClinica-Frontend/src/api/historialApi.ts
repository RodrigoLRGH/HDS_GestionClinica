import api from './axios';
import { type HistoriaClinica } from '../types/HistoriaClinica/HistoriaClinica';
import { type Evolucion } from '../types/Evolucion/Evolucion';

export const obtenerHistorialPorPaciente = (pacienteId: number) => 
    api.get<Evolucion[]>(`/api/historial/paciente/${pacienteId}`);

export const obtenerHistorialPorId = (id: number) => 
    api.get<HistoriaClinica>(`/api/historial/${id}`);
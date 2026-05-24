import api from '../api/axios';
import { type Evolucion } from '../types/Evolucion/Evolucion';
import { type CrearEvolucionDTO } from '../types/Evolucion/CrearEvolucionDTO';

export const obtenerEvoluciones = () => api.get<Evolucion[]>('/api/evoluciones');

export const obtenerEvolucionPorId = (id: number) => api.get<Evolucion>(`/api/evoluciones/${id}`);

export const crearEvolucion = (evolucion: CrearEvolucionDTO) => api.post<Evolucion>('/api/evoluciones', evolucion);

export const eliminarEvolucion = (id: number) => api.delete(`/api/evoluciones/${id}`);
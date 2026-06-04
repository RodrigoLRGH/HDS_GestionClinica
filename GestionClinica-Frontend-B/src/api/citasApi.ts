import api from "./axios";
import { type Cita} from "../types/Cita/Cita";
import { type CrearCitaDTO } from "../types/Cita/CrearCitaDTO";
import { type ActualizarCitaDTO } from "../types/Cita/ActualizarCitaDTO";

export const obtenerCitas = async () => api.get<Cita[]>("api/citas");

export const obtenerCitaPorId = async (id: number) => api.get<Cita>(`api/citas/${id}`);

export const crearCita = async (cita: CrearCitaDTO) => api.post<Cita>("api/citas", cita);

export const actualizarCita = async (id: number, cita: ActualizarCitaDTO) => api.put<Cita>(`api/citas/${id}`, cita);

export const cancelarCita = async (id: number) => api.delete(`api/citas/${id}`);
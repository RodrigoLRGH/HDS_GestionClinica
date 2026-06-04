import api from "./axios";
import { type Paciente } from "../types/Paciente/Paciente";

export const obtenerPacientes = async () => api.get<Paciente[]>("/api/pacientes");

export const obtenerPacientePorId = async (id: number) => api.get<Paciente>(`/api/pacientes/${id}`);
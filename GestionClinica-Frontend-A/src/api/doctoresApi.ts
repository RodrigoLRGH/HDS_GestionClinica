import api from './axios';
import { type Doctor } from '../types/Doctor/Doctor';

export const obtenerDoctores = () => api.get<Doctor[]>('/api/doctores');
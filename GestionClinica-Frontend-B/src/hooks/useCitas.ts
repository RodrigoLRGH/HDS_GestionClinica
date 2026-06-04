import { useState, useEffect } from "react";
import { obtenerCitas, crearCita, actualizarCita, cancelarCita } from "../api/citasApi";
import { type Cita } from "../types/Cita/Cita";
import { type CrearCitaDTO } from "../types/Cita/CrearCitaDTO";
import { type ActualizarCitaDTO } from "../types/Cita/ActualizarCitaDTO";

export const useCitas = () => {
    const [citas, setCitas] = useState<Cita[]>([]);
    const [cargando, setCargando] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const cargarCitas = async () => {
        setCargando(true);
        setError(null);

        try {
            const response = await obtenerCitas();
            setCitas(response.data);
        } catch (error) {
            setError("Error al cargar las citas");
        } finally {
            setCargando(false);
        }
    };

    const crear = async (dto: CrearCitaDTO) => {
        setCargando(true);
        setError(null);
        try {
            const duplicada = citas.find(c =>
                c.idPaciente === dto.idPaciente &&
                c.fechaHora.startsWith(dto.fechaHora)
            );
            if (duplicada) {
                setError('Ya existe una cita para este paciente en esa fecha y hora');
                return false;
            }

            await crearCita(dto);

            await cargarCitas();
            return true;
        } catch (error) {
            setError("Error al crear la cita");
            return false;
        } finally {
            setCargando(false);
        }
    };

    const actualizar = async (id: number, dto: ActualizarCitaDTO) => {
        setCargando(true);
        setError(null);
        try {
            await actualizarCita(id, dto);
            await cargarCitas();
            return true;
        } catch (error) {
            setError("Error al actualizar la cita");
            return false;
        } finally {
            setCargando(false);
        }
    };

    const cancelar = async (id: number) => {
        setCargando(true);
        setError(null);
        try {
            await cancelarCita(id);
            await cargarCitas();
            return true;
        } catch (error) {
            setError("Error al cancelar la cita");
            return false;
        } finally {
            setCargando(false);
        }
    };

    useEffect(() => {
        cargarCitas();
    }, []);

    return { citas, cargando, error, crear, actualizar, cancelar, limpiarError: () => setError(null) };

}
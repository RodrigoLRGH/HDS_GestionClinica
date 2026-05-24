import { useState, useEffect } from "react";
import { obtenerEvoluciones, crearEvolucion, eliminarEvolucion } from "../api/evolucionesApi";
import { type Evolucion } from "../types/Evolucion/Evolucion";
import { type CrearEvolucionDTO } from "../types/Evolucion/CrearEvolucionDTO";
import { obtenerHistorialPorPaciente } from "../api/historialApi";

export const useEvoluciones = () => {
    const [evoluciones, setEvoluciones] = useState<Evolucion[]>([]);
    const [cargando, setCargando] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const cargarEvoluciones = async () => {
        setCargando(true);
        setError(null);
        try {
            const response = await obtenerEvoluciones();
            setEvoluciones(response.data);
        } catch (error) {
            setError("Error al cargar las evoluciones");
        } finally {
            setCargando(false);
        }
    };

    const cargarEvolucionesPorPaciente = async (IdPaciente: number) => {
        setCargando(true);
        setError(null);
        try {
            const response = await obtenerHistorialPorPaciente(IdPaciente);
            setEvoluciones(response.data);
        } catch (error) {
            setError("Error al cargar las evoluciones del paciente");
        } finally {
            setCargando(false);
        }
    };

    const crear = async (dto: CrearEvolucionDTO): Promise<boolean> => {
        setCargando(true);
        setError(null);
        try {
            await crearEvolucion(dto);
            await cargarEvoluciones();
            return true;
        } catch (err) {
            setError("Error al crear la evolución");
            return false;
        } finally {
            setCargando(false);
        }
    };

    const eliminar = async (id: number) => {
        setCargando(true);
        setError(null);
        try {
            await eliminarEvolucion(id);
            await cargarEvoluciones();
            return true;
        } catch (error) {
            setError("Error al eliminar la evolución");
        } finally {
            setCargando(false);
        }
    };

    useEffect(() => {
        cargarEvoluciones();
    }, []);

    return { evoluciones, cargando, error, cargarEvoluciones, cargarEvolucionesPorPaciente, crear, eliminar };
}
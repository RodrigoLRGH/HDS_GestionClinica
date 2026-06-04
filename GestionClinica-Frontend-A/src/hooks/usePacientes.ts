import { useState, useEffect } from "react";
import { obtenerPacientes } from "../api/pacientesApi";
import { type Paciente } from "../types/Paciente/Paciente";

export const usePacientes = () => {
    const [pacientes, setPacientes] = useState<Paciente[]>([]);
    const [cargando, setCargando] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const cargarPacientes = async () => {
        setCargando(true);
        setError(null);
        try {
            const response = await obtenerPacientes();
            setPacientes(response.data.map((p: any) => ({
                ...p,
                nombreCompleto: `${p.nombres} ${p.apellidos}`
            })));
        } catch (error) {
            setError("Error al cargar los pacientes activos");
        } finally {
            setCargando(false);
        }
    };

    useEffect(() => {
        cargarPacientes();
    }, []);

    return { pacientes, cargando, error, cargarPacientes };
}
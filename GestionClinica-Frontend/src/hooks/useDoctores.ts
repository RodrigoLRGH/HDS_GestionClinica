import { useState, useEffect } from 'react';
import { type Doctor } from '../types/Doctor/Doctor';
import { obtenerDoctores } from '../api/doctoresApi';

export const useDoctores = () => {
    const [doctores, setDoctores] = useState<Doctor[]>([]);
    const [cargando, setCargando] = useState(false);

    useEffect(() => {
        setCargando(true);
        obtenerDoctores()
            .then(res => setDoctores(res.data))
            .catch(console.error)
            .finally(() => setCargando(false));
    }, []);

    return { doctores, cargando };
};
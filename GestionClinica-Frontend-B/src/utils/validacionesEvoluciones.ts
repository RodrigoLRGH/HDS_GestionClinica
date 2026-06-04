export const validarEvolucion = (form: {
    idHistoriaClinica: number;
    idDoctor: number;
    fecha: string;
    diagnostico: string;
    tratamiento: string;
}): string | null => {
    if (form.idHistoriaClinica === 0)
        return 'Este paciente no tiene historia clínica activa';

    if (form.idDoctor === 0)
        return 'Seleccione un doctor';

    if (!form.fecha)
        return 'La fecha es obligatoria';

    if (new Date(form.fecha) > new Date())
        return 'La fecha no puede ser futura';

    if (!form.diagnostico.trim())
        return 'El diagnóstico es obligatorio';

    if (!form.tratamiento.trim())
        return 'El tratamiento es obligatorio';

    return null;
};
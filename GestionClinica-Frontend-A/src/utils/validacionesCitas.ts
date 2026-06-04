export const validarCita = (form: {
    idPaciente: number;
    idDoctor: number;
    fechaHora: string;
    motivo: string;
}): string | null => {
    if (form.idPaciente === 0)
        return 'Seleccione un paciente';

    if (form.idDoctor === 0)
        return 'Seleccione un doctor';

    if (!form.fechaHora)
        return 'La fecha y hora son obligatorias';

    if (new Date(form.fechaHora) < new Date())
        return 'La fecha no puede ser pasada';

    if (form.motivo.trim().length < 5)
        return 'El motivo debe tener al menos 5 caracteres';

    return null;
};

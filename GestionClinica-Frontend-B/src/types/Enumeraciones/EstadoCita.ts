export const EstadoCita = {
    Pendiente: 1,
    Confirmada: 2,
    Cancelada: 3,
    Completada: 4,
    NoAsistio: 5
} as const;

export type EstadoCita = number;
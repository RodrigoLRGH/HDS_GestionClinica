export const Genero = {
    Masculino: 1,
    Femenino: 2,
    Otro: 3,
    PrefieroNoDecirlo: 4
} as const;

export type Genero = typeof Genero[keyof typeof Genero];
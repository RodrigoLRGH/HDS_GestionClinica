export const TipoDeDocumento = {
    Cedula: 1,
    Pasaporte: 2,
    Otro: 3
} as const;

export type TipoDeDocumento = typeof TipoDeDocumento[keyof typeof TipoDeDocumento];
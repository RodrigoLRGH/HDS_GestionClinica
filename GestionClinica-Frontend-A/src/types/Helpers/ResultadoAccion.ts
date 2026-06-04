export interface ResultadoAccion<T> {
    exitoso: boolean;
    mensaje?: string;
    datos?: T;
    errores?: string[];
}
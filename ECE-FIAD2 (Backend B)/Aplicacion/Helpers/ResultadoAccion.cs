namespace Aplicacion.Helpers
{
    public class ResultadoAccion<T>
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }
        public List<string> Errores { get; set; } = new();

        public static ResultadoAccion<T> Exito(T datos, string mensaje = "Operación exitosa") =>
            new() { Exitoso = true, Mensaje = mensaje, Datos = datos };

        public static ResultadoAccion<T> Falla(string mensaje, List<string>? errores = null) =>
            new() { Exitoso = false, Mensaje = mensaje, Errores = errores ?? new() };
    }

    public class ResultadoAccion
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<string> Errores { get; set; } = new();

        public static ResultadoAccion Exito(string mensaje = "Operación exitosa") =>
            new() { Exitoso = true, Mensaje = mensaje };

        public static ResultadoAccion Falla(string mensaje, List<string>? errores = null) =>
            new() { Exitoso = false, Mensaje = mensaje, Errores = errores ?? new() };
    }
}
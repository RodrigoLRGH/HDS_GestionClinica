using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Aplicacion.Helpers
{
    public static class EnumHelper
    {

        public static List<(T Value, string Nombre)> ObtenerOpcionesEnum<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(valor => (valor, ObtenerNombreEnum(valor)))
                .ToList();
        }

        public static string ObtenerNombreEnum<T>(T valor) where T : Enum
        {
            var field = valor.GetType().GetField(valor.ToString());
            var display = field?.GetCustomAttribute<DisplayAttribute>();
            return display?.Name ?? valor.ToString();
        }
    }
}
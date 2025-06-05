using QueriesFormation.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.General
{
    public static class DescripcionEncabezado
    {
        public static string EstableceDescripcion(SQLElemento sql)
        {
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine("/*");
            scriptBuilder.AppendLine("Descripción: "+sql.Descripcion);
            scriptBuilder.AppendLine("Módulo: "+sql.NombreModulo);
            scriptBuilder.AppendLine("Nombre: "+sql.NombreScript);
            scriptBuilder.AppendLine("Autor: "+sql.Autor);
            scriptBuilder.AppendLine("Version: "+sql.Version);
            scriptBuilder.AppendLine("Fecha: "+sql.FechaCreacion.ToString("yyyy-MM-dd"));
            scriptBuilder.AppendLine("*/");
            scriptBuilder.AppendLine();
            return scriptBuilder.ToString();
        }
    }
}

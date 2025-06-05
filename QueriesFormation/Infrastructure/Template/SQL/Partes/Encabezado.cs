using QueriesFormation.Dominio;
using QueriesFormation.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.SQL.Partes
{
    public static class Encabezado
    {
        public static string EstableceEncabezado(SQLElemento sql) {
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine("DECLARE @Modulo NVARCHAR(250), @Script NVARCHAR(250);");
            scriptBuilder.AppendLine($"SET @Modulo = '{sql.NombreModulo}';");
            scriptBuilder.AppendLine($"SET @Script = '{sql.NombreScript}';");
            scriptBuilder.AppendLine();
            return scriptBuilder.ToString();
        }

    }
}

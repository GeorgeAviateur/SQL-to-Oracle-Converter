using QueriesFormation.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.Oracle.Partes
{
    public static class EncabezadoOracle
    {
        public static string EstableceEncabezado(SQLElemento sql)
        {
            StringBuilder scriptBuilder = new();

            scriptBuilder.AppendLine("DECLARE");
            scriptBuilder.AppendLine($"l_modulo VARCHAR(250) := '{sql.NombreModulo}';");
            scriptBuilder.AppendLine($"l_script VARCHAR(250) := '{sql.NombreScript}';");
            scriptBuilder.AppendLine($"l_BaseDeDatos VARCHAR2(100);");
            scriptBuilder.AppendLine($"l_servername VARCHAR2(100);");
            scriptBuilder.AppendLine($"l_usuario VARCHAR2(100);");
            scriptBuilder.AppendLine($"l_err_num NUMBER := 0;");
            scriptBuilder.AppendLine($"l_err_msg VARCHAR2(255) := '';");
            scriptBuilder.AppendLine();
            return scriptBuilder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.Oracle.Partes
{
    public static class LogOracle
    {
        public static string InsertaLog()
        {
            StringBuilder cadena = new StringBuilder();



            cadena.AppendLine("WHEN OTHERS THEN ");
            cadena.AppendLine("l_err_num := SQLCODE;");
            cadena.AppendLine("l_err_msg := SQLERRM;");
            cadena.AppendLine("ROLLBACK;");
            cadena.AppendLine("");
            cadena.AppendLine("-- Log the error ");
            cadena.AppendLine("INSERT INTO SCRIPT (BaseDeDatos, Modulo, Script, ErrorNumber, ErrorMessage, SERVERNAME, USUARIO)");
            cadena.AppendLine("VALUES (l_BaseDeDatos, l_modulo, l_script, l_err_num, l_err_msg, l_servername, l_usuario);");
            cadena.AppendLine("");
            cadena.AppendLine("COMMIT;");
            cadena.AppendLine("");
            cadena.AppendLine("-- Raise application error    ");
            cadena.AppendLine("RAISE_APPLICATION_ERROR(-20001, DBMS_UTILITY.FORMAT_ERROR_BACKTRACE);");

            return cadena.ToString();
        }
    }
}

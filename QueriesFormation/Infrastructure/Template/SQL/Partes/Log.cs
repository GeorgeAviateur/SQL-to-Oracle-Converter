using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.SQL.Partes
{
    public static class Log
    {
        public static string InsertaLog() {
            StringBuilder cadena = new StringBuilder();

            cadena.AppendLine("    -- Log errores en la tabla SCRIPT");
            cadena.AppendLine("    INSERT INTO [SCRIPT] ([BaseDeDatos], [Modulo], [Script], SERVERNAME, USUARIO)");
            cadena.AppendLine("    SELECT DB_NAME(), @Modulo, @Script, @@SERVERNAME, SYSTEM_USER;");
            cadena.AppendLine();
            


            return cadena.ToString();
        }
        public static string InsertaLogError() {
            StringBuilder cadena = new StringBuilder();

            cadena.AppendLine("    -- Log errores en la tabla SCRIPT");
            cadena.AppendLine("    INSERT INTO [SCRIPT] ([BaseDeDatos], [Modulo], [Script], [ErrorNumber], [ErrorSeverity], [ErrorState], [ErrorProcedure], [ErrorLine], [ErrorMessage], SERVERNAME, USUARIO)");
            cadena.AppendLine("    SELECT DB_NAME(), @Modulo, @Script, ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), @@SERVERNAME, SYSTEM_USER;");
            cadena.AppendLine();
            cadena.AppendLine("    PRINT 'ERROR LINEA: ' + CONVERT(VARCHAR, ERROR_LINE()) + ' MENSAJE: ' + ERROR_MESSAGE();");
            cadena.AppendLine("    THROW 50001, 'ERROR', 1;");


            return cadena.ToString();
        }
    }
}

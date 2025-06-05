using QueriesFormation.Infrastructure.Template.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.Oracle.Partes
{
    public static class GetDatabase
    {
        public static string GetDatabaseName() {
            StringBuilder scriptBuilder = new();
        
            
            scriptBuilder.AppendLine($"{Utiles.Indentado(2)}SELECT GLOBAL_NAME INTO l_BaseDeDatos FROM GLOBAL_NAME;");
            scriptBuilder.AppendLine($"{Utiles.Indentado(2)}l_servername:= sys_context('userenv', 'server_host');");
            scriptBuilder.AppendLine($"{Utiles.Indentado(2)}SELECT USER INTO l_usuario FROM dual;");
            scriptBuilder.AppendLine();
            return scriptBuilder.ToString();


        }
    }
}

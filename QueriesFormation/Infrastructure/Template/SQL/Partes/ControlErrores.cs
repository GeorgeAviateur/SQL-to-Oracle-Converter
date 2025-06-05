using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.SQL.Partes
{
    public static class ControlErrores
    {
        public static string IniciaControl()
        {
            return "BEGIN TRY";
        }
        public static string FinControl()
        {
            return "END TRY";
        }

        public static string IniciaProceso()
        {
            return "BEGIN";
        }
        public static string FinProceso()
        {
            return "END";
        }

        public static string InicioCatch()
        {
            return "BEGIN CATCH";
        }

        public static string FinCatch()
        {
            return "END CATCH;";
        }

    }
}

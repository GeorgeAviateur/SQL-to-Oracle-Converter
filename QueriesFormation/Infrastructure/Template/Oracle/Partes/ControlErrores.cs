using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.Oracle.Partes
{
    public static class ControlErrores
    {
        public static string IniciaControl()
        {
            return "BEGIN";
        }
        public static string FinControl()
        {
            return "COMMIT;";
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
            return "EXCEPTION";
        }

        public static string FinCatch()
        {
            return "END;";
        }
        public static string FinalComment()
        {
            return @"/*
select * from script order by 1 desc
*/
";
        }
        public static string Rollback()
        {
            return "ROLLBACK;";
        }
    }
}

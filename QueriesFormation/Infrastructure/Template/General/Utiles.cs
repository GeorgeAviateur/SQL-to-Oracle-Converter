using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QueriesFormation.Infrastructure.Template.General
{
    public static class Utiles
    {
        public static string Indentado(int cantidadTabs)
        {
            if (cantidadTabs > 0)
            {
                string tabs = "";
                for (int i = 0; i < cantidadTabs - 1; i++)
                {
                    tabs += "   ";
                }
                return tabs;
            }

            return "";

        }

        public static string ConvertirAValorSQL(string fecha)
        {
            if (fecha == null) { return string.Empty; }
            if (fecha.ToUpper().Contains("DATE"))
            {
                string[] partes = fecha.Split('.');

                if (partes.Length > 1)
                {
                    if (partes[1].ToUpper().Contains( "NOW"))
                    {
                        return fecha.Replace("'DATE.NOW'", "CAST(GETDATE() AS DATE");
                    }
                }

            }
            return fecha;

        }

        public static string ConvertirAValorORACLE(string fecha)
        {
            if (fecha == null) { return string.Empty; }
            if (fecha.ToUpper().Contains("DATE"))
            {
                string[] partes = fecha.Split('.');

                if (partes.Length > 1)
                {
                    if (partes[1].ToUpper().Contains("NOW"))
                    {
                        return fecha.Replace("'DATE.NOW'", "TRUNC(SYSDATE)");
                    }
                }

            }
            return fecha;

        }
        public static string QuitarPuntoYComa(string texto) {

            return texto.Replace(";","");
        }
    }
}

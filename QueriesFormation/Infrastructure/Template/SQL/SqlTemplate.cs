using QueriesFormation.Dominio;
using QueriesFormation.Dominio;
using QueriesFormation.Infrastructure.Template.General;
using QueriesFormation.Infrastructure.Template.SQL.Partes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.SQL
{
    public class SqlTemplate
    {
        public  string GenerateScript(SQLElemento sql)
        {
            StringBuilder scriptBuilder = new StringBuilder();

            scriptBuilder.AppendLine(DescripcionEncabezado.EstableceDescripcion(sql));          
            scriptBuilder.AppendLine(Encabezado.EstableceEncabezado(sql));          
            
            scriptBuilder.AppendLine(ControlErrores.IniciaControl());

            
            
            foreach (QueryElemento query in sql.Query)
            {
                scriptBuilder.AppendLine("");

                string valida = CuerpoQuery.ValidarExistencia(query); //IF...
                if (!string.IsNullOrEmpty(valida))
                {
                    scriptBuilder.AppendLine(valida);
                    scriptBuilder.AppendLine(CuerpoQuery.ComenzarAccion());
                    scriptBuilder.AppendLine(CuerpoQuery.CrearCuerpo(query));
                    scriptBuilder.AppendLine(CuerpoQuery.ResultadoImprime(query));
                    scriptBuilder.AppendLine(CuerpoQuery.FinalizarAccion());

                    scriptBuilder.AppendLine("  ELSE");

                    scriptBuilder.AppendLine(CuerpoQuery.ComenzarAccion());
                    scriptBuilder.AppendLine(CuerpoQuery.SiNoImprime(query));
                    scriptBuilder.AppendLine(CuerpoQuery.FinalizarAccion());

                }
            }
            scriptBuilder.AppendLine(Log.InsertaLog());

            scriptBuilder.AppendLine(ControlErrores.FinControl());

            // Bloque de errores
            scriptBuilder.AppendLine(ControlErrores.InicioCatch());
            scriptBuilder.AppendLine(Log.InsertaLogError());
            scriptBuilder.AppendLine(ControlErrores.FinCatch());
            scriptBuilder.AppendLine();

            return scriptBuilder.ToString();
        }


        public string GenerateScriptName(SQLElemento sql)
        {
            string ruta = "";
            ruta = sql.Consecutivo.ToString();
            ruta += "_";
            ruta += sql.FechaCreacion.ToString("yyyyMMdd");
            ruta += "_";
            ruta += sql.NombreModulo.Replace(" ", "_");
            ruta += "_";
            ruta += sql.NombreScript.Replace(" ", "");
            ruta += "_";
            ruta += sql.Autor;
            ruta += "_SQL";

            return ruta;

        }

    }

}

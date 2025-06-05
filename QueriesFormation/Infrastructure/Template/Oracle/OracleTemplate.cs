using QueriesFormation.Dominio;
using QueriesFormation.Infrastructure.Template.General;
using QueriesFormation.Infrastructure.Template.Oracle.Partes;
using System.Text;

namespace QueriesFormation.Infrastructure.Template.Oracle
{
    public class OracleTemplate
    {
        public string GenerateScript(SQLElemento sql)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine(DescripcionEncabezado.EstableceDescripcion(sql));

            scriptBuilder.AppendLine(EncabezadoOracle.EstableceEncabezado(sql));
            scriptBuilder.AppendLine(Oracle.Partes.ControlErrores.IniciaControl());
            scriptBuilder.AppendLine(GetDatabase.GetDatabaseName());


            int round = 1;
            foreach (QueryElemento query in sql.Query)
            {
                scriptBuilder.AppendLine("");



                string valida = CuerpoQueryOracle.ValidarExistencia(query,round); 
                if (!string.IsNullOrEmpty(valida))
                {
                    scriptBuilder.AppendLine(valida);
                    
                    if(query.Tipoquery != Dominio.Enum.TipoQuery.Actualizacion)
                    scriptBuilder.AppendLine(CuerpoQueryOracle.ComenzarAccion   ());

                    scriptBuilder.Append(CuerpoQueryOracle.CrearCuerpo(query));
                    if (query.Tipoquery != Dominio.Enum.TipoQuery.Actualizacion)
                        scriptBuilder.AppendLine(CuerpoQueryOracle.FinalizarAccion());
                    scriptBuilder.AppendLine(CuerpoQueryOracle.ResultadoImprime(query));
                    scriptBuilder.AppendLine("      ELSE");
                    scriptBuilder.AppendLine(CuerpoQueryOracle.SiNoImprime(query));
                    scriptBuilder.AppendLine("      END IF;");
                    scriptBuilder.AppendLine("   END;"); // END BEGIN
                }
                round++;
            }

            scriptBuilder.AppendLine(Oracle.Partes.ControlErrores.FinControl());
            scriptBuilder.AppendLine(Oracle.Partes.ControlErrores.InicioCatch());
            scriptBuilder.AppendLine(LogOracle.InsertaLog());
            scriptBuilder.AppendLine(Oracle.Partes.ControlErrores.FinCatch());
            scriptBuilder.AppendLine(Oracle.Partes.ControlErrores.FinalComment());

            scriptBuilder.AppendLine();

            return scriptBuilder.ToString();
        }

        public string GenerateScriptName(SQLElemento sql) {
            string ruta = "";
            ruta = sql.Consecutivo.ToString();
            ruta += "_";
            ruta += sql.FechaCreacion.ToString("yyyyMMdd");
            ruta += "_";
            ruta += sql.NombreModulo;
            ruta += "_";
            ruta += sql.NombreScript.Replace(" ","");
            ruta += "_";
            ruta += sql.Autor;
            ruta += "_ORA";

            return ruta;

        }

    }
}

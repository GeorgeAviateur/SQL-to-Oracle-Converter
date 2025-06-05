using QueriesFormation.Dominio.Enum;
using QueriesFormation.Dominio;
using QueriesFormation.Infrastructure.Template.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.Oracle.Partes
{
    public static class CuerpoQueryOracle
    {
        public static string CrearCuerpo(QueryElemento query)
        {
            string scriptBuilder = string.Empty;
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                try { return scriptBuilder = CrearTablaOracle(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al crear la tabla {query.NombreTabla}: " + ex; }
            }

            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                try { return scriptBuilder = InsertarElementosSQL(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Actualizacion)
            {
                try { return scriptBuilder = ActualizarElementosORA(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                try { return scriptBuilder = CrearColumnaSQL(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }

            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionRelacionFK)
            {
                try { return scriptBuilder = CrearForeignOracle(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }

            if (query.Tipoquery == Dominio.Enum.TipoQuery.ModificarColumna)
            {
                try { return scriptBuilder = ModificarColumnaOracle(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionIndice)
            {
                try { return scriptBuilder = CrearIndiceOracle(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }


            return "";

        }
        private static string DevolverTipoVariable(TipoColumna tipoColumna)
        {
            switch (tipoColumna)
            {
                case TipoColumna.EnteroLlave:
                    return ("NUMBER GENERATED AS IDENTITY PRIMARY KEY");
                case TipoColumna.Entero:
                    return ("NUMBER");
                case TipoColumna.Booleano:
                    return ("NUMBER(1)");
                case TipoColumna.Texto:
                    return ("VARCHAR(20)");
                case TipoColumna.Fecha:
                    return ("DATE");
                case TipoColumna.DateTime:
                    return ("TIMESTAMP(6)");

            }


            return "";
        }

        private static string ActualizarElementosORA(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();


            scriptBuilder.Append($"{Utiles.Indentado(4)}UPDATE {query.NombreTabla.ToUpper()} SET ");


            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(1)}{query.Columnas[i].NombreColumna.ToUpper()} = {query.Columnas[i].NuevoValor}");
                if (i < query.Columnas.Length - 1)
                {
                    scriptBuilder.Append(",    ");
                }


            }

            scriptBuilder.AppendLine("");
            scriptBuilder.Append($"{Utiles.Indentado(4)}WHERE ");
            string columnasValidar = "";
            for (int i = 0; i < query.Columnas.Length; i++)
            {

                if (string.IsNullOrEmpty(query.Columnas[i].Valor))
                    columnasValidar += query.Filtro;
                else
                {
                    columnasValidar += $"UPPER({query.Columnas[i].NombreColumna.ToUpper()}) = {query.Columnas[i].Valor}";


                }
                if (i < query.Columnas.Length - 2)
                {
                    columnasValidar += " AND ";
                }
                   
            }
            scriptBuilder.AppendLine($"{Utiles.ConvertirAValorORACLE(columnasValidar)}");


            if (!string.IsNullOrEmpty(query.QueryAdicional))
            {
                scriptBuilder.AppendLine($"{Utiles.Indentado(5)}{query.QueryAdicional}");

            }


            return scriptBuilder.ToString();
        }
        private static string InsertarElementosSQL(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();


            scriptBuilder.Append($"{Utiles.Indentado(6)}INSERT INTO {query.NombreTabla.ToUpper()} (");


            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(1)}{query.Columnas[i].NombreColumna.ToUpper()}");
                if (i < query.Columnas.Length - 1)
                {
                    scriptBuilder.Append(",    ");
                }
                else
                {
                    scriptBuilder.AppendLine(")");
                }

            }


            if (!string.IsNullOrEmpty(query.QueryAdicional))
            {
                scriptBuilder.AppendLine($"{Utiles.Indentado(6)}{query.QueryAdicional};");

            }
            else
            {
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}VALUES (");
                for (int i = 0; i < query.Columnas.Length; i++)
                {

                    if (query.Columnas[i].Tipo == TipoColumna.Caracter || query.Columnas[i].Tipo == TipoColumna.Fecha || query.Columnas[i].Tipo == TipoColumna.Texto)
                    {
                        scriptBuilder.Append($"{Utiles.Indentado(5)}''{query.Columnas[i].NuevoValor.Replace("'","")}''");
                    }
                    else
                    {
                        scriptBuilder.Append($"{Utiles.Indentado(5)}{query.Columnas[i].NuevoValor.Replace("'", "")}");
                    }
                    
                    if (i < query.Columnas.Length - 1)
                    {
                        scriptBuilder.AppendLine(",");
                    }
                }

            }
            scriptBuilder.Append(")");
            return scriptBuilder.ToString();
        }
        private static string CrearTablaOracle(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"{Utiles.Indentado(6)}CREATE TABLE {query.NombreTabla} (     ");
            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(6)}{query.Columnas[i].NombreColumna.ToUpper()} ");
                scriptBuilder.Append(DevolverTipoVariable(query.Columnas[i].Tipo)+" ");

                if (query.Columnas[i].PermiteNulo != null && query.Columnas[i].Tipo != TipoColumna.EnteroLlave)
                {
                    if (query.Columnas[i].PermiteNulo == true)
                    {
                        //scriptBuilder.Append("NULL ");
                    }
                    else
                    {
                        scriptBuilder.Append("NOT NULL ");
                    }

                }

                if (!string.IsNullOrEmpty(query.Columnas[i].ValorXDefecto) && query.Columnas[i].ValorXDefecto!="''" && query.Columnas[i].Tipo != TipoColumna.EnteroLlave)
                {
                    if (query.Columnas[i].Tipo != TipoColumna.Fecha && query.Columnas[i].Tipo != TipoColumna.DateTime)
                    { scriptBuilder.Append($"DEFAULT {query.Columnas[i].ValorXDefecto} "); }
                    else { scriptBuilder.Append($"DEFAULT SYSTIMESTAMP "); }
                }
                
                if (i < query.Columnas.Length - 1)
                {
                    scriptBuilder.Append(",");
                    scriptBuilder.AppendLine("");
                }

            }
            scriptBuilder.AppendLine(" ) ");
            return scriptBuilder.ToString();

        }



        private static string CrearForeignOracle(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"{Utiles.Indentado(6)}{query.QueryCompleto.ToUpper().Replace(";","")}");

           
            return scriptBuilder.ToString();

        }
        private static string ModificarColumnaOracle(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            string queryConvertido = $"ALTER TABLE {query.NombreTabla.ToUpper()} MODIFY ({query.Columnas[0]?.NombreColumna.ToUpper()} {DevolverTipoVariable((TipoColumna) query.Columnas[0]?.Tipo)})";
            scriptBuilder.AppendLine($"{Utiles.Indentado(6)}{queryConvertido}");

           
            return scriptBuilder.ToString();

        }
         private static string CrearIndiceOracle(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            string queryConvertido = $"CREATE INDEX {query.Valor.ToUpper()} ON {query.NombreTabla.ToUpper()}(";
            int pos = 0;
            foreach (SQLColumnas columna in query.Columnas) {
                if (pos > 0)
                {
                    queryConvertido += ",";
                }
                queryConvertido += $"{columna.NombreColumna.ToUpper()}";
            
            }

            queryConvertido += ")";


            scriptBuilder.AppendLine($"{Utiles.Indentado(6)}{queryConvertido}");

           
            return scriptBuilder.ToString();

        }
        private static string CrearColumnaSQL(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"{Utiles.Indentado(3)}ALTER TABLE {query.NombreTabla.ToUpper()} ");


            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(5)}ADD {query.Columnas[i].NombreColumna.ToUpper()} ");
                scriptBuilder.Append(DevolverTipoVariable(query.Columnas[i].Tipo)+" ");

                if (query.Columnas[i].PermiteNulo != null)
                {
                    if (query.Columnas[i].PermiteNulo == true)
                    {
                        scriptBuilder.Append(" ");
                    }
                    else
                    {
                        scriptBuilder.Append("NOT NULL ");
                    }
                }
                if (!string.IsNullOrEmpty(query.Columnas[i].ValorXDefecto))
                {
                    scriptBuilder.Append($"DEFAULT {query.Columnas[i].ValorXDefecto} ");
                }
                if (i < query.Columnas.Length - 1)
                {
                    scriptBuilder.Append(",");
                    scriptBuilder.AppendLine("");
                }

            }
            
            return scriptBuilder.ToString();

        }

        public static string ValidarExistencia(QueryElemento query, int round)
        {


            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {

                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}v_tableExists_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}BEGIN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}SELECT COUNT(*) INTO v_tableExists_{round} FROM user_tables WHERE table_name = '{query.NombreTabla}';");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_tableExists_{round} = 0 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('Creando tabla {query.NombreTabla}...');");

                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE v_tableEmpty_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}BEGIN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}SELECT COUNT(*) INTO v_tableEmpty_{round} FROM {query.NombreTabla.ToUpper()} WHERE");

                string columnasValidar = "";
                for (int i = 0; i < query.Columnas.Length; i++)
                {
                    if (string.IsNullOrEmpty(query.Columnas[i].NuevoValor))
                        columnasValidar += query.Filtro;
                    else
                    {
                        if (query.Columnas[i].Tipo == TipoColumna.Caracter || query.Columnas[i].Tipo == TipoColumna.Texto)
                        {
                            columnasValidar += $" UPPER({query.Columnas[i].NombreColumna.ToUpper()}) = {query.Columnas[i].NuevoValor}";
                        }
                        else
                        {
                            columnasValidar += $" {query.Columnas[i].NombreColumna.ToUpper()} = {query.Columnas[i].NuevoValor}";
                        }

                    }
                    if (i < query.Columnas.Length - 1)
                    {
                        columnasValidar += " AND ";
                    }
                }

                scriptBuilder.AppendLine($"{Utiles.QuitarPuntoYComa(Utiles.ConvertirAValorORACLE(columnasValidar))};");


                scriptBuilder.AppendLine("");

                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_tableEmpty_{round} = 0 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('La tabla {query.NombreTabla.ToUpper()} está vacía. Insertando Registros...');");

                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Actualizacion)
            {
                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE v_valueExist_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}BEGIN");

                scriptBuilder.Append($"{Utiles.Indentado(3)}SELECT COUNT(1) INTO v_valueExist_{round} FROM {query.NombreTabla.ToUpper()} WHERE ");


                string columnasValidar = "";
                for (int i = 0; i < query.Columnas.Length; i++)
                {
                    if (string.IsNullOrEmpty(query.Columnas[i].Valor))
                        columnasValidar += query.Filtro;
                    else
                    {
                        columnasValidar += $"UPPER({query.Columnas[i].NombreColumna.ToUpper()}) = {query.Columnas[i].Valor}";


                    }
                    if (i < query.Columnas.Length - 2)
                    {
                        columnasValidar += " AND ";
                    }
                }

                scriptBuilder.AppendLine($"{Utiles.QuitarPuntoYComa(Utiles.ConvertirAValorORACLE(columnasValidar))}");



                scriptBuilder.Append(";");
                scriptBuilder.AppendLine("");

                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_valueExist_{round} > 0 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('Valor antiguo encontrado en {query.NombreTabla.ToUpper()}. Actualizando Registros...');");



                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}v_columnExist_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}BEGIN");

                scriptBuilder.Append($"{Utiles.Indentado(3)}SELECT COUNT(1) INTO v_columnExist_{round} FROM USER_TAB_COLUMNS");
                scriptBuilder.Append($" WHERE TABLE_NAME= '{query.NombreTabla.ToUpper()}' AND COLUMN_NAME = '{query.Columnas[0].NombreColumna.ToUpper()}';");

                scriptBuilder.AppendLine("");

                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_columnExist_{round} = 0 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('Creando Columna...');");



                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionRelacionFK)
            {
                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}v_constraintExists_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}BEGIN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}SELECT COUNT(*) INTO v_constraintExists_{round} FROM user_constraints WHERE table_name = '{query.NombreTabla.ToUpper()}' AND  constraint_name = '{query.Columnas[0]?.NombreColumna.ToUpper()}';");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_constraintExists_{round} = 0 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('Creando Llave {query.NombreTabla}...');");

               


                return scriptBuilder.ToString();

            }

            if (query.Tipoquery == Dominio.Enum.TipoQuery.ModificarColumna)
            {
                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}v_columnExists_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}BEGIN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}SELECT COUNT(*) INTO v_columnExists_{round} FROM USER_TAB_COLUMNS WHERE TABLE_NAME  = '{query.NombreTabla.ToUpper()}' AND  COLUMN_NAME  = '{query.Columnas[0]?.NombreColumna.ToUpper()}' AND DATA_TYPE != '{DevolverTipoVariable((TipoColumna) query.Columnas[0]?.Tipo)}' ;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_columnExists_{round} = 1 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('Modificando Columna {query.NombreTabla}...');");

               


                return scriptBuilder.ToString();

            }

            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionIndice)
            {
                StringBuilder scriptBuilder = new StringBuilder();


                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}DECLARE");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}v_indexExists_{round} NUMBER := 0;");
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}BEGIN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}SELECT COUNT(*) INTO v_indexExists_{round} FROM USER_IND_COLUMNS WHERE TABLE_NAME  = '{query.NombreTabla.ToUpper()}' AND COLUMN_NAME = '{query.Columnas[0]?.NombreColumna.ToUpper()}';");
                scriptBuilder.AppendLine($"{Utiles.Indentado(3)}IF v_indexExists_{round} = 0 THEN");
                scriptBuilder.AppendLine($"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('Creando Indice {query.NombreTabla}...');");

               


                return scriptBuilder.ToString();

            }



            if (!string.IsNullOrEmpty(query.ValidacionCustom))
            {
                return $"{Utiles.Indentado(2)}{query.ValidacionCustom}";

            }
            //TODO: Update
            return "";
        }

        public static string ComenzarAccion()
        {
            return $"{Utiles.Indentado(5)}EXECUTE IMMEDIATE '";

        }
        public static string FinalizarAccion()
        {
            return $"{Utiles.Indentado(6)}';";

        }
        public static string SiNoImprime(QueryElemento query)
        {
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('LA TABLA {query.NombreTabla} YA EXISTE.');";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('EL VALOR A INGRESAR EN {query.NombreTabla} YA EXISTE.');";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Actualizacion)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('EL VALOR A INGRESAR EN {query.NombreTabla} YA ESTÁ MODIFICADO.');";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('LA COLUMNA A CREAR EN {query.NombreTabla} YA EXISTE.');";
            }

            return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');";
        }
        public static string ResultadoImprime(QueryElemento query)
        {
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('LA TABLA {query.NombreTabla} FUE CREADA.');";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('EL VALOR EN LA TABLA {query.NombreTabla} FUE INSERTADO.');";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('LA COLUMNA EN LA TABLA {query.NombreTabla} FUE CREADA.');";
            }
            return $"{Utiles.Indentado(4)}DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');";
        }
    }
}

using QueriesFormation.Dominio;
using QueriesFormation.Dominio.Enum;
using QueriesFormation.Infrastructure.Template.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;


namespace QueriesFormation.Infrastructure.Template.SQL.Partes
{
    public static class CuerpoQuery
    {
        public static string CrearCuerpo(QueryElemento query)
        {
            string scriptBuilder = string.Empty;


            /*
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                try { return scriptBuilder = CrearTablaSQL(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al crear la tabla {query.NombreTabla}: " + ex; }
            }

            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                try { return scriptBuilder = InsertarElementosSQL(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Actualizacion)
            {
                try { return scriptBuilder = ActualizarElementosSQL(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                try { return scriptBuilder = CrearColumnaSQL(query); }
                catch (Exception ex) { return scriptBuilder = $"Error al insertar en la tabla {query.NombreTabla}: " + ex; }

            }
            */

            return InsertarQuery(query);
            

        }

        private static string DevolverTipoVariable(TipoColumna tipoColumna) {
            StringBuilder scriptBuilder = new StringBuilder();
            switch (tipoColumna)
            {
                case TipoColumna.EnteroLlave:
                    return "INT IDENTITY(1,1) PRIMARY KEY ";
                case TipoColumna.Entero:
                    return "INT ";
                case TipoColumna.Booleano:
                    return "BIT ";
                case TipoColumna.Texto:
                    return "VARCHAR(20) ";
                    //TODO:  TEXT, DATE

            }


            return "";
        }


        private static string ActualizarElementosSQL(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();


            scriptBuilder.Append($"{Utiles.Indentado(4)}UPDATE {query.NombreTabla} SET ");


            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(1)}{query.Columnas[i].NombreColumna} = {query.Columnas[i].NuevoValor}");
                if (i < query.Columnas.Length - 1)
                {
                    scriptBuilder.Append(",    ");
                }
                

            }

            scriptBuilder.AppendLine("");
            scriptBuilder.Append ($"{Utiles.Indentado(4)}WHERE ");

            string columnasValidar = "";
            for (int i = 0; i < query.Columnas.Length - 1; i++)
            {
                columnasValidar+=$"{query.Columnas[i].NombreColumna} = {query.Columnas[i].Valor}";
                if (i < query.Columnas.Length - 2)
                {
                    columnasValidar+= " AND ";
                }
            }
            if (string.IsNullOrEmpty(columnasValidar))
            {
                columnasValidar = query.Filtro;
            }

            scriptBuilder.AppendLine(columnasValidar);

            if (!string.IsNullOrEmpty(query.QueryAdicional))
            {
                scriptBuilder.AppendLine($"{Utiles.Indentado(5)}{query.QueryAdicional};");

            }
            
            
            return scriptBuilder.ToString();
        }
        private static string InsertarElementosSQL(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();


            scriptBuilder.Append($"{Utiles.Indentado(3)}INSERT INTO {query.NombreTabla} (");


            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(1)}{query.Columnas[i].NombreColumna}");
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
                scriptBuilder.AppendLine($"{Utiles.Indentado(5)}{query.QueryAdicional};");

            }
            else
            {
                scriptBuilder.Append($"{Utiles.Indentado(4)}VALUES (");
                for (int i = 0; i < query.Columnas.Length; i++)
                {
                    

                    scriptBuilder.Append($"{Utiles.Indentado(5)}{query.Columnas[i].NuevoValor}     ");
                    if (i < query.Columnas.Length - 1)
                    {
                        scriptBuilder.AppendLine(",    ");
                    }
                }
                scriptBuilder.Append($"{Utiles.Indentado(4)});");
                
            }
            
            return scriptBuilder.ToString();
        }
        private static string CrearColumnaSQL(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"{Utiles.Indentado(3)}ALTER TABLE {query.NombreTabla} ");


            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(5)}ADD {query.Columnas[i].NombreColumna} ");
                scriptBuilder.Append(DevolverTipoVariable(query.Columnas[i].Tipo));
                
                if (query.Columnas[i].PermiteNulo != null)
                {
                    if (query.Columnas[i].PermiteNulo == true)
                    {
                        scriptBuilder.Append("NULL ");
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
            scriptBuilder.AppendLine(";");

            return scriptBuilder.ToString();

        }
        private static string InsertarQuery(QueryElemento query) {

            StringBuilder scriptBuilder = new StringBuilder();

            string[] lines = query.QueryCompleto.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                scriptBuilder.AppendLine(Utiles.Indentado(3) + Utiles.ConvertirAValorSQL( line));
            }
            return scriptBuilder.ToString();
        }
        private static string CrearTablaSQL(QueryElemento query)
        {
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine($"{Utiles.Indentado(3)}CREATE TABLE {query.NombreTabla} (     ");
            for (int i = 0; i < query.Columnas.Length; i++)
            {
                scriptBuilder.Append($"{Utiles.Indentado(5)}{query.Columnas[i].NombreColumna} ");
                scriptBuilder.Append(DevolverTipoVariable(query.Columnas[i].Tipo));
                if (query.Columnas[i].PermiteNulo != null)
                {
                    if (query.Columnas[i].PermiteNulo == true)
                    {
                        scriptBuilder.Append("NULL ");
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
            scriptBuilder.AppendLine(" ); ");
            return scriptBuilder.ToString();

        }
        public static string ValidarExistencia(QueryElemento query)
        {

            if (!string.IsNullOrEmpty(query.ValidacionCustom))
            {
                return $"{Utiles.Indentado(2)}{query.ValidacionCustom}";

            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                return $"{Utiles.Indentado(2)}IF OBJECT_ID('dbo.{query.NombreTabla}', 'U') IS NULL";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionVista)
            {
                return $"{Utiles.Indentado(2)}IF OBJECT_ID('dbo.{query.NombreTabla}', 'U') IS NULL";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                StringBuilder scriptBuilder = new StringBuilder();
                string columnasValidar = "";


                for (int i = 0; i < query.Columnas.Length; i++)
                {
                    
                    

                    if (query.Columnas[i].Tipo == TipoColumna.Caracter|| query.Columnas[i].Tipo == TipoColumna.Fecha|| query.Columnas[i].Tipo == TipoColumna.Texto) {
                        columnasValidar += $"UPPER ({query.Columnas[i].NombreColumna} ) = ";
                        if (query.Columnas[i].NuevoValor != null)
                        {
                            columnasValidar += $"'{query.Columnas[i].NuevoValor.Replace("'", "").ToUpper()}'";
                        }
                        else
                        {
                            columnasValidar += $"'_____'";
                        }
                    }
                    else
                    {
                        columnasValidar += $"{query.Columnas[i].NombreColumna}  = ";
                        columnasValidar += $"{query.Columnas[i].NuevoValor}";
                    }

                    if (i < query.Columnas.Length - 1)
                    {
                        columnasValidar+=" AND ";
                    }
                }
                scriptBuilder.AppendLine(
                    $"{Utiles.Indentado(2)}IF NOT EXISTS (SELECT * FROM {query.NombreTabla} WHERE {Utiles.QuitarPuntoYComa(Utiles.ConvertirAValorSQL(columnasValidar))})"
                    );

                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Actualizacion) {
                StringBuilder scriptBuilder = new StringBuilder();
                string columnasValidar = "";
                for (int i = 0; i < query.Columnas.Length - 1; i++)
                {
                    columnasValidar += $"UPPER({query.Columnas[i].NombreColumna}) = {query.Columnas[i].Valor}";
                    if (i < query.Columnas.Length - 2)
                    {
                        columnasValidar += " AND ";
                    }
                }
                if (string.IsNullOrEmpty(columnasValidar))
                    columnasValidar = query.Filtro;

                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}IF EXISTS (SELECT * FROM {query.NombreTabla} WHERE {Utiles.QuitarPuntoYComa(Utiles.ConvertirAValorSQL(columnasValidar))})");


                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna) {
                StringBuilder scriptBuilder = new StringBuilder();




                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{query.NombreTabla}' AND COLUMN_NAME = '{query.Columnas[0].NombreColumna}')");


                return scriptBuilder.ToString();
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionRelacionFK) {

                return $"{Utiles.Indentado(2)}IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='{query.Columnas[0]?.NombreColumna}')";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.ModificarColumna) {

                return $"{Utiles.Indentado(2)}IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{query.NombreTabla}' AND COLUMN_NAME = '{query.Columnas[0]?.NombreColumna}' AND DATA_TYPE <> '{query.Columnas[0]?.Tipo}')";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionIndice) {
                StringBuilder scriptBuilder = new StringBuilder();

                
                scriptBuilder.AppendLine($"{Utiles.Indentado(2)}IF NOT EXISTS (");
                scriptBuilder.AppendLine(
                    @" SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id");
                scriptBuilder.AppendLine($" WHERE t.name = '{query.NombreTabla}' AND c.name = '{query.Columnas[0]?.NombreColumna}')");
                return scriptBuilder.ToString();
            }

                
                return "";
        }

        public static string ComenzarAccion()
        {
            return $"{Utiles.Indentado(3)}BEGIN";

        }
        public static string FinalizarAccion()
        {
            return $"{Utiles.Indentado(3)}END";

        }
        public static string SiNoImprime(QueryElemento query)
        {
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                return $"        PRINT 'LA TABLA {query.NombreTabla} YA EXISTE.'";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                return $"        PRINT 'EL VALOR A INGRESAR EN {query.NombreTabla} YA EXISTE.'";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Actualizacion)
            {
                return $"        PRINT 'EL VALOR EN {query.NombreTabla} YA ESTÁ ACTUALIZADO.'";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                return $"        PRINT 'LA COLUMNA EN {query.NombreTabla} YA ESTÁ CREADA.'";
            }

            return $"        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'";
        }
        public static string ResultadoImprime(QueryElemento query)
        {
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionTabla)
            {
                return $"{Utiles.Indentado(4)}PRINT 'LA TABLA {query.NombreTabla} FUE CREADA.'";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.Insercion)
            {
                return $"{Utiles.Indentado(4)}PRINT 'EL VALOR EN LA TABLA {query.NombreTabla} FUE INSERTADO.'";
            }
            if (query.Tipoquery == Dominio.Enum.TipoQuery.CreacionColumna)
            {
                return $"{Utiles.Indentado(4)}PRINT 'LA COLUMNA EN LA TABLA {query.NombreTabla} FUE INSERTADA.'";
            }

            return $"{Utiles.Indentado(4)}PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA {query.NombreTabla} CON ÉXITO.'"; ;
        }

    }
}

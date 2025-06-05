using QueriesFormation.Dominio;
using QueriesFormation.Dominio.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QueriesFormation.Infrastructure.Template.General
{
    public static class ProcesarQuery
    {
        public static List<QueryElemento> ProcessQuery(string sqlQuery)
        {
            SQLElemento elemento = new SQLElemento();

            // Process CREATE TABLE
            if (sqlQuery.StartsWith("CREATE TABLE", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessCreateTable(sqlQuery);
            }

            // Process INSERT INTO
            if (sqlQuery.StartsWith("INSERT INTO", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessInsert(sqlQuery);
            }

            // Process UPDATE
            if (sqlQuery.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessUpdate(sqlQuery);
            }

            // Process DELETE
            if (sqlQuery.StartsWith("DELETE FROM", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessDelete(sqlQuery);
            }
            // PROCESS CREATE COLUMNS
            if (sqlQuery.StartsWith("ALTER TABLE", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessAlterTable(sqlQuery);
            }
            // Process CREATE VIEW
            if (sqlQuery.StartsWith("CREATE VIEW", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessCreateView(sqlQuery);
            }
            if (sqlQuery.StartsWith("CREATE INDEX", StringComparison.OrdinalIgnoreCase))
            {
                return ProcessCreateIndex(sqlQuery);
            }

            return null;
        }

        private static List<QueryElemento> ProcessCreateIndex(string sqlQuery)
        {
            // Regex to match: CREATE INDEX index_name ON table_name (col1, col2, ...)
            var match = Regex.Match(sqlQuery,
                @"CREATE\s+INDEX\s+(\w+)\s+ON\s+(\w+)\s*\(([^)]+)\)",
                RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                throw new Exception("Invalid CREATE INDEX syntax.");
            }

            string indexName = match.Groups[1].Value;
            string tableName = match.Groups[2].Value;
            string columnList = match.Groups[3].Value;

            var columnNames = columnList
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToList();

            // Build SQLColumnas array
            SQLColumnas[] columns = columnNames.Select(col => new SQLColumnas
            {
                NombreColumna = col,
                Tipo = TipoColumna.Indice, // You may adjust if you have something more accurate
                PermiteNulo = true, // Nullability doesn't change with indexes
                ValorXDefecto = null
            }).ToArray();

            return new List<QueryElemento>
    {
        new QueryElemento
        {
            Tipoquery = TipoQuery.CreacionIndice, // Make sure this enum exists
            NombreTabla = tableName,
            Columnas = columns,
            QueryCompleto = sqlQuery,
            Valor = indexName,
        }
    };
        }



        private static List<QueryElemento> ProcessCreateTable(string sqlQuery)
        {
            // Extraer nombre de tabla
            var tableMatch = Regex.Match(sqlQuery, @"CREATE TABLE\s+(\w+)", RegexOptions.IgnoreCase);
            string tableName = tableMatch.Groups[1].Value;

            // Extraer sección de definición de columnas
            var columnDefinitionSection = sqlQuery
                .Replace(tableMatch.Groups[0].Value, "") // Eliminar el CREATE TABLE ...
                .Trim(' ', '(', ')', ';'); // Limpiar paréntesis y punto y coma

            // Separar cada columna respetando paréntesis internos
            var columnLines = SplitSqlColumns(columnDefinitionSection);
            var columns = new List<SQLColumnas>();

            foreach (var line in columnLines)
            {
                // Ignorar constraints externas (FOREIGN, CONSTRAINT, etc.)
                if (Regex.IsMatch(line, @"^\s*(CONSTRAINT|FOREIGN|REFERENCES)\b", RegexOptions.IgnoreCase))
                    continue;

                // Parsear la columna (nombre, tipo, atributos)
                var match = Regex.Match(line, @"^(\w+)\s+([^\s]+(?:\s*\([^)]+\))?)\s*(.*)$");
                if (!match.Success) continue;

                string columnName = match.Groups[1].Value;
                string columnType = TipoDato(match.Groups[2].Value);
                string attributes = match.Groups[3].Value;

                TipoColumna tipoColumna = MapColumnType(columnType);
                bool isPrimaryKey = attributes.Contains("PRIMARY KEY", StringComparison.OrdinalIgnoreCase);
                bool isIdentity = attributes.Contains("IDENTITY", StringComparison.OrdinalIgnoreCase);
                bool isNullable = attributes.Contains("NOT NULL", StringComparison.OrdinalIgnoreCase);
                string defaultValue = null;

                // Extraer valor por defecto
                var defaultMatch = Regex.Match(attributes, @"DEFAULT\s+([\w\d']+)", RegexOptions.IgnoreCase);
                if (defaultMatch.Success)
                {
                    defaultValue = defaultMatch.Groups[1].Value;
                }

                // Si es PK, no puede ser nulo
                if (isPrimaryKey)
                {
                    tipoColumna = TipoColumna.EnteroLlave;
                    isNullable = false;
                }

                columns.Add(new SQLColumnas
                {
                    NombreColumna = columnName,
                    Tipo = tipoColumna,
                    PermiteNulo = isNullable,
                    ValorXDefecto = defaultValue
                });
            }

            return new List<QueryElemento>
    {
        new QueryElemento
        {
            Tipoquery = TipoQuery.CreacionTabla,
            NombreTabla = tableName,
            Columnas = columns.ToArray(),
            QueryCompleto = sqlQuery,
        }
    };
        }

        public static string TipoDato(string input)
        {
            if (input.ToUpper().Contains("VARCHAR"))
            {
                return "VARCHAR";
            }
            return input;

        }

        public static List<string> SplitSqlColumns(string input)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            int parenDepth = 0;

            foreach (var c in input)
            {
                if (c == '(') parenDepth++;
                else if (c == ')') parenDepth--;

                if (c == ',' && parenDepth == 0)
                {
                    result.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
                result.Add(sb.ToString().Trim());

            return result;
        }



        private static List<QueryElemento> ProcessInsert(string sqlQuery)
        {
            // Extract table name
            var tableMatch = Regex.Match(sqlQuery, @"INSERT INTO\s+(\w+)", RegexOptions.IgnoreCase);
            string tableName = tableMatch.Success ? tableMatch.Groups[1].Value : null;

            // Extract column names
            var columnMatch = Regex.Match(sqlQuery, @"INSERT INTO\s+\w+\s*\((.*?)\)", RegexOptions.IgnoreCase);
            string[] columns = columnMatch.Success ? columnMatch.Groups[1].Value.Split(',') : Array.Empty<string>();



            var sqlColumns = new List<SQLColumnas>();
            foreach (string column in columns)
            {
                if (!string.IsNullOrEmpty(column.Trim()))
                {
                    var tipoCol = TipoColumna.Caracter;
                    if (column.Trim().StartsWith("Id") || column.Trim().StartsWith("Cod"))
                    {
                        tipoCol = TipoColumna.Entero;
                    }
                    sqlColumns.Add(new SQLColumnas
                    {
                        NombreColumna = column.Trim(),
                        Tipo = tipoCol,
                    });
                }
            }

            //revisa si hay select


            var selectMatch = Regex.Match(sqlQuery, @"SELECT\s+.+\s+FROM\s+.+", RegexOptions.IgnoreCase);
            string queryAdicional = selectMatch.Success ? selectMatch.Value.Trim() : null;

            if (selectMatch.Success)
            {

                return new List<QueryElemento>()
            {
                new  QueryElemento() {
                Tipoquery = TipoQuery.Insercion,
                NombreTabla = tableName,
                Columnas = sqlColumns.ToArray(),
                QueryAdicional = queryAdicional,
                QueryCompleto = sqlQuery,
                }
            };
            }



            string columnNames = columnMatch.Success ? columnMatch.Groups[1].Value.Trim() : "";

            var valuesMatch = Regex.Match(sqlQuery, @"VALUES\s*(.*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            List<QueryElemento> resultado = new List<QueryElemento>();
            if (valuesMatch.Success)
            {
                string valuesSection = valuesMatch.Groups[1].Value.Trim();

                var valueSets = Regex.Split(valuesSection, @"\),", RegexOptions.IgnoreCase);
                foreach (var valueSet in valueSets)
                {
                    string cleanValues = valueSet.Trim();
                    if (!cleanValues.EndsWith(")"))
                        cleanValues += ")";

                    // Quita los paréntesis 
                    cleanValues = cleanValues.Trim('(', ')');

                    // Separa los valores
                    string[] values = Regex.Split(cleanValues, @",(?![^']*'[^']*$)");

                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        string valorNuevo = values[i].Replace(";", "").Replace(")", "");
                        TipoColumna tipo = DetectarTipoColumna(valorNuevo);

                        if (values[i].StartsWith("'") && values[i].EndsWith("'"))
                        {
                            values[i] = values[i].Trim('\'');
                        }

                        sqlColumns[i].Tipo = tipo;
                        sqlColumns[i].NuevoValor = valorNuevo;
                    }




                    QueryElemento elementoDB = new QueryElemento()
                    {
                        Tipoquery = TipoQuery.Insercion,
                        NombreTabla = tableName,
                        Columnas = sqlColumns.ToArray(),
                        QueryCompleto = sqlQuery,
                    };
                    resultado.Add(elementoDB);
                }
            }





            return resultado;


        }



        private static TipoColumna DetectarTipoColumna(string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                string trimmedValue = value.Trim('\'');
                return trimmedValue.Length == 1 ? TipoColumna.Caracter : TipoColumna.Texto;
            }

            if (int.TryParse(value, out _))
            {
                return TipoColumna.Entero;
            }

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return TipoColumna.Decimal;
            }

            if (value.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                value.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return TipoColumna.Booleano;
            }

            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return TipoColumna.Fecha;
            }

            if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return TipoColumna.Timestamp;
            }

            return TipoColumna.Desconocido;
        }

        private static List<QueryElemento> ProcessUpdate(string sqlQuery)
        {
            // Extract table name
            var tableMatch = Regex.Match(sqlQuery, @"UPDATE\s+(\w+)", RegexOptions.IgnoreCase);
            string tableName = tableMatch.Success ? tableMatch.Groups[1].Value : null;

            // Extract SET clause
            var setMatch = Regex.Match(sqlQuery, @"SET\s+(.*?)\s+WHERE", RegexOptions.IgnoreCase);
            string[] setClauses = setMatch.Success ? setMatch.Groups[1].Value.Split(',') : Array.Empty<string>();

            // Extract WHERE clause
            var whereMatch = Regex.Match(sqlQuery, @"WHERE\s+(.*)", RegexOptions.IgnoreCase);
            string whereClause = whereMatch.Success ? whereMatch.Groups[1].Value : null;

            // Parse columns in SET clause
            var sqlColumns = new List<SQLColumnas>();
            foreach (string setClause in setClauses)
            {
                var parts = setClause.Split('=');
                if (parts.Length == 2)
                {
                    sqlColumns.Add(new SQLColumnas
                    {
                        NombreColumna = parts[0].Trim(),
                        NuevoValor = parts[1].Trim(),
                    });
                }
            }

            // Extract conditions from WHERE clause
            var whereConditions = new List<(string Column, string Value)>();
            if (!string.IsNullOrEmpty(whereClause))
            {
                // Adjust regex to include the full value with quotes
                var conditionMatches = Regex.Matches(whereClause, @"(\w+)\s*=\s*('[^']+'|\d+)", RegexOptions.IgnoreCase);
                foreach (Match condition in conditionMatches)
                {
                    whereConditions.Add((Column: condition.Groups[1].Value.Trim(), Value: condition.Groups[2].Value.Trim()));
                }
            }

            // Map old values (from WHERE clause) to columns
            foreach (var condition in whereConditions)
            {
                var column = sqlColumns.FirstOrDefault(c => string.Equals(c.NombreColumna, condition.Column, StringComparison.OrdinalIgnoreCase));
                if (column != null)
                {
                    column.Valor = condition.Value; // Assign the old value from the WHERE clause, including quotes
                }
            }

            // Return the query element
            List<QueryElemento> resultado = new List<QueryElemento>()
            {
                new  QueryElemento() {
                Tipoquery = TipoQuery.Actualizacion,
                NombreTabla = tableName,
                Columnas = sqlColumns.ToArray(),
                Filtro = whereClause ,
                QueryCompleto = sqlQuery,
            } };
            return resultado;
        }


        private static List<QueryElemento> ProcessDelete(string sqlQuery)
        {
            // Extract table name
            var tableMatch = Regex.Match(sqlQuery, @"DELETE FROM\s+(\w+)", RegexOptions.IgnoreCase);
            string tableName = tableMatch.Groups[1].Value;

            List<QueryElemento> resultado = new List<QueryElemento>()
            {
                new  QueryElemento() {
                Tipoquery = TipoQuery.Eliminacion,
                NombreTabla = tableName
            }
            };
            return resultado;
        }


        private static List<QueryElemento> ProcessAlterTable(string sqlQuery)
        {
            // Extract table name
            var tableMatch = Regex.Match(sqlQuery, @"ALTER TABLE\s+(\w+)", RegexOptions.IgnoreCase);
            string tableName = tableMatch.Success ? tableMatch.Groups[1].Value : null;

            if (string.IsNullOrEmpty(tableName))
                throw new Exception("Invalid ALTER TABLE syntax. Table name not found.");

            // Determine the operation type
            TipoQuery tipoQuery;
            Match columnMatch;

            if (Regex.IsMatch(sqlQuery, @"\bADD COLUMN\b", RegexOptions.IgnoreCase))
            {
                tipoQuery = TipoQuery.CreacionColumna;
                columnMatch = Regex.Match(sqlQuery, @"ADD\s+COLUMN\s+(\w+)\s+(\w+)(?:\s+(NULL|NOT NULL))?(?:\s+DEFAULT\s+([^;]+))?", RegexOptions.IgnoreCase);
            }
            else if (Regex.IsMatch(sqlQuery, @"\bADD CONSTRAINT\b", RegexOptions.IgnoreCase))
            {
                tipoQuery = TipoQuery.CreacionRelacionFK;
                columnMatch = Regex.Match(sqlQuery, @"ADD CONSTRAINT\s+(\w+)\s+FOREIGN KEY\s*\((\w+)\)", RegexOptions.IgnoreCase);
            }
            else if (Regex.IsMatch(sqlQuery, @"\bDROP COLUMN\b", RegexOptions.IgnoreCase))
            {
                tipoQuery = TipoQuery.EliminarColumna;
                columnMatch = Regex.Match(sqlQuery, @"DROP COLUMN\s+(\w+)", RegexOptions.IgnoreCase);
            }
            else if (Regex.IsMatch(sqlQuery, @"\bMODIFY\b", RegexOptions.IgnoreCase))
            {
                tipoQuery = TipoQuery.ModificarColumna;
                columnMatch = Regex.Match(sqlQuery, @"MODIFY\s+(\w+)\s+(\w+)(?:\s+(NULL|NOT NULL))?", RegexOptions.IgnoreCase);
            }
            else if (Regex.IsMatch(sqlQuery, @"\bALTER COLUMN\b", RegexOptions.IgnoreCase))
            {
                tipoQuery = TipoQuery.ModificarColumna;
                if (Regex.IsMatch(sqlQuery, @"ALTER COLUMN\s+\w+\s+(SET|DROP)\s+NOT NULL", RegexOptions.IgnoreCase))
                {
                    columnMatch = Regex.Match(sqlQuery, @"ALTER COLUMN\s+(\w+)\s+(SET|DROP)\s+NOT NULL", RegexOptions.IgnoreCase);
                }
                else
                {
                    // Matches: ALTER COLUMN Fecha DATETIME
                    columnMatch = Regex.Match(sqlQuery, @"ALTER COLUMN\s+(\w+)\s+(\w+)", RegexOptions.IgnoreCase);
                }
            }
            else
            {
                throw new Exception("Unsupported ALTER TABLE operation.");
            }

            if (!columnMatch.Success)
                throw new Exception($"Invalid {tipoQuery} syntax. Could not extract column definition.");

            // Extract details based on the operation type
            string columnName = columnMatch.Groups[1].Value;
            string columnType = (tipoQuery == TipoQuery.CreacionColumna || tipoQuery == TipoQuery.ModificarColumna)
                                && columnMatch.Groups.Count > 2 ? columnMatch.Groups[2].Value : string.Empty;

            string nullabilityGroup = columnMatch.Groups.Count > 3 ? columnMatch.Groups[3].Value : string.Empty;
            string defaultValue = columnMatch.Groups.Count > 4 ? columnMatch.Groups[4].Value?.Trim() ?? "" : "";

            bool isNullable = !sqlQuery.Contains("NOT NULL", StringComparison.OrdinalIgnoreCase)
                              || sqlQuery.Contains("DROP NOT NULL", StringComparison.OrdinalIgnoreCase);

            TipoColumna tipoColumna = (tipoQuery == TipoQuery.CreacionColumna || tipoQuery == TipoQuery.ModificarColumna)
                ? MapColumnType(columnType)
                : tipoQuery == TipoQuery.CreacionRelacionFK ? TipoColumna.ForeignKey : TipoColumna.Desconocido;

            var column = new SQLColumnas
            {
                NombreColumna = columnName,
                Tipo = tipoColumna,
                PermiteNulo = isNullable,
                ValorXDefecto = defaultValue
            };

            return new List<QueryElemento>
    {
        new QueryElemento
        {
            Tipoquery = tipoQuery,
            NombreTabla = tableName,
            Columnas = [column],
            QueryCompleto = sqlQuery
        }
    };
        }



        private static List<QueryElemento> ProcessCreateView(string sqlQuery)
        {
            // Extract table name
            var tableMatch = Regex.Match(sqlQuery, @"CREATE VIEW\s+(\w+)", RegexOptions.IgnoreCase);
            string tableName = tableMatch.Groups[1].Value;

            // Extract column definitions (without CREATE TABLE clause)
            var columnDefinitionSection = sqlQuery
                .Replace(tableMatch.Groups[0].Value, "") // Remove the CREATE TABLE clause
                .Trim(' ', '(', ')', ';'); // Clean up surrounding characters

            // Regular expression to parse column definitions
            var columnMatches = Regex.Matches(
                columnDefinitionSection,
                @"(\w+)\s+(\w+)(?:\s+(IDENTITY\(\d+,\d+\)|NOT\s+NULL|NULL|DEFAULT\s+[\w\d']+|PRIMARY\s+KEY))*",
                RegexOptions.IgnoreCase
            );

            var columns = new List<SQLColumnas>();



            // Return the query element
            List<QueryElemento> resultado = new List<QueryElemento>()
            {
                new QueryElemento() {
                Tipoquery = TipoQuery.CreacionVista,
                NombreTabla = tableName,
                Columnas = columns.ToArray(),
                QueryCompleto = sqlQuery,
                }
            };

            return resultado;
        }

        private static TipoColumna MapColumnType(string sqlType)
        {
            return sqlType.ToUpper() switch
            {
                "INT IDENTITY(1,1) PRIMARY KEY" => TipoColumna.EnteroLlave,
                "PRIMARY" => TipoColumna.EnteroLlave,
                "INT" => TipoColumna.Entero,
                "NUMBER" => TipoColumna.Entero,
                "VARCHAR" => TipoColumna.Texto,
                "VARCHAR2" => TipoColumna.Texto,
                "BOOLEAN" => TipoColumna.Booleano,
                "BIT" => TipoColumna.Booleano,
                "DATE" => TipoColumna.Fecha,
                "DATETIME" => TipoColumna.DateTime,
                "TIMESTAMP" => TipoColumna.Timestamp,
                "DECIMAL" => TipoColumna.Decimal,
                _ => throw new Exception($"Unrecognized column type: {sqlType}")
            };
        }


    }
}

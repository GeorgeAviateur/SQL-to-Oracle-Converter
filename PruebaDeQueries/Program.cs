using QueriesFormation.Dominio;
using QueriesFormation.Infrastructure.Template.General;
using QueriesFormation.Infrastructure.Template.Oracle;
using QueriesFormation.Infrastructure.Template.SQL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.IO;
using System.Runtime.Intrinsics.X86;


/*
Este proyecto se crea con el fin de agilizar el trabajo de crear scripts y nombrarlos. 
This project was created to optimize and make less effort in the scripts and templates creation.

Creado por Jorge Garcia :D



*** consultas soportadas y ejemplos:

Actualizar datos en tablas: @"UPDATE TABLA set C1 = 'V1' WHERE UPPER(C2)='V2';"

Insertar valores: @"insert into TABLA (C1, C2, Cn) Values (V1, V2, Vn);",
Crear tablas:
    @"CREATE TABLE TABLA (
    C1 INT IDENTITY(1,1) PRIMARY KEY, 
	C2 INT ,
    C3 VARCHAR(200) NOT NULL, 
    C4 DATETIME NOT NULL DEFAULT 1, 
	C5 DATETIME NULL, 
);",


Crear Columnas: @"ALTER TABLE TABLA ADD C1 BIT NULL DEFAULT 0;",
Modificar Columnas: @"ALTER TABLE EntregaEpp ALTER COLUMN Fecha DATETIME;"

Llaves Foráneas: @"ALTER TABLE TABLA
						ADD CONSTRAINT FK_MILLAVE
						FOREIGN KEY (C1)
						REFERENCES TABLA-RELACIONADA (C1-RELACIONADA); ",


'DATE.NOW' = coloque esto para la fecha de hoy. Ejemplo: update entregaEPP set TipoEntrega='TAREA' WHERE fechaEntrega < 'DATE.NOW';


**** NOTAS: 
- No soporta [ ]
- Soporta mayusculas y minusculas.
- REVISAR LOS QUERIES Y SUS VALIDACIONES PARA NO DUPLICAR INFORMACION EN DB.

 */



string[] QuerySQL = {
@"insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
values ('EPP Solicitud',329, 1, 0, 'Plantilla para solicitar un EPP');",

@"insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
values ('EPP Respuesta Solicitud',330, 1, 0, 'Plantilla para ver la respuesta a Solicitud de un EPP'); ",

@"insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
values ('EPP Tarea de Mantenimiento',331, 1, 0, 'Plantilla para ver los mantenimientos del EPP'); ",

@"insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
values ('EPP Tarea de Reposicion',332, 1, 0, 'Plantilla para ver las reposiciones del EPP');",

};


SQLElemento elem = new SQLElemento()
{
    Consecutivo = 21,
    Descripcion = "Adicion de inserts a plantilla",
    NombreModulo = "SST EPP Indices",
    NombreScript = "Indexamiento de tablas Epp",
    Autor = "Jorge Garcia",
    Version = 1,
    FechaCreacion = DateTime.Now,
    Query = new List<QueryElemento>()
};


for (int i = 0; i < QuerySQL.Length; i++)
{
    List <QueryElemento> elementos = ProcesarQuery.ProcessQuery(QuerySQL[i]);
    if (elementos != null)
    {
        foreach (QueryElemento elemento in elementos)
        {
            elem.Query.Add(elemento);

        }
    }
}
if (elem.Query.Count > 0)
{

    string NombreScript = elem.NombreScript;
    SqlTemplate sqlDoc = new SqlTemplate();
    OracleTemplate oracleDoc = new OracleTemplate();


    elem.NombreScript = sqlDoc.GenerateScriptName(elem);
    string sqlFilePath = elem.NombreScript + ".sql";
    string sqlFinal = sqlDoc.GenerateScript(elem);


    elem.NombreScript = NombreScript;
    elem.NombreScript = oracleDoc.GenerateScriptName(elem);
    string oracleFilePath = elem.NombreScript + ".sql";
    string OracleFinal = oracleDoc.GenerateScript(elem);

    /*
    Console.WriteLine(OracleFinal);
    Console.ReadLine();
    */





    try
    {
        // Write sqlFinal to SqlScript.txt
        File.WriteAllText(sqlFilePath, sqlFinal);
        Console.WriteLine($"SQL script written to {sqlFilePath}");

        // Write OracleFinal to OracleScript.txt
        File.WriteAllText(oracleFilePath, OracleFinal);
        Console.WriteLine($"Oracle script written to {oracleFilePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}
else
{

    Console.WriteLine("No hay elementos para insertar");
}
//Console.ReadLine();

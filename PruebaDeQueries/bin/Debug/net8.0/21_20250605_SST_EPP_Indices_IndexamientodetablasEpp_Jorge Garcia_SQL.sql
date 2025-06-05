/*
Descripción: Adicion de inserts a plantilla
Módulo: SST EPP Indices
Nombre: 21_20250605_SST_EPP_Indices_IndexamientodetablasEpp_Jorge Garcia_SQL
Autor: Jorge Garcia
Version: 1
Fecha: 2025-06-05
*/


DECLARE @Modulo NVARCHAR(250), @Script NVARCHAR(250);
SET @Modulo = 'SST EPP Indices';
SET @Script = '21_20250605_SST_EPP_Indices_IndexamientodetablasEpp_Jorge Garcia_SQL';


BEGIN TRY

   IF NOT EXISTS (SELECT * FROM plantilla WHERE UPPER (nomplantilla ) = 'EPP SOLICITUD' AND codplantillaflujo  = 329 AND activo  = 1 AND esagendable  = 0 AND UPPER (descripcion ) = 'PLANTILLA PARA SOLICITAR UN EPP')

      BEGIN
      insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
      values ('EPP Solicitud',329, 1, 0, 'Plantilla para solicitar un EPP');

         PRINT 'EL VALOR EN LA TABLA plantilla FUE INSERTADO.'
      END
  ELSE
      BEGIN
        PRINT 'EL VALOR A INGRESAR EN plantilla YA EXISTE.'
      END

   IF NOT EXISTS (SELECT * FROM plantilla WHERE UPPER (nomplantilla ) = 'EPP RESPUESTA SOLICITUD' AND codplantillaflujo  = 330 AND activo  = 1 AND esagendable  = 0 AND UPPER (descripcion ) = 'PLANTILLA PARA VER LA RESPUESTA A SOLICITUD DE UN EPP')

      BEGIN
      insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
      values ('EPP Respuesta Solicitud',330, 1, 0, 'Plantilla para ver la respuesta a Solicitud de un EPP'); 

         PRINT 'EL VALOR EN LA TABLA plantilla FUE INSERTADO.'
      END
  ELSE
      BEGIN
        PRINT 'EL VALOR A INGRESAR EN plantilla YA EXISTE.'
      END

   IF NOT EXISTS (SELECT * FROM plantilla WHERE UPPER (nomplantilla ) = 'EPP TAREA DE MANTENIMIENTO' AND codplantillaflujo  = 331 AND activo  = 1 AND esagendable  = 0 AND UPPER (descripcion ) = 'PLANTILLA PARA VER LOS MANTENIMIENTOS DEL EPP')

      BEGIN
      insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
      values ('EPP Tarea de Mantenimiento',331, 1, 0, 'Plantilla para ver los mantenimientos del EPP'); 

         PRINT 'EL VALOR EN LA TABLA plantilla FUE INSERTADO.'
      END
  ELSE
      BEGIN
        PRINT 'EL VALOR A INGRESAR EN plantilla YA EXISTE.'
      END

   IF NOT EXISTS (SELECT * FROM plantilla WHERE UPPER (nomplantilla ) = 'EPP TAREA DE REPOSICION' AND codplantillaflujo  = 332 AND activo  = 1 AND esagendable  = 0 AND UPPER (descripcion ) = 'PLANTILLA PARA VER LAS REPOSICIONES DEL EPP')

      BEGIN
      insert into plantilla (nomplantilla, codplantillaflujo, activo, esagendable, descripcion)
      values ('EPP Tarea de Reposicion',332, 1, 0, 'Plantilla para ver las reposiciones del EPP');

         PRINT 'EL VALOR EN LA TABLA plantilla FUE INSERTADO.'
      END
  ELSE
      BEGIN
        PRINT 'EL VALOR A INGRESAR EN plantilla YA EXISTE.'
      END
    -- Log errores en la tabla SCRIPT
    INSERT INTO [SCRIPT] ([BaseDeDatos], [Modulo], [Script], SERVERNAME, USUARIO)
    SELECT DB_NAME(), @Modulo, @Script, @@SERVERNAME, SYSTEM_USER;


END TRY
BEGIN CATCH
    -- Log errores en la tabla SCRIPT
    INSERT INTO [SCRIPT] ([BaseDeDatos], [Modulo], [Script], [ErrorNumber], [ErrorSeverity], [ErrorState], [ErrorProcedure], [ErrorLine], [ErrorMessage], SERVERNAME, USUARIO)
    SELECT DB_NAME(), @Modulo, @Script, ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), @@SERVERNAME, SYSTEM_USER;

    PRINT 'ERROR LINEA: ' + CONVERT(VARCHAR, ERROR_LINE()) + ' MENSAJE: ' + ERROR_MESSAGE();
    THROW 50001, 'ERROR', 1;

END CATCH;


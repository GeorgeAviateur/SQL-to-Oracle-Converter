/*
Descripción: Se modifican las columnas de entrega para hacer más precisa la validación de las solicitudes
Módulo: SST EPP Solicitud EPP
Nombre: 19_20250409_SST_EPP_Solicitud_EPP_AjusteColumnaEntregas_Jorge Garcia_SQL
Autor: Jorge Garcia
Version: 1
Fecha: 2025-04-09
*/


DECLARE @Modulo NVARCHAR(250), @Script NVARCHAR(250);
SET @Modulo = 'SST EPP Solicitud EPP';
SET @Script = '19_20250409_SST_EPP_Solicitud_EPP_AjusteColumnaEntregas_Jorge Garcia_SQL';


BEGIN TRY

   IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EntregaEpp' AND COLUMN_NAME = 'Fecha' AND DATA_TYPE <> 'DateTime')
      BEGIN
      ALTER TABLE EntregaEpp
      ALTER COLUMN Fecha DATETIME;

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA EntregaEpp CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EntregaEpp' AND COLUMN_NAME = 'FechaEntrega' AND DATA_TYPE <> 'DateTime')
      BEGIN
      ALTER TABLE EntregaEpp
      ALTER COLUMN FechaEntrega DATETIME NULL;

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA EntregaEpp CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
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


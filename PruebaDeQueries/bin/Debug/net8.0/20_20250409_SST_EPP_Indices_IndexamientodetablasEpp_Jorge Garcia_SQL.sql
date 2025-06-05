/*
Descripción: Se crean indices a la tabla para mejorar su busqueda en estos módulos de epp
Módulo: SST EPP Indices
Nombre: 20_20250409_SST_EPP_Indices_IndexamientodetablasEpp_Jorge Garcia_SQL
Autor: Jorge Garcia
Version: 1
Fecha: 2025-04-09
*/


DECLARE @Modulo NVARCHAR(250), @Script NVARCHAR(250);
SET @Modulo = 'SST EPP Indices';
SET @Script = '20_20250409_SST_EPP_Indices_IndexamientodetablasEpp_Jorge Garcia_SQL';


BEGIN TRY

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'SstFuncionario' AND c.name = 'CodCargo')

      BEGIN
      CREATE INDEX idx_codCargo ON SstFuncionario (CodCargo);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA SstFuncionario CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'SsteppCargo' AND c.name = 'CodSSTEPP')

      BEGIN
      CREATE INDEX idx_codSSTEPP ON SsteppCargo (CodSSTEPP);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA SsteppCargo CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'SstFuncionario' AND c.name = 'Id_SstFuncionario')

      BEGIN
      CREATE INDEX idx_idSstFuncionario ON SstFuncionario (Id_SstFuncionario);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA SstFuncionario CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'SolicitudMantenimientoEpp' AND c.name = 'CodSstFuncionario')

      BEGIN
      CREATE INDEX idx_idSstFuncionario_solicitud ON SolicitudMantenimientoEpp (CodSstFuncionario);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA SolicitudMantenimientoEpp CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'ElementosEppSolicitados' AND c.name = 'IdSolicitudEpp')

      BEGIN
      CREATE INDEX idx_idSolicitud_ElementosEppSolicitados ON ElementosEppSolicitados (IdSolicitudEpp);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA ElementosEppSolicitados CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'ElementosEppSolicitados' AND c.name = 'CodSstepp')

      BEGIN
      CREATE INDEX idx_idEpp_ElementosEppSolicitados ON ElementosEppSolicitados (CodSstepp);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA ElementosEppSolicitados CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'EntregaEpp' AND c.name = 'CodUsuario')

      BEGIN
      CREATE INDEX idx_codUsuarioEntregaEpp ON EntregaEpp (CodUsuario);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA EntregaEpp CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'EntregaDetalleEpp' AND c.name = 'CodSSTFuncionario')

      BEGIN
      CREATE INDEX idx_codFuncionarioEntregaDetalleEpp ON EntregaDetalleEpp (CodSSTFuncionario);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA EntregaDetalleEpp CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'EntregaDetalleEpp' AND c.name = 'CodEntregaEPP')

      BEGIN
      CREATE INDEX idx_codEntregaEntregaDetalleEpp ON EntregaDetalleEpp (CodEntregaEPP);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA EntregaDetalleEpp CON ÉXITO.'
      END
  ELSE
      BEGIN
        PRINT 'SE REALIZÓ ESTE PROCESO ANTERIORMENTE'
      END

   IF NOT EXISTS (
 SELECT 1 FROM sys.indexes i
                        JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                        JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                        JOIN sys.tables t ON i.object_id = t.object_id
 WHERE t.name = 'EntregaDetalleEpp' AND c.name = 'CodSSTEPP')

      BEGIN
      CREATE INDEX idx_codEppEntregaDetalleEpp ON EntregaDetalleEpp (CodSSTEPP);

         PRINT 'SE REALIZÓ EL PROCESO DE LA TABLA EntregaDetalleEpp CON ÉXITO.'
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


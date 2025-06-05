/*
Descripción: Se crean indices a la tabla para mejorar su busqueda en estos módulos de epp
Módulo: SST EPP Indices
Nombre: 20_20250409_SST EPP Indices_IndexamientodetablasEpp_Jorge Garcia_ORA
Autor: Jorge Garcia
Version: 1
Fecha: 2025-04-09
*/


DECLARE
l_modulo VARCHAR(250) := 'SST EPP Indices';
l_script VARCHAR(250) := '20_20250409_SST EPP Indices_IndexamientodetablasEpp_Jorge Garcia_ORA';
l_BaseDeDatos VARCHAR2(100);
l_servername VARCHAR2(100);
l_usuario VARCHAR2(100);
l_err_num NUMBER := 0;
l_err_msg VARCHAR2(255) := '';


BEGIN
   SELECT GLOBAL_NAME INTO l_BaseDeDatos FROM GLOBAL_NAME;
   l_servername:= sys_context('userenv', 'server_host');
   SELECT USER INTO l_usuario FROM dual;



   DECLARE
      v_indexExists_1 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_1 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'SSTFUNCIONARIO' AND COLUMN_NAME = 'CODCARGO';
      IF v_indexExists_1 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice SstFuncionario...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_CODCARGO ON SSTFUNCIONARIO(CODCARGO)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_2 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_2 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'SSTEPPCARGO' AND COLUMN_NAME = 'CODSSTEPP';
      IF v_indexExists_2 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice SsteppCargo...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_CODSSTEPP ON SSTEPPCARGO(CODSSTEPP)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_3 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_3 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'SSTFUNCIONARIO' AND COLUMN_NAME = 'ID_SSTFUNCIONARIO';
      IF v_indexExists_3 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice SstFuncionario...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_IDSSTFUNCIONARIO ON SSTFUNCIONARIO(ID_SSTFUNCIONARIO)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_4 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_4 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'SOLICITUDMANTENIMIENTOEPP' AND COLUMN_NAME = 'CODSSTFUNCIONARIO';
      IF v_indexExists_4 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice SolicitudMantenimientoEpp...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_IDSSTFUNCIONARIO_SOLICITUD ON SOLICITUDMANTENIMIENTOEPP(CODSSTFUNCIONARIO)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_5 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_5 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'ELEMENTOSEPPSOLICITADOS' AND COLUMN_NAME = 'IDSOLICITUDEPP';
      IF v_indexExists_5 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice ElementosEppSolicitados...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_IDSOLICITUD_ELEMENTOSEPPSOLICITADOS ON ELEMENTOSEPPSOLICITADOS(IDSOLICITUDEPP)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_6 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_6 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'ELEMENTOSEPPSOLICITADOS' AND COLUMN_NAME = 'CODSSTEPP';
      IF v_indexExists_6 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice ElementosEppSolicitados...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_IDEPP_ELEMENTOSEPPSOLICITADOS ON ELEMENTOSEPPSOLICITADOS(CODSSTEPP)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_7 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_7 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'ENTREGAEPP' AND COLUMN_NAME = 'CODUSUARIO';
      IF v_indexExists_7 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice EntregaEpp...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_CODUSUARIOENTREGAEPP ON ENTREGAEPP(CODUSUARIO)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_8 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_8 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'ENTREGADETALLEEPP' AND COLUMN_NAME = 'CODSSTFUNCIONARIO';
      IF v_indexExists_8 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice EntregaDetalleEpp...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_CODFUNCIONARIOENTREGADETALLEEPP ON ENTREGADETALLEEPP(CODSSTFUNCIONARIO)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_9 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_9 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'ENTREGADETALLEEPP' AND COLUMN_NAME = 'CODENTREGAEPP';
      IF v_indexExists_9 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice EntregaDetalleEpp...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_CODENTREGAENTREGADETALLEEPP ON ENTREGADETALLEEPP(CODENTREGAEPP)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_indexExists_10 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_indexExists_10 FROM USER_IND_COLUMNS WHERE TABLE_NAME  = 'ENTREGADETALLEEPP' AND COLUMN_NAME = 'CODSSTEPP';
      IF v_indexExists_10 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('Creando Indice EntregaDetalleEpp...');

            EXECUTE IMMEDIATE '
               CREATE INDEX IDX_CODEPPENTREGADETALLEEPP ON ENTREGADETALLEEPP(CODSSTEPP)
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;
COMMIT;
EXCEPTION
WHEN OTHERS THEN 
l_err_num := SQLCODE;
l_err_msg := SQLERRM;
ROLLBACK;

-- Log the error 
INSERT INTO SCRIPT (BaseDeDatos, Modulo, Script, ErrorNumber, ErrorMessage, SERVERNAME, USUARIO)
VALUES (l_BaseDeDatos, l_modulo, l_script, l_err_num, l_err_msg, l_servername, l_usuario);

COMMIT;

-- Raise application error    
RAISE_APPLICATION_ERROR(-20001, DBMS_UTILITY.FORMAT_ERROR_BACKTRACE);

END;
/*
select * from script order by 1 desc
*/



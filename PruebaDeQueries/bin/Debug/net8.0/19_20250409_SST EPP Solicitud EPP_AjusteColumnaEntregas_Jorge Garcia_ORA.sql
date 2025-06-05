/*
Descripción: Se modifican las columnas de entrega para hacer más precisa la validación de las solicitudes
Módulo: SST EPP Solicitud EPP
Nombre: 19_20250409_SST EPP Solicitud EPP_AjusteColumnaEntregas_Jorge Garcia_ORA
Autor: Jorge Garcia
Version: 1
Fecha: 2025-04-09
*/


DECLARE
l_modulo VARCHAR(250) := 'SST EPP Solicitud EPP';
l_script VARCHAR(250) := '19_20250409_SST EPP Solicitud EPP_AjusteColumnaEntregas_Jorge Garcia_ORA';
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
      v_columnExists_1 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_columnExists_1 FROM USER_TAB_COLUMNS WHERE TABLE_NAME  = 'ENTREGAEPP' AND  COLUMN_NAME  = 'FECHA' AND DATA_TYPE != 'TIMESTAMP(6)' ;
      IF v_columnExists_1 = 1 THEN
         DBMS_OUTPUT.PUT_LINE('Modificando Columna EntregaEpp...');

            EXECUTE IMMEDIATE '
               ALTER TABLE ENTREGAEPP MODIFY (FECHA TIMESTAMP(6))
               ';
         DBMS_OUTPUT.PUT_LINE('SE PROCES+O EL QUERY CON ÉXITO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('ESTE PROCESO SE EJECUTÓ ANTERIORMENTE');
      END IF;
   END;

   DECLARE
      v_columnExists_2 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_columnExists_2 FROM USER_TAB_COLUMNS WHERE TABLE_NAME  = 'ENTREGAEPP' AND  COLUMN_NAME  = 'FECHAENTREGA' AND DATA_TYPE != 'TIMESTAMP(6)' ;
      IF v_columnExists_2 = 1 THEN
         DBMS_OUTPUT.PUT_LINE('Modificando Columna EntregaEpp...');

            EXECUTE IMMEDIATE '
               ALTER TABLE ENTREGAEPP MODIFY (FECHAENTREGA TIMESTAMP(6))
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



/*
Descripción: Adicion de inserts a plantilla
Módulo: SST EPP Indices
Nombre: 21_20250605_SST EPP Indices_IndexamientodetablasEpp_Jorge Garcia_ORA
Autor: Jorge Garcia
Version: 1
Fecha: 2025-06-05
*/


DECLARE
l_modulo VARCHAR(250) := 'SST EPP Indices';
l_script VARCHAR(250) := '21_20250605_SST EPP Indices_IndexamientodetablasEpp_Jorge Garcia_ORA';
l_BaseDeDatos VARCHAR2(100);
l_servername VARCHAR2(100);
l_usuario VARCHAR2(100);
l_err_num NUMBER := 0;
l_err_msg VARCHAR2(255) := '';


BEGIN
   SELECT GLOBAL_NAME INTO l_BaseDeDatos FROM GLOBAL_NAME;
   l_servername:= sys_context('userenv', 'server_host');
   SELECT USER INTO l_usuario FROM dual;



   DECLARE v_tableEmpty_1 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_tableEmpty_1 FROM PLANTILLA WHERE
 UPPER(NOMPLANTILLA) = 'EPP Solicitud' AND  CODPLANTILLAFLUJO = 329 AND  ACTIVO = 1 AND  ESAGENDABLE = 0 AND  UPPER(DESCRIPCION) = 'Plantilla para solicitar un EPP';

      IF v_tableEmpty_1 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('La tabla PLANTILLA está vacía. Insertando Registros...');

            EXECUTE IMMEDIATE '
               INSERT INTO PLANTILLA (NOMPLANTILLA,    CODPLANTILLAFLUJO,    ACTIVO,    ESAGENDABLE,    DESCRIPCION)
         VALUES (
            ''EPP Solicitud'',
            329,
            1,
            0,
            ''Plantilla para solicitar un EPP'')               ';
         DBMS_OUTPUT.PUT_LINE('EL VALOR EN LA TABLA plantilla FUE INSERTADO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('EL VALOR A INGRESAR EN plantilla YA EXISTE.');
      END IF;
   END;

   DECLARE v_tableEmpty_2 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_tableEmpty_2 FROM PLANTILLA WHERE
 UPPER(NOMPLANTILLA) = 'EPP Respuesta Solicitud' AND  CODPLANTILLAFLUJO = 330 AND  ACTIVO = 1 AND  ESAGENDABLE = 0 AND  UPPER(DESCRIPCION) = 'Plantilla para ver la respuesta a Solicitud de un EPP';

      IF v_tableEmpty_2 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('La tabla PLANTILLA está vacía. Insertando Registros...');

            EXECUTE IMMEDIATE '
               INSERT INTO PLANTILLA (NOMPLANTILLA,    CODPLANTILLAFLUJO,    ACTIVO,    ESAGENDABLE,    DESCRIPCION)
         VALUES (
            ''EPP Respuesta Solicitud'',
            330,
            1,
            0,
            ''Plantilla para ver la respuesta a Solicitud de un EPP'')               ';
         DBMS_OUTPUT.PUT_LINE('EL VALOR EN LA TABLA plantilla FUE INSERTADO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('EL VALOR A INGRESAR EN plantilla YA EXISTE.');
      END IF;
   END;

   DECLARE v_tableEmpty_3 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_tableEmpty_3 FROM PLANTILLA WHERE
 UPPER(NOMPLANTILLA) = 'EPP Tarea de Mantenimiento' AND  CODPLANTILLAFLUJO = 331 AND  ACTIVO = 1 AND  ESAGENDABLE = 0 AND  UPPER(DESCRIPCION) = 'Plantilla para ver los mantenimientos del EPP';

      IF v_tableEmpty_3 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('La tabla PLANTILLA está vacía. Insertando Registros...');

            EXECUTE IMMEDIATE '
               INSERT INTO PLANTILLA (NOMPLANTILLA,    CODPLANTILLAFLUJO,    ACTIVO,    ESAGENDABLE,    DESCRIPCION)
         VALUES (
            ''EPP Tarea de Mantenimiento'',
            331,
            1,
            0,
            ''Plantilla para ver los mantenimientos del EPP'')               ';
         DBMS_OUTPUT.PUT_LINE('EL VALOR EN LA TABLA plantilla FUE INSERTADO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('EL VALOR A INGRESAR EN plantilla YA EXISTE.');
      END IF;
   END;

   DECLARE v_tableEmpty_4 NUMBER := 0;
   BEGIN
      SELECT COUNT(*) INTO v_tableEmpty_4 FROM PLANTILLA WHERE
 UPPER(NOMPLANTILLA) = 'EPP Tarea de Reposicion' AND  CODPLANTILLAFLUJO = 332 AND  ACTIVO = 1 AND  ESAGENDABLE = 0 AND  UPPER(DESCRIPCION) = 'Plantilla para ver las reposiciones del EPP';

      IF v_tableEmpty_4 = 0 THEN
         DBMS_OUTPUT.PUT_LINE('La tabla PLANTILLA está vacía. Insertando Registros...');

            EXECUTE IMMEDIATE '
               INSERT INTO PLANTILLA (NOMPLANTILLA,    CODPLANTILLAFLUJO,    ACTIVO,    ESAGENDABLE,    DESCRIPCION)
         VALUES (
            ''EPP Tarea de Reposicion'',
            332,
            1,
            0,
            ''Plantilla para ver las reposiciones del EPP'')               ';
         DBMS_OUTPUT.PUT_LINE('EL VALOR EN LA TABLA plantilla FUE INSERTADO.');
      ELSE
         DBMS_OUTPUT.PUT_LINE('EL VALOR A INGRESAR EN plantilla YA EXISTE.');
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



# SQL-to-Oracle-Converter

This is a converter from SQL scripts to Oracle and SQL Server statements. This also works as a prettifier, giving a decent final result.
This exports two files: 1 for Oracle and 1 for SQL Server.
This project was created to optimise the script creation.


## **SPECS 🧑‍💻 :**
- This project has been created as an architectural practice to learn Clean Architecture. Although I am not using the database to collect data, I am using some models on the domain project.

## **LANGUAGE 🇨🇴 :**
- This project mainly uses Spanish as a core language for some customer needs.

## **SUPPORTED QUERIES 🤙 :**
Uppercase and lowercase are both supported.

### - _UPDATE_:
    @"UPDATE MYTABLE set C1 = 'V1' WHERE UPPER(C2)='V2';"
 
### - _INSERT_:
    @"insert into MYTABLE (C1, C2, Cn) Values (V1, V2, Vn);"

### - _CREATE TABLE_:
    @"CREATE TABLE MYTABLE (
      C1 INT IDENTITY(1,1) PRIMARY KEY, 
  	  C2 INT,
      C3 VARCHAR(200) NOT NULL, 
      C4 DATETIME NOT NULL DEFAULT 1, 
  	  C5 DATETIME NULL, );"

### - _CREATE COLUMN_:
      @"ALTER TABLE TABLA ADD C1 BIT NULL DEFAULT 0;"

### - _MODIFY COLUMN_:
      @"ALTER TABLE EntregaEpp ALTER COLUMN Fecha DATETIME;"

### - _ADD FOREIGN KEYS_:
      @"ALTER TABLE TABLA 
      ADD CONSTRAINT FK_MILLAVE FOREIGN KEY (C1) 
      REFERENCES TABLA-RELACIONADA (C1-RELACIONADA); ",

### - _'DATE.NOW'_ :
 Use 'DATE.NOW' to set today date. 
 E.G: UPDATE MYTABLE SET VALUEONE='TASK' WHERE MYDATECOLUMN < 'DATE.NOW';

## ** **IT DOESN'T SUPPORT: 😞 ** 

- '[' AND ']' CHARACTERS
- SPECIAL CHARACTERS OUTSIDE THE SCRIPT.

## ☣️☣️ ***PLEASE _ALWAYS_ DOUBLE CHECK THE RESULTS!! SOMETIMES IT COULD LEAD TO WRONG VALIDATIONS OR SPECIFIC ISSUES*** ☣️☣️

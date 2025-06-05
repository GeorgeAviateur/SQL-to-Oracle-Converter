using QueriesFormation.Dominio.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Dominio
{
    public class QueryElemento
    {
        /// <summary>
        /// Query es un valor general en caso de necesitar un query custom
        /// </summary>
        public string QueryAdicional;
        /// <summary>
        /// NombreTabla es el nombre actual de la tabla. 
        /// </summary>
        public string NombreTabla;
        /// <summary>
        /// NuevoNombreTabla se usa cuando se quiere modificar el valor de la tabla( como el nombre o algunas caracteristicas)
        /// </summary>
        public string NuevoNombreTabla;
        /// <summary>
        /// NombreColumna este valor sirve para modificar un valor con update o dar el nombre actual de la columna si se desea modificar
        /// </summary>
        public string NombreColumna;
        /// <summary>
        /// NuevoNombreColumna Este valor solo se usa cuando se desea modificar el nombre de la columna o cambiar configuraciones de esta
        /// </summary>
        public string NuevoNombreColumna;
        /// <summary>
        /// Valor este campo se usa cuando se quiere insertar un valor en la tabla
        /// </summary>
        public string Valor;
        public string NuevoValor;
        public string ValidacionCustom;
        public TipoQuery Tipoquery;
        public SQLColumnas[] Columnas;
        public string Filtro;
        public string QueryCompleto;
    }
}

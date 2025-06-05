using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueriesFormation.Dominio.Enum
{
    public enum TipoQuery
    {
        Insercion,
        Eliminacion,
        Actualizacion,


        CreacionTabla,
        CreacionColumna,
        CreacionVista,
        CreacionRelacionFK,
        CreacionIndice,

        ModificarTabla,
        ModificarColumna,
        ModificarValor,

        ElliminarTabla,
        EliminarColumna,
        EliminarValor,
    }
}

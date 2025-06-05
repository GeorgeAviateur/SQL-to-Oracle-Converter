using QueriesFormation.Dominio;

namespace QueriesFormation.Dominio
{
    public class SQLElemento
    {
        public int Consecutivo;
        public int Version;
        public string Autor;
        public string Descripcion;
        public string NombreModulo;
        public string NombreScript;
        public DateTime FechaCreacion;
        public List<QueryElemento> Query;
        

    }
}

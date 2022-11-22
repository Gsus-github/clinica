using System.Xml;

namespace ClinicaNS{
    /*
+--------------------------------------------------------------------------------------------------
| Clase Configuración para guardar todos los parámetros de la aplicación.
+--------------------------------------------------------------------------------------------------
*/
        

    public class Configuracion{
        public String rutaMito{get; set;}="Vacio";
        public String rutaBdd{get; set;}="Vacio";
        public String rutaLogs{get; set;}="Vacio";
        public String rutaDocs{get; set;}="Vacio";
        public String rutaBack{get; set;}="Vacio";

        public Configuracion(){
            XmlDocument doc = new XmlDocument();
            XmlNode nodo;
            doc.Load("config.xml");
            if (doc!=null){
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='MITO']/direccion");
                if (nodo!=null)
                    rutaMito = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='BDD']/direccion");
                if (nodo!=null)
                    rutaBdd = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='LOGS']/direccion");
                if (nodo!=null)
                    rutaLogs = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='DOCS']/direccion");
                if (nodo!=null)
                    rutaDocs = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='BACK']/direccion");
                if (nodo!=null)
                    rutaBack = nodo.InnerText;
            }
        }
    }
}
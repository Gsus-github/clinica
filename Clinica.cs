using System;
using System.Xml;

namespace ClinicaNS
{
    public class Clinica {
        public static void Main(string[] args)
        {
            Clinica clinica = new Clinica();

            Console.WriteLine("Pruebas para leer un archivo de configuración en xml");

            //Crear una instancia a un objeto que contenga la configuración.
            Configuracion configuracion = new Configuracion();
            
            //Instancia al documento XML
            XmlDocument doc = new XmlDocument();
            configuracion = cargarConfiguracion(doc);
            

            Console.WriteLine("MITO: " + configuracion.rutaMito);
            Console.WriteLine("BDD: " + configuracion.rutaBdd);
            Console.WriteLine("LOGS: " + configuracion.rutaLogs);
            Console.WriteLine("DOCS: " + configuracion.rutaDocs);
            Console.WriteLine("BACKUP: " + configuracion.rutaBack);

            /*
            //Cargar el documento
            doc.Load("config.xml");
            //Carga los datos en la configuración.
            XmlNode nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='MITO']/direccion");
            configuracion.rutaMito = nodo.InnerText;
            Console.WriteLine("Ruta a MITO: " + configuracion.rutaMito);
            */
        }

        public static  Configuracion cargarConfiguracion(XmlDocument doc){
            Configuracion confTemp = new Configuracion();
            //Cargar el documento xml
            doc.Load("config.xml");
            //Carga los datos en la configuración.
            XmlNode nodo;
            //Cargando rutas
            if (doc!=null){
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='MITO']/direccion");
                if (nodo!=null)
                    confTemp.rutaMito = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='BDD']/direccion");
                if (nodo!=null)
                    confTemp.rutaBdd = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='LOGS']/direccion");
                if (nodo!=null)
                    confTemp.rutaLogs = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='DOCS']/direccion");
                if (nodo!=null)
                    confTemp.rutaDocs = nodo.InnerText;
                nodo = doc.DocumentElement.SelectSingleNode("//configuracion/rutas/ruta[@name='BACK']/direccion");
                if (nodo!=null)
                    confTemp.rutaBack = nodo.InnerText;
            }
            return confTemp;
        }   
    }

        

    public class Configuracion{
        public String rutaMito{get; set;}="Vacio";
        public String rutaBdd{get; set;}="Vacio";
        public String rutaLogs{get; set;}="Vacio";
        public String rutaDocs{get; set;}="Vacio";
        public String rutaBack{get; set;}="Vacio";
    }


}

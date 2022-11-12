using System;
using System.Xml;

namespace Clinica
{
    class Clinica {
        static void Main(string[] args){
            Console.WriteLine("Pruebas para leer un archivo de configuración en xml");

            //Instancia al documento XML
            XmlDocument doc = new XmlDocument();
            //Cargar el documento
            doc.Load("config.xml");
            //Recorrer los nodos
            foreach(XmlNode nodo in doc.DocumentElement.ChildNodes){
                if (nodo.HasChildNodes){
                    foreach(XmlNode nodo2 in nodo.ChildNodes){
                        String ruta = nodo2.Attributes["name"].Value;
                        Console.WriteLine ("Cargando: " + ruta);
                        foreach(XmlNode nodo3 in nodo2.ChildNodes){
                            Console.WriteLine(">>" + nodo2.InnerText);
                        }
                    }
                }
            }
        }
    }

    class Configuracion{
        private String rutaMito{get; set;}="Vacio";
        private String rutaBdd{get; set;}="Vacio";
        private String rutaLogs{get; set;}="Vacio";
        private String rutaDocs{get; set;}="Vacio";
    }
}

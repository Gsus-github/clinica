using System;
using System.Xml;
using Microsoft.Data.Sqlite;
using System.IO;

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

            //Crear la base de datos principal
            BDDsqlite bdd = new BDDsqlite(configuracion.rutaBdd);
            
   
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

    public class BDDsqlite{

        public BDDsqlite(String ruta){
            ruta = Path.Combine(ruta, "clinica.db");
            Console.WriteLine("Abriendo base de datos principal en: " + ruta);
            if(!File.Exists(ruta)){
                Console.WriteLine("No existe el fichero de BDD");
            }else{
                Console.WriteLine("El fichero de BDD EXISTE");
                SqliteConnection sqlite_conexion = CreateConnection(ruta);
                String consulta = "SELECT nombreUsuario FROM 'tblUsuarios'";
                LeerDatos(sqlite_conexion, consulta);
            }
  
        }
        static SqliteConnection CreateConnection(String ruta)     {

                SqliteConnection sqlite_conn;
              
                // Crear nueva conexión a la BDD
                String cadenaConexion = "Data Source=" + ruta;
                sqlite_conn = new SqliteConnection(cadenaConexion);
                // Open the connection:
                try
                {
                    sqlite_conn.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().ToString());
                }
                return sqlite_conn;
        }

        static void LeerDatos(SqliteConnection sqlite_conn, String consulta){
            sqlite_conn.Open();
            SqliteCommand comando = new SqliteCommand(consulta, sqlite_conn);
            comando = sqlite_conn.CreateCommand();
            //comando.CommandText=@"SELECT nombreUsuario FROM tblUsuarios";
            comando.CommandText=("SELECT [nombreUsuario] FROM [main].[tblUsuarios]");
            using (var lector = comando.ExecuteReader()){
                while(lector.Read()){
                    var nombre = lector.GetString(0);
                    Console.WriteLine(nombre);
                }
            };
            sqlite_conn.Close();
        }

    }

}

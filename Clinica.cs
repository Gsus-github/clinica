using System;
using System.Xml;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Text;
using System.Security.Cryptography;

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

            //Conexión con la base de datos principal
            BDDsqlite bdd = new BDDsqlite(configuracion.rutaBdd, "clinica.db");

            //Trabajos con contraseñas, salt y hashes
            GestionPassword contrasenas = new GestionPassword();
            Console.WriteLine("Pass: ");
            String entrada1 = Console.ReadLine();
            Console.WriteLine("Sal: ");
            String entrada2 = Console.ReadLine();
            GestionPassword otraContrasena = new GestionPassword(entrada1, entrada2);



        }
/*
+--------------------------------------------------------------------------------------------------
| Cargando la configuración.
| Devuelve un objeto Configuración
+--------------------------------------------------------------------------------------------------
*/
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
    }

/*
+--------------------------------------------------------------------------------------------------
| Clase BDDsqlite para todas las operaciones relativas a las BDD en SQLite
+--------------------------------------------------------------------------------------------------
*/

    public class BDDsqlite{
        /*
        +------------------------------------------------------------------------------------------
        | Constructor
        +------------------------------------------------------------------------------------------
        */
        public BDDsqlite(String ruta, String nombreBdd){
            ruta = Path.Combine(ruta, nombreBdd);
            Console.WriteLine("Abriendo base de datos principal en: " + ruta);
            if(!File.Exists(ruta)){
                Console.WriteLine("No existe el fichero de BDD");
            }else{
                Console.WriteLine("El fichero de BDD EXISTE");
                //SqliteConnection sqlite_conexion = CreateConnection(ruta);
                //String consulta = "SELECT [nombreUsuario] FROM [main].[tblUsuarios]";
                //LeerDatos(sqlite_conexion, consulta);
            }
  
        }
        /*
        +------------------------------------------------------------------------------------------
        | Función que crea la conexión.
        +------------------------------------------------------------------------------------------
        */
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
        /*
        +------------------------------------------------------------------------------------------
        | Funcion que ejecuta una consulta
        +------------------------------------------------------------------------------------------
        */
        static void LeerDatos(SqliteConnection sqlite_conn, String consulta){

            //Abrir la conexión
            sqlite_conn.Open();

            //Crear el comando de consulta
            SqliteCommand comando = new SqliteCommand(consulta, sqlite_conn);
            comando = sqlite_conn.CreateCommand();
           
            comando.CommandText=(consulta);
            using (var lector = comando.ExecuteReader()){
                while(lector.Read()){
                    var nombre = lector.GetString(0);
                    Console.WriteLine(nombre);
                }
            };
            sqlite_conn.Close();
        }

    }

/*
+--------------------------------------------------------------------------------------------------
| Clase GestionPassword para gestionar las contraseñas
+--------------------------------------------------------------------------------------------------
*/
    public class GestionPassword{

        public GestionPassword(){
            byte[] mostrarSal;
            String passwordHasheada;
            String contrasenaTextoClaro = "";
            String salEnClaro;

            mostrarSal = CrearSal();
            Console.WriteLine("Sal: \n");
            for (int i = 0; i < mostrarSal.Length; i++){
                Console.Write(mostrarSal[i]);
            }
            salEnClaro = Encoding.ASCII.GetString(mostrarSal);
            Console.WriteLine("\n" + salEnClaro);

            Console.WriteLine("\n Contraseña texto plano: ");
            contrasenaTextoClaro = Console.ReadLine();
            passwordHasheada = crearPassHasheada(contrasenaTextoClaro, mostrarSal);
            Console.WriteLine("Contraseña hasheada: " + passwordHasheada);

        }

        public GestionPassword(String passClaro, String salClaro){
            byte[] salEnBytes;
            byte[] passEnBytes;
            String passComprobada;
            int i;
            
            salEnBytes = new byte[salClaro.Length];
            salEnBytes = Encoding.ASCII.GetBytes(salClaro);
            for(i = 0; i < salEnBytes.Length; i++){
                Console.Write(salEnBytes[i]);
            }
          
            passComprobada = crearPassHasheada(passClaro, salEnBytes);

            Console.WriteLine ("Comprobación: " + passComprobada);



        }


        /*
        +------------------------------------------------------------------------------------------
        | Generamos la sal
        +------------------------------------------------------------------------------------------
        */
        public static byte[] CrearSal(){
            //Genera un número aleatorio.
            RNGCryptoServiceProvider rng;
            Random numero = new Random();
            int tamanoSal;
            byte[] salByte;

            //Generamos un número aleatorio para darle un tamaño aleatorio a la sal de entre 4 y 10 carácteres
            tamanoSal = numero.Next(4, 8);

            //Creamos una array de bytes con el tamaño conseguido
            salByte = new byte[tamanoSal];
            
            //Rellenamos la sal con valores crypto fuertes
            rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salByte);

            //Devolvemos la sal
            return salByte;

        }

        public static String crearPassHasheada(String textoPlano, byte[] sal){
            String textoHasheado = "";
            byte[] plainTextBytes;
            byte[] textoPlanoConSal;
            int i;
            HashAlgorithm hash;
            byte[] hashBytes;

            //Convierte la password de texto plano en un array de bytes con caracteres UTF8
            plainTextBytes = Encoding.UTF8.GetBytes(textoPlano);

            //Sumamos la sal
            textoPlanoConSal = new byte[plainTextBytes.Length + sal.Length];
            for (i=0; i < plainTextBytes.Length; i++){
                textoPlanoConSal[i] = plainTextBytes[i];
            } 
            for (i = 0; i < sal.Length; i++){
                textoPlanoConSal[plainTextBytes.Length+i] = sal[i]; 
            }

            //Indicamos el tipo de hash que vamos a utilizar
            hash = new MD5CryptoServiceProvider();

            //Creamos el hash
            hashBytes = hash.ComputeHash(textoPlanoConSal);
            textoHasheado = Convert.ToBase64String(hashBytes);



            return textoHasheado;
        }
        
    }

}

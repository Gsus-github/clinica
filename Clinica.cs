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
        String contrasena{get; set;} = "";
        String sal{get; set;} = "";
        String contrasenaHasheada{get; set;} = "";

        public GestionPassword(){

            //Si se crea una instancia de GestionPassword sin argumentos, se genera una contraseña, una sal y una passsword hasheada
            contrasena = crearContrasena();
            Console.WriteLine("Nueva contraseña aleatoria: " + contrasena);
            sal = CrearSal();
            Console.WriteLine("Nueva sal: " + sal); 
            contrasenaHasheada = hashearContrasena(contrasena, sal);
            Console.WriteLine("Contraseña + sal + hash MD5: " + contrasenaHasheada);
            contrasenaHasheada = hashearContrasena(contrasena, sal);
            Console.WriteLine("Contraseña + sal + hash MD5: " + contrasenaHasheada);
        }
                
        public GestionPassword(String passClaro, String salClaro){
            contrasenaHasheada = hashearContrasena(passClaro, salClaro);
        }
        

        /*
        +------------------------------------------------------------------------------------------
        | Generamos la sal
        +------------------------------------------------------------------------------------------
        */
        public static String CrearSal(){
            String nuevaSal = "";
            String cadenaCaracteres = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz01234567890";
            char[] caracter;
            Random numeroAleatorio = new Random();
            int longitudSal=0;
            int i;

            //Determinamos aleatorioamente la longitud de la cadena (entre 8 y 16 carácteres):
            longitudSal = numeroAleatorio.Next(8,16);
            caracter = new char[longitudSal];


            //Creamos una cadena con el número de carácteres calculado
            for (i = 0; i < longitudSal; i++){
                caracter[i] = cadenaCaracteres[numeroAleatorio.Next(cadenaCaracteres.Length)];
            }

            nuevaSal = new String(caracter);
            return nuevaSal;

            /*
            //Genera un número aleatorio.
            RNGCryptoServiceProvider rng;
            Random numero = new Random();
            int tamanoSal;
            byte[] salByte;

            //Generamos un número aleatorio para darle un tamaño aleatorio a la sal de entre 4 y 10 carácteres
            tamanoSal = numero.Next(4, 16);

            //Creamos una array de bytes con el tamaño conseguido
            salByte = new byte[tamanoSal];
            
            //Rellenamos la sal con valores crypto fuertes
            rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salByte);
            Console.WriteLine(salByte + " - " +  salByte.ToString());

            //Devolvemos la sal
            return salByte;
            */

        }

        public static String hashearContrasena(String textoPlano, String sal){
            String textoHasheado = "";
            String textoPlanoConSal = "";
            byte[] textoBytes; 
            int i;
            HashAlgorithm hash;
            byte[] hashBytes;

            //Sumamos la sal
            textoPlanoConSal = textoPlano + sal;

            //Indicamos el tipo de hash que vamos a utilizar
            hash = new MD5CryptoServiceProvider();

            //Pasamos la cadena a bytes
            textoBytes = System.Text.Encoding.ASCII.GetBytes(textoPlanoConSal);

            //Creamos el hash
            hashBytes = hash.ComputeHash(textoBytes);
            textoHasheado = Convert.ToBase64String(textoBytes);

            return textoHasheado;
        }

        private static String crearContrasena(){
            String nuevaContrasena = "";
            String cadenaCaracteres = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz01234567890";
            char[] caracter;
            Random numeroAleatorio = new Random();
            int longitudContrasena=0;
            int i;

            //Determinamos aleatorioamente la longitud de la cadena (entre 8 y 16 carácteres):
            longitudContrasena = numeroAleatorio.Next(8,16);
            caracter = new char[longitudContrasena];


            //Creamos una cadena con el número de carácteres calculado
            for (i = 0; i < longitudContrasena; i++){
                caracter[i] = cadenaCaracteres[numeroAleatorio.Next(cadenaCaracteres.Length)];
            }

            nuevaContrasena = new String(caracter);
            return nuevaContrasena;


            

            return nuevaContrasena;
        }
        
        
    }

}

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

            //Creamos un nuevo usuario

            Usuario usuario = new Usuario();

            usuario.nombreUsuario ="usuario1";
            usuario.passwordUsuario = "Minisdef01";
           
            //Conexión con la base de datos principal
            BDDsqlite bdd = new BDDsqlite(configuracion.rutaBdd, "clinica.db");
            bdd.AbrirBdd();
            Boolean registrado = bdd.BuscarUsuario(usuario.nombreUsuario, usuario.passwordUsuario);
            bdd.CerrarBdd();
   


 
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
| Clase Usuario para guardar todos los datos del usuario.
+--------------------------------------------------------------------------------------------------
*/
    public class Usuario{
        public String nombreUsuario{get; set;}="";
        public String passwordUsuario{get; set;}="";
        public String saltUsuario{get; set;}="";
        public String rolUsuario{get;set;}="";
        public String dniUsuario{get;set;}="";
    }

/*
+--------------------------------------------------------------------------------------------------
| Clase BDDsqlite para todas las operaciones relativas a las BDD en SQLite
+--------------------------------------------------------------------------------------------------
*/

    public class BDDsqlite{

        public static SqliteConnection? sqlite_conexion;
        /*
        +------------------------------------------------------------------------------------------
        | Constructor
        +------------------------------------------------------------------------------------------
        */
        public BDDsqlite(String ruta, String nombreBdd){
            ruta = Path.Combine(ruta, nombreBdd);
            String cadenaConexion = "Data Source=" + ruta;

            if(File.Exists(ruta)){
                sqlite_conexion = new SqliteConnection(cadenaConexion);
            }
  
        }
        /*
        +------------------------------------------------------------------------------------------
        | Función que crea la conexión.
        +------------------------------------------------------------------------------------------
        
        public static SqliteConnection CreateConnection(String ruta)     {
                SqliteConnection sqlite_con;
                // Crear nueva conexión a la BDD
                String cadenaConexion = "Data Source=" + ruta;
                sqlite_con = new SqliteConnection(cadenaConexion);
                // Open the connection:
                try
                {
                    sqlite_con.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().ToString());
                }
                return sqlite_con;
        }
        */
        
        public void AbrirBdd(){
            try
                {
                    sqlite_conexion.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().ToString());
                }
        }

        public void CerrarBdd(){
            sqlite_conexion.Close();
        }
        
        /*
        +------------------------------------------------------------------------------------------
        | Funcion que ejecuta una consulta
        +------------------------------------------------------------------------------------------
        
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
        */

        internal bool BuscarUsuario(string nombreUsuario, string passwordUsuario)
        {
            String consulta = "SELECT [nombreUsuario], [password], [salt] FROM [main].[tblUsuarios] WHERE [nombreUsuario]=" + "'" + nombreUsuario + "'";
            SqliteCommand comando = new SqliteCommand(consulta, sqlite_conexion);
            comando = sqlite_conexion.CreateCommand();
            GestionPassword contra;
            AbrirBdd();
            comando.CommandText=(consulta);
            using(var lector = comando.ExecuteReader()){
                if(lector.Read()){
                    Console.WriteLine("El usuario existe: " + lector.GetString(0) + " su contraseña es " + lector.GetString(1) + " y su sal " + lector.GetString(2));
                    contra = new GestionPassword(passwordUsuario, lector.GetString(2));
                    if(contra.contrasenaHasheada == lector.GetString(1)){
                        Console.WriteLine("La contraseña coincide");
                        return true;
                    }else{
                        Console.WriteLine("La contraseña NO coincide");
                    }
                }else{
                    Console.WriteLine("El usuario NO existe");
                    return false;
                };

                return false;

            }

            throw new NotImplementedException();
        }
    }

/**
+--------------------------------------------------------------------------------------------------
| Clase GestionPassword para gestionar las contraseñas
+--------------------------------------------------------------------------------------------------
**/
    public class GestionPassword{
        public String contrasena{get; set;} = "";
        public String sal{get; set;} = "";
        public String contrasenaHasheada{get; set;} = "";

        public GestionPassword(){

            //Si se crea una instancia de GestionPassword sin argumentos, se genera una contraseña, una sal y una passsword hasheada
            contrasena = crearContrasena();
            sal = CrearSal();
            contrasenaHasheada = hashearContrasena(contrasena, sal);
        }
                
        public GestionPassword(String passClaro, String salClaro){
            contrasena = passClaro;
            sal = salClaro;
            contrasenaHasheada = hashearContrasena(passClaro, salClaro);
        }

        public GestionPassword(String passClaro){
            contrasena = passClaro;
            sal = CrearSal();
            contrasenaHasheada = hashearContrasena(passClaro, sal);
        }
        

        /*
        +------------------------------------------------------------------------------------------
        | Generamos la sal
        +------------------------------------------------------------------------------------------
        */
        public static String CrearSal(){
            String nuevaSal = "";
            String cadenaCaracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890";
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

        }

        public static String hashearContrasena(String textoPlano, String sal){
            String textoHasheado = "";
            String textoPlanoConSal = "";
            byte[] textoBytes; 
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
            String cadenaCaracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890";
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

        }
        
        
    }

}

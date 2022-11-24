using System;
using System.Xml;
using System.IO;
using System.Text;


namespace ClinicaNS
{
    public class Clinica {
        public static void Main(string[] args)
        {
            Clinica clinica = new Clinica();
            String nUsuario = "";
            String pUsuario = "";
            //Crear una instancia a un objeto que contenga la configuración.
            Configuracion configuracion = new Configuracion();
            Console.WriteLine("MITO: " + configuracion.rutaMito);
            Console.WriteLine("BDD: " + configuracion.rutaBdd);
            Console.WriteLine("LOGS: " + configuracion.rutaLogs);
            Console.WriteLine("DOCS: " + configuracion.rutaDocs);
            Console.WriteLine("BACKUP: " + configuracion.rutaBack);

            //Creamos un nuevo usuario

            Console.Write("Nombre de usuario: ");
            nUsuario = Console.ReadLine();
            //nUsuario = "usuario1";
            Console.Write("\nContraseña: ");
            pUsuario = Console.ReadLine();
            //pUsuario = "Minisdef01";


            Usuario usuario = new Usuario(nUsuario, pUsuario, configuracion);

            if (usuario.verificado){
                Console.WriteLine ("El usuario: " + usuario.nombreUsuario + " está verificado.");
            }else{
                Console.WriteLine ("EL usuario: " + usuario.nombreUsuario + " NO está verificado.");
            }
        }
    }
}

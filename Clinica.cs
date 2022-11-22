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
            //Crear una instancia a un objeto que contenga la configuración.
            Configuracion configuracion = new Configuracion();
            Console.WriteLine("MITO: " + configuracion.rutaMito);
            Console.WriteLine("BDD: " + configuracion.rutaBdd);
            Console.WriteLine("LOGS: " + configuracion.rutaLogs);
            Console.WriteLine("DOCS: " + configuracion.rutaDocs);
            Console.WriteLine("BACKUP: " + configuracion.rutaBack);

            //Creamos un nuevo usuario

            Usuario usuario = new Usuario("usuario1", "Minisdef01", configuracion);

            if (usuario.verificado){
                Console.WriteLine ("El usuario: " + usuario.nombreUsuario + " está verificado.");
                Console.WriteLine ("Pass hash: " + usuario.contrasena.contrasenaHasheada);
            }else{
                Console.WriteLine ("EL usuario: " + usuario.nombreUsuario + " NO está verificado.");
            }

            //Creamos otro usuario para poder comparar
            Usuario usuario2 = new Usuario("usuario33", "Minisdef01", configuracion);

            if (usuario.verificado){
                Console.WriteLine ("El usuario: " + usuario2.nombreUsuario + " está verificado.");
                Console.WriteLine ("Pass hash: " + usuario2.contrasena.contrasenaHasheada);
            }else{
                Console.WriteLine ("EL usuario: " + usuario2.nombreUsuario + " NO está verificado.");
            }
        }
    }
}

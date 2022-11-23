namespace ClinicaNS{
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
        public Boolean verificado{get; private set;} = false;
        public GestionPassword contrasena;

        public Usuario(String nombre, string passw, Configuracion configuracion){
            nombreUsuario = nombre;
            passwordUsuario = passw;
            contrasena = new GestionPassword(nombreUsuario, passwordUsuario);

            //Existe el usuario en la BDD??
            BDDsqlite bdd = new BDDsqlite(configuracion.rutaBdd, "clinica.db");
            bdd.AbrirBdd();
            if(bdd.BuscarUsuario(nombreUsuario)){
                Console.WriteLine("El usuario " + nombreUsuario + " EXISTE ");
                if(bdd.ComprobarContrasena(passwordUsuario)){
                    verificado = true;
                }else{
                    verificado = false;
                }
                Console.WriteLine("El usuario " + nombreUsuario + " NO EXISTE ");
                verificado = false;
            }

            //Su contraseña es correcta??


        }
    }
}
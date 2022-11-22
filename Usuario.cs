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

            BDDsqlite bdd = new BDDsqlite(configuracion.rutaBdd, "clinica.db");
            bdd.AbrirBdd();
            verificado = bdd.BuscarUsuario(nombreUsuario, passwordUsuario);
            bdd.CerrarBdd();

        }
    }
}
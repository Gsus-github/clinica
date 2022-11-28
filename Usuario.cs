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
            bddAccess bddMito = new bddAccess(configuracion.rutaMito, "MitoC.mdb", "MITO.mdw");

            //Verificar usuario
            
            bdd.AbrirBdd();
            if(bdd.BuscarUsuario(nombreUsuario)){
                if(bdd.ComprobarContrasena(nombreUsuario, passwordUsuario)){
                    verificado = true;
                    //Cargando resto de datos del usuairo
                    rolUsuario = bdd.getRolUsuario(nombreUsuario);
                    dniUsuario = bdd.getDniUsuario(nombreUsuario);
                }else{
                    verificado = false;
                }
            }else{
                verificado = false;
            }


        }
    }
}
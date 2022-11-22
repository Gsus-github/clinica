using Microsoft.Data.Sqlite;
namespace ClinicaNS{
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

        
        public void AbrirBdd(){
            try
                {
                    if(sqlite_conexion is not null){
                        sqlite_conexion.Open();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().ToString());
                }
        }

        public void CerrarBdd(){
            if (sqlite_conexion is not null){
                sqlite_conexion.Close();
            }
        }
        
        
        internal bool BuscarUsuario(string nombreUsuario, string passwordUsuario)
        {
            String consulta = "SELECT [nombreUsuario], [password], [salt] FROM [main].[tblUsuarios] WHERE [nombreUsuario]= 'usuario1'";
            SqliteCommand comando = new SqliteCommand(consulta, sqlite_conexion);
            SqliteDataReader lector;

            if (sqlite_conexion is not null){
                comando.Parameters.Add(new SqliteParameter("@nombreUsuario", nombreUsuario));
                comando = sqlite_conexion.CreateCommand();
            }
            
            lector = comando.ExecuteReader();

            GestionPassword contra;

            while (lector.Read()){
                contra = new GestionPassword(passwordUsuario, lector.GetString(2));
                if (contra.contrasenaHasheada == lector.GetString(1)){
                    return true;
                }
            }

            return false;
        }
    }
}
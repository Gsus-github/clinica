using System.Data.OleDb;
using System.Data.Odbc;
using System.Data;

namespace ClinicaNS
{

    public class bddAccess{
        public static OleDbConnection? con;
        //public static OleDbConnection? conOdbc;
        public static OleDbCommand? commando;

        public bddAccess(String ruta, String nombreBdd, String nombreBddSistema){
            String rutaBDD = Path.Combine(ruta, nombreBdd);
            String rutaSistema = Path.Combine(ruta, nombreBddSistema);
            String cadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + rutaBDD + ";Jet OLEDB:System Database=" + rutaSistema + ";User ID=ADMINMITO;Password=FIZDECOTOVELO";
            //String cadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaBDD + ";Jet OLEDB:System Database=" + rutaSistema + ";User ID=ADMINMITO;Password=FIZDECOTOVELO";
            
            con = new OleDbConnection(cadenaConexion);
            abrirAccess();
            cerrarAccess();
        }

        public void abrirAccess(){
            try{
                con.Open();
                Console.WriteLine("Conectada");
            }catch(Exception e){
                Console.WriteLine ("ERROR: " + e.ToString());
            }
            
        }

        public void cerrarAccess(){
            try{
                con.Close();
                Console.WriteLine("Desconectada");
            }catch(Exception e){
                Console.WriteLine("ERROR: " + e.ToString());
            }
            
        }        
    }
}
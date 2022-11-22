using System.Security.Cryptography;

namespace ClinicaNS{
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
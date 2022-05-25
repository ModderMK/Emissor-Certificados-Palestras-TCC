using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCC
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Conexao conexao = new Conexao();
            Usuario usuario = new Usuario();


            //Verifica se existem usu�rios.
            string query = "SELECT COUNT(*) FROM usuarios";
            conexao.Conectar();
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            int quantidadeUsuario = Convert.ToInt32(comandoMySql.ExecuteScalar());
            conexao.Desconectar();

            //se n�o exististirem usu�rios, d� acesso de administrador � quem est� usando e redireciona � tela principal
            if (quantidadeUsuario >= 1)
            {
                Application.Run(new TelaLogin());

            }
            else
            {
                usuario.setTipoUsuario("1");
                Application.Run(new TelaPrincipal(usuario));
            }
        }
    }
}

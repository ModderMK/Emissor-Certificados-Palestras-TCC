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


            //Verifica se existem usuários.
            string query = "SELECT COUNT(*) FROM usuarios";
            conexao.Conectar();
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            int quantidadeUsuario = Convert.ToInt32(comandoMySql.ExecuteScalar());
            conexao.Desconectar();

            //se não exististirem usuários, dá acesso de administrador à quem está usando e redireciona à tela principal
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace TCC
{
    public class Conexao
    {
        public MySqlConnection ObjetoConexao = new MySqlConnection();

        public Conexao()
        {            
            this.ObjetoConexao.ConnectionString = "server=localhost;port = 3306;user Id=root; database = db_evento; password =";
        }

        public void Conectar()
        {
            if (ObjetoConexao.State == ConnectionState.Closed)
            {
                ObjetoConexao.Open();
            }
        }

        public void Desconectar()
        {
            if (ObjetoConexao.State == ConnectionState.Open)
            {
                ObjetoConexao.Close();
            }
        }
    }
}

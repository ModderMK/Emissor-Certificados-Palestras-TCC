using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace TCC
{
    public class Usuario
    {
        Conexao conexao = new Conexao();
        private string Nome;
        private string Senha;
        private string TipoUsuario;


        //Setters
        public void setNome(string nome)
        {
            this.Nome = nome;
        }


        public void setSenha(string senha)
        {
            this.Senha = senha;
        }


        public void setTipoUsuario(string tipo)
        {
            this.TipoUsuario = tipo;
        }


        //Getters
        public string getTipo()
        {
            return this.TipoUsuario;
        }


        //Métodos da classe.
        public string Cadastrar()
        {
            //Se o nome ou senha não estiverem vazios, Cadastra o usuário
            if (!String.IsNullOrWhiteSpace(this.Nome) && !String.IsNullOrWhiteSpace(this.Senha))
            {
                conexao.Conectar();

                string query = "SELECT COUNT(*) FROM usuarios WHERE Nome = '" + this.Nome + "'";
                MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                int qtdeUsuarios = Convert.ToInt32(comandoMySql.ExecuteScalar());

                if (qtdeUsuarios != 0)
                {
                    conexao.Desconectar();
                    return "O nome de usuário digitado já existe!";
                }
                else
                {
                    query = "INSERT INTO usuarios (Nome, Senha, Administrador) VALUES ('" + this.Nome + "', '" + this.Senha + "', '" + this.TipoUsuario + "')";
                    comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                    comandoMySql.ExecuteNonQuery();
                    conexao.Desconectar();
                    return "Usuário Cadastrado com sucesso.";
                }
            }
            return "Senha ou usuário inválidos.";
        }


        public bool ValidarUsuario()
        {
            conexao.Conectar();

            //Verifica se existe usuário com os dados informados
            string query = "SELECT COUNT(*) FROM usuarios WHERE Nome = '" + this.Nome + "' AND Senha = '" + this.Senha + "';";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);

            //quantidade de usuários com o nome e senha iguais aos digitados
            int qtdeUsuarios = Convert.ToInt32(comandoMySql.ExecuteScalar());

            //Se os dados forem válidos e o usuário existir, define o tipo de usuário e retorna true, se inválidos, retorna false.
            if (!String.IsNullOrWhiteSpace(this.Nome) && !String.IsNullOrWhiteSpace(this.Senha) && qtdeUsuarios == 1)
            {
                //Define o tipo de usuário
                query = "SELECT Administrador FROM usuarios WHERE Nome = '" + this.Nome + "' AND Senha = '" + this.Senha + "';";
                comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                this.TipoUsuario = comandoMySql.ExecuteScalar().ToString();

                conexao.Desconectar();
                return true;
            }
            conexao.Desconectar();
            return false;
        }


        public string AlterarDadosUsuario()
        {
            conexao.Conectar();
            string query = "UPDATE usuarios SET Senha = '" + this.Senha + "', Administrador = '" + this.TipoUsuario + "' WHERE Nome = '" + this.Nome + "';";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.ExecuteNonQuery();
            conexao.Desconectar();
            return "Informações do usuário alteradas.";
        }


        public string ExcluirUsuario()
        {
            conexao.Conectar();
            string query = "DELETE FROM usuarios where Nome = '" + this.Nome + "';";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.ExecuteNonQuery();
            conexao.Desconectar();
            return "Usuário excluído";
        }
    }
}

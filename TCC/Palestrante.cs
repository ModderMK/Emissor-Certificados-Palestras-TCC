using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace TCC
{
    class Palestrante
    {
        private String Nome;
        private String E_mail;
        private String Curriculo;
        TextInfo textinfo = new CultureInfo("pt-Br", false).TextInfo;
        Conexao conexao = new Conexao();


        // **** SETTERS ****

        public void setNome(String nome)
        {
            this.Nome = nome;
        }


        public void setCurriculo(String curriculo)
        {
            this.Curriculo = curriculo;
        }


        public void setE_mail(String email)
        {
            this.E_mail = email;
        }



        //Métodos da classe

        public bool ValidarDados()
        {
            if (String.IsNullOrWhiteSpace(this.Nome) || String.IsNullOrWhiteSpace(this.E_mail) || String.IsNullOrWhiteSpace(this.Curriculo))
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        public bool ValidarExclusao(int IdPalestrante)
        {
            string query = "SELECT DATE_FORMAT(Data, '%d/%m/%Y') FROM palestras WHERE IdPalestrante = '" + IdPalestrante + "' ;";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.CommandType = CommandType.Text;
            MySqlDataReader leitor = comandoMySql.ExecuteReader();

            while (leitor.Read())
            {
                if (DateTime.Compare(Convert.ToDateTime(leitor.GetString(0)), DateTime.Today) == 1)
                {
                    leitor.Close();
                    return false;
                }
            }
            leitor.Close();
            return true;
        }


        public DataTable Buscar(string nome)
        {
            DataTable tabelaDeDados = new DataTable();
            string query;
            conexao.Conectar();
            //Define o comando 
            query = "SELECT IdPalestrante, UPPER(Nome), E_mail FROM palestrantes WHERE Nome LIKE '%" + (nome.ToUpper() == "TODOS" || nome == "" ? "" : nome) + "%';";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.CommandType = CommandType.Text;
            MySqlDataAdapter objetoDataAdapter = new(comandoMySql);

            //preenche a tabela com os dados obtidos da consulta e a define como a fonte de dados do DataGridView 
            objetoDataAdapter.Fill(tabelaDeDados);


            conexao.Desconectar();
            return tabelaDeDados;
        }


        public string Cadastrar()
        {

            string query;
            if (ValidarDados())
            {
                conexao.Conectar();

                //Define e executa o comando que insere o novo palestrante no Banco de Dados
                query = "INSERT INTO palestrantes (`Nome`, `E_mail`, `MiniCurriculum`) VALUES ('" + textinfo.ToTitleCase(this.Nome.ToLower()) + "','" +
                    this.E_mail + "','" + this.Curriculo + "');";
                MySqlCommand comando = new MySqlCommand(query, conexao.ObjetoConexao);
                comando.CommandType = CommandType.Text;
                comando.ExecuteNonQuery();

                conexao.Desconectar();
                return "Palestrante cadastrado com sucesso.";
                //Fecha a conexão
            }
            else
            {
                return "Algum dado está faltando, corrija e tente novamente.";
            }
        }


        public string AlterarDados(int IdPalestrante)
        {
            if (ValidarDados())
            {
                conexao.Conectar();
                //Atualiza os dados do palestrante no banco de dados
                string query = "UPDATE palestrantes SET  E_mail = '" + this.E_mail + "', MiniCurriculum = '" + this.Curriculo + "' WHERE IdPalestrante = '" + IdPalestrante + "';";
                MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                comandoMySql.CommandType = CommandType.Text;
                comandoMySql.ExecuteNonQuery();

                conexao.Desconectar();
                return "Dados atualizados";
            }
            else
            {
                return "Algum dado está faltando, corrija e tente novamente.";
            }
        }


        public string Excluir(int IdPalestrante)
        {
            conexao.Conectar();
            if (ValidarExclusao(IdPalestrante))
            {
                //DELETA AS PALESTRAS DO PALESTRANTE
                string query = "DELETE FROM palestras WHERE IdPalestrante = '" + IdPalestrante + "';";
                MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                comandoMySql.ExecuteNonQuery();

                //DELETA O PALESTRANTE
                query = "DELETE FROM palestrantes WHERE IdPalestrante = '" + IdPalestrante + "';";
                comandoMySql.CommandText = query;
                comandoMySql.ExecuteNonQuery();


                conexao.Desconectar();
                return "Palestrante excluído.";
            }
            else
            {
                conexao.Desconectar();
                return "O palestrante ainda tem palestras a serem apresentadas, então não pode ser excluído.";
            }
        }
    }
}

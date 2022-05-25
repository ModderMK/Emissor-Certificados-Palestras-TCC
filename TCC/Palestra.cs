using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace TCC
{
    class Palestra
    {
        private String Nome;
        private String Data;
        private String Tema;
        private String ChaveSecreta;
        Conexao conexao = new Conexao();
        TextInfo textinfo = new CultureInfo("pt-Br", false).TextInfo;


        // **** SETTERS ****
        public void setNome(String nome)
        {
            this.Nome = nome;
        }


        public void setData(String data)
        {
            this.Data = data;
        }


        public void setTema(String tema)
        {
            this.Tema = tema;
        }


        public void setChaveSecreta(String chavesecreta)
        {
            this.ChaveSecreta = chavesecreta;
        }


        //Métodos da classe
        public bool ValidarDados()
        {
            string[] formats = { "dd/MM/yyyy" };

            //Verifica se algum dado não foi informado e se a data está no formato válido
            if (String.IsNullOrWhiteSpace(this.Nome) || String.IsNullOrWhiteSpace(this.Data.Replace("/", "")) || String.IsNullOrWhiteSpace(this.Tema) || !DateTime.TryParseExact(this.Data, formats, new CultureInfo("pt-br"), DateTimeStyles.None, out DateTime datetime))
            {
                return false;
            }
            else
            {
                return true;
            }

        }


        public DataTable Buscar(string nome)
        {
            DataTable tabelaDeDados = new DataTable();
            string query;
            conexao.Conectar();
            //Define o comando 
            query = "SELECT IdPalestra, NomePalestrante, Tema, DATE_FORMAT(Data, '%d/%m/%Y') FROM palestras WHERE NomePalestrante LIKE '%" + (nome.ToUpper() == "TODOS" || nome == "" ? "" : nome) + "%';";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.CommandType = CommandType.Text;
            MySqlDataAdapter objetoDataAdapter = new(comandoMySql);

            //preenche a tabela com os dados obtidos da consulta.
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

                //Pega o ID do Palestrante Para inserir no Banco de dados
                query = "SELECT IdPalestrante FROM palestrantes WHERE Nome = '" + this.Nome + "';";
                MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                comandoMySql.CommandType = CommandType.Text;
                int IdPalestrante = (int)comandoMySql.ExecuteScalar();


                //Se a chave secreta for nula, não aplica a criptografia
                if (!String.IsNullOrWhiteSpace(this.ChaveSecreta))
                {
                    //Insere uma nova palestra no  banco de dados
                    query = "INSERT INTO palestras (IdPalestrante , NomePalestrante, Data, Tema, SHA) VALUES ('" + IdPalestrante + "','" +
                        textinfo.ToTitleCase(this.Nome.ToLower()) + "','" + DateTime.Parse(this.Data).ToString("yyyy/MM/dd") + "', '" + this.Tema + "', SHA1('" + this.ChaveSecreta + "'));";
                }
                else
                {
                    //Insere uma nova palestra no  banco de dados
                    query = "INSERT INTO palestras (IdPalestrante , NomePalestrante, Data, Tema, SHA) VALUES ('" + IdPalestrante + "','" +
                        textinfo.ToTitleCase(this.Nome.ToLower()) + "','" + DateTime.Parse(this.Data).ToString("yyyy/MM/dd") + "', '" + this.Tema + "', '');";
                }


                comandoMySql.CommandText = query;
                comandoMySql.ExecuteNonQuery();


                //Fecha a conexão e retorna o resultado
                conexao.Desconectar();
                return "Palestra cadastrada e certificado disponibilizado para emissão.";
            }
            else
            {
                return "Dados incorretos, corrija e tente novamente.";
            }
        }


        public string AlterarDados(int IdPalestra)
        {
            if (ValidarDados())
            {
                conexao.Conectar();


                //Se a chave secreta for nula, não aplica a criptografia
                if (!String.IsNullOrWhiteSpace(this.ChaveSecreta))
                {

                    //ATUALIZA OS DADOS da palestra
                    string query = "UPDATE palestras SET Data = '" + DateTime.Parse(this.Data).ToString("yyyy/MM/dd") +
                        "', Tema = '" + this.Tema + "', SHA = SHA1('" + this.ChaveSecreta + "') WHERE IdPalestra = '" + IdPalestra + "';";
                    MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                    comandoMySql.CommandType = CommandType.Text;
                    comandoMySql.ExecuteNonQuery();
                }
                else
                {
                    //ATUALIZA OS DADOS DA PALESTRA
                    string query = "UPDATE palestras SET NomePalestrante = '" + this.Nome + "', Data = '" + DateTime.Parse(this.Data).ToString("yyyy/MM/dd") +
                        "', Tema = '" + this.Tema + "', SHA = '' WHERE IdPalestra = '" + IdPalestra + "';";
                    MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                    comandoMySql.CommandType = CommandType.Text;
                    comandoMySql.ExecuteNonQuery();
                }

                conexao.Desconectar();
                return "Dados atualizados";
            }
            else
            {
                return "Dados incorretos, corrija e tente novamente";
            }
        }


        public string Excluir(int IdPalestra)
        {
            conexao.Conectar();
            //DELETA A PALESTRA DO BANCO DE DADOS
            string query = "DELETE FROM palestras WHERE IdPalestra = '" + IdPalestra + "';";
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.ExecuteNonQuery();

            conexao.Desconectar();
            return "Palestra excluída";
        }


        public DataTable ListarCertificados(string nome, string anoDeEmissao)
        {
            string query = "";
            DataTable tabelaDeDados = new DataTable();
            conexao.Conectar();

            //Adiciona à tabela de dados as duas primeiras colunas.
            tabelaDeDados.Columns.Add("", typeof(bool));
            tabelaDeDados.Columns.Add("", typeof(String));

            //Procura no banco de dados os palestrantes com certificado cadastrado que tenham o nome parecido com o informado pelo usuário
            if (anoDeEmissao == "TODOS")
            {
                query = "SELECT SHA, IdPalestra , UPPER(NomePalestrante), DATE_FORMAT(Data, '%d/%m/%Y') FROM palestras WHERE NomePalestrante Like '%" + (nome.ToUpper() == "TODOS" || nome == "" ? "" : nome) + "%';";
            }
            else
            {
                query = "SELECT SHA, IdPalestra , UPPER(NomePalestrante), DATE_FORMAT(Data, '%d/%m/%Y') FROM palestras WHERE NomePalestrante Like '%" + (nome.ToUpper() == "TODOS" || nome == "" ? "" : nome) + "%' AND YEAR(Data) = " + anoDeEmissao + " ;";

            }
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            comandoMySql.CommandType = CommandType.Text;

            //Executa o comando, preenche a tabela com os dados obtidos da consulta e a define como a fonte de dados do DataGridView 
            MySqlDataAdapter objetoDataAdapter = new(comandoMySql);
            objetoDataAdapter.Fill(tabelaDeDados);

            //Desconecta do Banco de dados e retorna a tabela de dados.
            conexao.Desconectar();
            return tabelaDeDados;
        }
    }
}
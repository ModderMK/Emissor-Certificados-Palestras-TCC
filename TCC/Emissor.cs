using System.Globalization;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;
using System.Linq;
using System.IO;
using System.Diagnostics;
namespace TCC
{
    class Emissor
    {
        Conexao conexao = new Conexao();
        string path = "";
        string arquivotxt = "/certificado/Certificados.txt";
        string arquivodocx = "/certificado/EmissãoDeCertificadoDoProjeto.docx";
        string workingDirectory = Environment.CurrentDirectory;


        public string EmitirCertificadosSelecionados(int[] IdsPalestras)
        {
            try
            {
                path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + arquivotxt;
                if (IdsPalestras.Length > 0)
                {
                    conexao.Conectar();

                    //Delete o arquivo para esvazior os registros
                    File.Delete(path);
                    StreamWriter manipuladorDeArquivos = new StreamWriter(path, true);
                    //escreve o cabeçalho do arquivo
                    manipuladorDeArquivos.WriteLine("NOME, TEMA, DATA, SHA");

                    foreach (int IdPalestra in IdsPalestras)
                    {
                        //Resgata os dados do certificado que se deseja emitir
                        string query = "SELECT NomePalestrante, Tema, SHA FROM palestras WHERE IdPalestra = '" + IdPalestra + "';";

                        MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                        comandoMySql.CommandType = CommandType.Text;
                        MySqlDataReader leitor = comandoMySql.ExecuteReader();
                        while (leitor.Read())
                        {
                            //Escreve os dados do certificado
                            manipuladorDeArquivos.WriteLine(leitor.GetString(0) + ", " + leitor.GetString(1) + ", " + DateTime.Now.Date.ToString("dd 'de' MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("pt-br")) + ", " + leitor.GetString(2));
                        }
                        leitor.Close();
                    }

                    manipuladorDeArquivos.Close();
                    conexao.Desconectar();
                    AbrirCertificadosNoWord();
                    return "Certificados emitidos.";
                }
                else
                {
                    return "Nenhum certificado selecionado.";
                }
            }
            catch (Exception)
            {
                return "O arquivo já está aberto, feche-o e tente novamente.";
            }
        }


        public string EmitirCertificadosdoAnoSelecionado(string ano)
        {
            try
            {
                path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + arquivotxt;
                conexao.Conectar();
                //Resgata todos os certificados do ano informado
                string query = "SELECT NomePalestrante, Tema, SHA FROM palestras WHERE YEAR(Data) = '" + ano + "';";

                MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                comandoMySql.CommandType = CommandType.Text;
                MySqlDataReader leitor = comandoMySql.ExecuteReader();

                //Cria (ou reescreve) o arquivo texto, inserindo o cabeçalho de dados.
                File.Delete(path);
                StreamWriter manipuladorDeArquivos = new StreamWriter(path, true);
                manipuladorDeArquivos.WriteLine("NOME, TEMA, DATA, SHA");
                while (leitor.Read())
                {
                    //Escreve os dados no arquivo texto
                    manipuladorDeArquivos.WriteLine(leitor.GetString(0) + ", " + leitor.GetString(1) + ", " + DateTime.Now.Date.ToString("dd 'de' MMMM 'de' yyyy", CultureInfo.CreateSpecificCulture("pt-br")) + ", " + leitor.GetString(2));
                }
                manipuladorDeArquivos.Close();
                leitor.Close();

                conexao.Desconectar();
                AbrirCertificadosNoWord();
                return "Certificados emitidos.";
            }
            catch (Exception)
            {
                return "O arquivo já está aberto, feche-o e tente novamente.";
            }
        }

        public void AbrirCertificadosNoWord()
        {
            path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + arquivodocx;
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}
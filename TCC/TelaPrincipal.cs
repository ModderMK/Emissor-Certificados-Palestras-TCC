using System;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TCC
{
    public partial class TelaPrincipal : Form
    {

        //Importa a dependencia para gerar a borda redonda.
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        // Instanciando as classes usadas no programa
        Emissor emissor = new Emissor();
        Conexao conexao = new Conexao();
        Palestrante palestrante = new Palestrante();
        Palestra palestra = new Palestra();
        Usuario usuario = new Usuario();
        public static TelaPrincipal instanciaDaClasse;
        //Usado para deixar as primeiras letras das palavras de uma frase em maiúsculo
        TextInfo textinfo = new CultureInfo("pt-Br", false).TextInfo;
        int IdSelecionado;

        public TelaPrincipal(Usuario usuario)
        {
            InitializeComponent();
            //Arredonda as bordas do formulário
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));

            this.usuario = usuario;
            instanciaDaClasse = this;
        }

        //Importa os métodos para poder arrastar o formulário.
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void Inicio_Load(object sender, EventArgs e)
        {
            CarregarNomes();
            CarregarAnos();


            //Se o usuário não for o administrador, Não mostrará a aba de// emissão de certificado
            //e o campo de inserção da chave sereta
            if (usuario.getTipo().Equals("0"))
            {
                lblChaveSecreta.Visible = false;
                txtChaveSecreta.Visible = false;
                tbctControleDeAbas.TabPages.Remove(tbctControleDeAbas.TabPages[3]);
                tbctControleDeAbas.TabPages.Remove(tbctControleDeAbas.TabPages[2]);

            }
        }

        // ***** Código da aba de cadastro de Palestrantes *****
        private void btnBuscarPalestrante_Click(object sender, EventArgs e)
        {
            //Busca os dados do palestrante no banco de dados
            dgvListarPalestrantes.DataSource = null;
            dgvListarPalestrantes.DataSource = palestrante.Buscar(txt_NomeBusca.Text);
            dgvListarPalestrantes.RowHeadersVisible = false;
            dgvListarPalestrantes.Columns[0].Visible = false;
            dgvListarPalestrantes.Columns[1].HeaderText = "Nome do Palestrante";
            dgvListarPalestrantes.Columns[1].Width = 500;
            dgvListarPalestrantes.Columns[2].Width = 320;
            dgvListarPalestrantes.Columns[2].HeaderText = "E-mail do Palestrante";
        }


        private void btnCadastrarPalestrante_Click(object sender, EventArgs e)
        {
            //Define os dados do palestrante e cadastra
            palestrante.setNome(textinfo.ToTitleCase(txtNome.Text));
            palestrante.setE_mail(txtEmail.Text);
            palestrante.setCurriculo(txtCurriculum.Text);
            MessageBox.Show(palestrante.Cadastrar());
            //Esvazia os campos após o cadastro do Palestrante            
            btnLimparCamposPalestrante.PerformClick();
            //Atualiza os nomes do combobox da tela de palestras
        }


        private void btnAtualizarDadosPalestrante_Click(object sender, EventArgs e)
        {
            palestrante.setNome(txtNome.Text);
            palestrante.setE_mail(txtEmail.Text);
            palestrante.setCurriculo(txtCurriculum.Text);
            MessageBox.Show(palestrante.AlterarDados(IdSelecionado));

            //Reinicia os campos após a atualização dos dados do palestrante
            btnLimparCamposPalestrante.PerformClick();
        }


        private void btnLimparCamposPalestrante_Click(object sender, EventArgs e)
        {
            //Limpa os campos
            txt_NomeBusca.Text = "TODOS";
            txtNome.Clear();
            txtNome.Enabled = true;
            txtEmail.Clear();
            txtCurriculum.Clear();
            dgvListarPalestrantes.DataSource = null;

            //Reinicia o Estado inicial dos botões
            btnExcluirPalestrante.Enabled = false;
            btnCadastrarPalestrante.Enabled = true;
            btnAtualizarDadosPalestrante.Enabled = false;
        }


        private void btnExcluirPalestrante_Click(object sender, EventArgs e)
        {

            if (DialogResult.Yes == MessageBox.Show("Confirmar exclusão de palestrante?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {

                MessageBox.Show((palestrante.Excluir(IdSelecionado)));
                CarregarNomes();

                //Limpa os campos após a exclusão
                btnLimparCamposPalestrante.PerformClick();
            }
        }


        private void dgvListarPalestrantes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            conexao.Conectar();
            try
            {
                // Resgata o ID do palestrante selecionado
                IdSelecionado = (int)dgvListarPalestrantes.Rows[e.RowIndex].Cells[0].Value;

                //Resgata os dados do palestrante desejado e os habilita para edição
                string query = "SELECT Nome, E_mail, MiniCurriculum FROM palestrantes WHERE IdPalestrante= '" + IdSelecionado + "';";
                MySqlCommand comandoMysql = new MySqlCommand(query, conexao.ObjetoConexao);
                comandoMysql.CommandType = CommandType.Text;

                MySqlDataReader leitor = comandoMysql.ExecuteReader();
                while (leitor.Read())
                {
                    txtNome.Text = leitor.GetString(0);
                    txtEmail.Text = leitor.GetString(1);
                    txtCurriculum.Text = leitor.GetString(2);
                }
                leitor.Close();

                btnAtualizarDadosPalestrante.Enabled = true;
                txtNome.Enabled = false;
                btnCadastrarPalestrante.Enabled = false;
                btnExcluirPalestrante.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERRO:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conexao.Desconectar();
        }




        // ***** Código da aba de cadastro de Palestras *****
        private void btnBuscarPalestra_Click(object sender, EventArgs e)
        {
            //Busca            
            dgvListarPalestras.DataSource = null;
            dgvListarPalestras.DataSource = palestra.Buscar(txt_NomeBuscaPalestra.Text);
            dgvListarPalestras.RowHeadersVisible = false;
            dgvListarPalestras.Columns[0].Visible = false;
            dgvListarPalestras.Columns[1].HeaderText = "Nome do palestrante";
            dgvListarPalestras.Columns[1].Width = 400;
            dgvListarPalestras.Columns[2].HeaderText = "Tema da Palestra";
            dgvListarPalestras.Columns[2].Width = 295;
            dgvListarPalestras.Columns[3].HeaderText = "Data da Palestra";
            dgvListarPalestras.Columns[3].Width = 193;
        }


        private void btn_Cadastrar_Palestra_Click(object sender, EventArgs e)
        {
            palestra.setTema(textinfo.ToTitleCase(txtTema.Text.ToLower()));
            palestra.setNome(cboNomePalestrante.Text);
            palestra.setData(txtData.Text);
            palestra.setChaveSecreta(txtChaveSecreta.Text);
            MessageBox.Show(palestra.Cadastrar());

            //Esvazia os campos após o cadastro do Palestrante            
            btnLimparCamposPalestra.PerformClick();
        }


        private void btnLimparCamposPalestra_Click(object sender, EventArgs e)
        {
            //Esvazia e habilita os campos de dados
            txt_NomeBuscaPalestra.Text = "TODOS";
            cboNomePalestrante.SelectedItem = null;
            txtData.Clear();
            txtTema.Clear();
            txtChaveSecreta.Clear();
            dgvListarPalestras.DataSource = null;
            cboNomePalestrante.Enabled = true;

            //Reinicia o estado inicial dos botões
            btnAtualizarDadosPalestra.Enabled = false;
            btnCadastrarPalestra.Enabled = true;
            btnBuscarPalestra.Enabled = true;
            btnExcluirPalestra.Enabled = false;
        }


        private void btnAtualizarDadosPalestra_Click(object sender, EventArgs e)
        {
            palestra.setTema(textinfo.ToTitleCase(txtTema.Text.ToLower()));
            palestra.setNome(cboNomePalestrante.Text);
            palestra.setData(txtData.Text);
            palestra.setChaveSecreta(txtChaveSecreta.Text);
            MessageBox.Show(palestra.AlterarDados(IdSelecionado));
            //Reinicia os campos após a atualização dos dados do palestrante
            btnLimparCamposPalestra.PerformClick();
        }

        private void dgvListarPalestra_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            conexao.Conectar();
            try
            {
                //identificao o ID da linha selecionada
                IdSelecionado = (int)dgvListarPalestras.Rows[e.RowIndex].Cells[0].Value;

                //Resgata os dados correspondentes e os habilita para a edição
                string query = "SELECT NomePalestrante, Tema, Data, SHA FROM palestras WHERE IdPalestra = '" + IdSelecionado + "';";
                MySqlCommand comandoMysql = new MySqlCommand(query, conexao.ObjetoConexao);
                comandoMysql.CommandType = CommandType.Text;

                MySqlDataReader leitor = comandoMysql.ExecuteReader();
                while (leitor.Read())
                {
                    cboNomePalestrante.Text = leitor.GetString(0);
                    txtTema.Text = leitor.GetString(1);
                    txtData.Text = leitor.GetString(2);
                    txtChaveSecreta.Text = leitor.GetString(3);
                }
                leitor.Close();

                btnAtualizarDadosPalestra.Enabled = true;
                cboNomePalestrante.Enabled = false;
                btnCadastrarPalestra.Enabled = false;
                btnExcluirPalestra.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERRO:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            conexao.Desconectar();
        }


        private void btnExcluirPalestra_Click(object sender, EventArgs e)
        {
            //Confirma se o usuário quer deletar
            if (DialogResult.Yes == MessageBox.Show("Confirmar exclusão da palestra?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                //Exclui a Palestra e o certificado 
                MessageBox.Show(palestra.Excluir(IdSelecionado));

                //Limpa os campos após a exclusão
                btnLimparCamposEmissao.PerformClick();
                btnLimparCamposPalestra.PerformClick();
            }
        }
        // ***** FIM DO CÓDIGO DA ABA DE GERENCIAR PALESTRAS *****



        //*****  Código da aba de emissão de certificado  *****
        private void btnLimparCamposEmissao_Click(object sender, EventArgs e)
        {
            dgvListarCertificadosEmissao.DataSource = null;
            txtNomeBuscaEmissao.Text = "TODOS";
            cboAnoBusca.SelectedIndex = 0;
        }


        private void btnEmitirSelecionados_Click(object sender, EventArgs e)
        {
            int[] IdsPalestras = new int[dgvListarCertificadosEmissao.Rows.Count];
            int x = 0;
            foreach (DataGridViewRow row in dgvListarCertificadosEmissao.Rows)
            {
                //Verifica se todas as palestras selecionadas são válidas
                if (row.Cells[0].Value is true)
                {
                    //Emissão das palestras selecionadas
                    IdsPalestras[x] = (int)row.Cells[3].Value;
                }
                x++;
            }

            MessageBox.Show(emissor.EmitirCertificadosSelecionados(IdsPalestras));
            btnLimparCamposEmissao.PerformClick();
        }


        private void btnEmitirCertificadosDoAnoSelecionado_Click(object sender, EventArgs e)
        {
            MessageBox.Show(emissor.EmitirCertificadosdoAnoSelecionado(cboAnoBusca.Text));
            btnLimparCamposEmissao.PerformClick();
        }


        private void btnBuscasCertificadoEmissao_Click(object sender, EventArgs e)
        {
            dgvListarCertificadosEmissao.DataSource = null;
            dgvListarCertificadosEmissao.DataSource = palestra.ListarCertificados(txtNomeBuscaEmissao.Text, cboAnoBusca.Text);
            dgvListarCertificadosEmissao.RowHeadersVisible = false;

            dgvListarCertificadosEmissao.Columns[0].Width = 20;
            dgvListarCertificadosEmissao.Columns[0].HeaderText = "";
            dgvListarCertificadosEmissao.Columns[1].Width = 20;
            dgvListarCertificadosEmissao.Columns[1].HeaderText = "";
            //Fazer verificação e informa se o código de verificação está informado ou não
            foreach (DataGridViewRow linha in dgvListarCertificadosEmissao.Rows)
            {
                if (!String.IsNullOrWhiteSpace(linha.Cells[2].Value.ToString()))
                {
                    linha.Cells[1].Value = "*";
                }
            }
            dgvListarCertificadosEmissao.Columns[1].ReadOnly = true;
            dgvListarCertificadosEmissao.Columns[2].Visible = false;
            dgvListarCertificadosEmissao.Columns[3].Visible = false;
            dgvListarCertificadosEmissao.Columns[4].ReadOnly = true;
            dgvListarCertificadosEmissao.Columns[4].HeaderText = "Nome do Palestrante";
            dgvListarCertificadosEmissao.Columns[4].Width = 600;
            dgvListarCertificadosEmissao.Columns[5].ReadOnly = true;
            dgvListarCertificadosEmissao.Columns[5].HeaderText = "Data da apresentação";
            dgvListarCertificadosEmissao.Columns[5].Width = 140;
        }
        // *****Fim do Código da aba de emissão*****



        // *****Código da aba de gerenciamento de usuários*****
        private void btnCadastrarUsuario_Click(object sender, EventArgs e)
        {
            if (rdbAdministrador.Checked)
            {
                usuario.setTipoUsuario("1");
            }
            else
            {
                usuario.setTipoUsuario("0");
            }
            usuario.setNome(txtUsuario.Text);
            usuario.setSenha(txtsenha.Text);
            MessageBox.Show(usuario.Cadastrar());
            LimparCamposTelaDeUsuarios();
            CarregarUsuarios();
        }


        public void LimparCamposTelaDeUsuarios()
        {
            txtsenha.Clear();
            txtUsuario.Clear();
            txtSenhaAlterar.Clear();
            txtSenhaAlterar.Enabled = false;
            btnExcluirUsuario.Enabled = false;
            btnAlterarDadosUsuario.Enabled = false;
            rdbAdministrador.Checked = true;
            rdbAdiministradorAlterar.Checked = true;
            rdbAdiministradorAlterar.Enabled = false;
            rdbComumAlterar.Enabled = false;
            cboUsuario.SelectedIndex = 0;
        }


        private void btnAlterarDados_Click(object sender, EventArgs e)
        {
            if (rdbAdiministradorAlterar.Checked)
            {
                usuario.setTipoUsuario("1");
            }
            else
            {
                usuario.setTipoUsuario("0");
            }
            usuario.setNome(cboUsuario.Text);
            usuario.setSenha(txtSenhaAlterar.Text);
            MessageBox.Show(usuario.AlterarDadosUsuario()); ;
            LimparCamposTelaDeUsuarios();
        }


        private void btnExcluirUsuario_Click(object sender, EventArgs e)
        {
            usuario.setNome(cboUsuario.Text);
            usuario.setSenha(txtSenhaAlterar.Text);
            if (rdbAdiministradorAlterar.Checked)
            {
                usuario.setTipoUsuario("1");
            }
            else
            {
                usuario.setTipoUsuario("0");
            }
            MessageBox.Show(usuario.ExcluirUsuario().ToString());
            LimparCamposTelaDeUsuarios();
            CarregarUsuarios();
        }


        private void cboUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUsuario.SelectedIndex != 0)
            {

                btnAlterarDadosUsuario.Enabled = true;
                btnExcluirUsuario.Enabled = true;
                txtSenhaAlterar.Enabled = true;
                rdbAdiministradorAlterar.Enabled = true;
                rdbComumAlterar.Enabled = true;
                //Resgata os dados do usuário selecionado

                conexao.Conectar();
                string query = "SELECT Senha, Administrador FROM usuarios WHERE Nome = '" + cboUsuario.Text + "'";
                MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
                MySqlDataReader leitor = comandoMySql.ExecuteReader();
                while (leitor.Read())
                {
                    txtSenhaAlterar.Text = leitor.GetString(0);
                    if (leitor.GetString(1) == "1")
                    {
                        rdbAdiministradorAlterar.Checked = true;
                    }
                    else
                    {
                        rdbComumAlterar.Checked = true;
                    }
                }
                leitor.Close();
                conexao.Desconectar();

            }
            else
            {
                btnAlterarDadosUsuario.Enabled = false;
                btnExcluirUsuario.Enabled = false;
                txtSenhaAlterar.Enabled = false;
                txtSenhaAlterar.Clear();
                rdbAdiministradorAlterar.Enabled = false;
                rdbComumAlterar.Enabled = false;
            }
        }


        public void CarregarUsuarios()
        {
            string query = "SELECT Nome FROM usuarios;";
            conexao.Conectar();
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            MySqlDataReader leitor = comandoMySql.ExecuteReader();
            //Limpa os itens da combobox antes de inserir 
            cboUsuario.Items.Clear();
            cboUsuario.Items.Add("Nenhum");
            //Preenche os itens da combobox com os anos
            while (leitor.Read())
            {
                cboUsuario.Items.Add(leitor.GetString(0));
            }
            leitor.Close();
            conexao.Desconectar();
            cboAnoBusca.SelectedIndex = 0;
        }

        //*****Fim do Código da aba de gerenciamento de usuários*****




        // *****Código dos botões de  navegação*****
        private void tbctControleDeAbas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Limpas todas as telas cada vez que mudar de tela
            btnLimparCamposPalestra.PerformClick();
            btnLimparCamposPalestrante.PerformClick();
            btnLimparCamposEmissao.PerformClick();
            CarregarUsuarios();
            LimparCamposTelaDeUsuarios();
            txtNomeBuscaEmissao.Text = "TODOS";
            txt_NomeBusca.Text = "TODOS";
            txt_NomeBuscaPalestra.Text = "TODOS";
            CarregarAnos();
            CarregarNomes();
        }

        private void btnIrProInicio1_Click(object sender, EventArgs e)
        {
            tbctControleDeAbas.SelectedIndex = 0;
        }


        private void btnIrProFinal1_Click(object sender, EventArgs e)
        {
            tbctControleDeAbas.SelectedIndex = tbctControleDeAbas.TabCount - 1;
        }


        private void btnProximo_Click(object sender, EventArgs e)
        {
            //Se não for a última aba, avança para a próxima aba
            if (tbctControleDeAbas.SelectedIndex != tbctControleDeAbas.TabCount - 1)
            {
                tbctControleDeAbas.SelectedIndex = tbctControleDeAbas.SelectedIndex + 1;
            }
        }


        private void btnAnterior_Click(object sender, EventArgs e)
        {

            if (tbctControleDeAbas.SelectedIndex != 0)
            {
                tbctControleDeAbas.SelectedIndex = tbctControleDeAbas.SelectedIndex - 1;
            }
        }
        //*****   FIM CÓDIGO BOTÕES   *****



        //Função que carrega os nomes de palestrantes na combobox
        public void CarregarNomes()
        {
            string query = "SELECT Nome FROM palestrantes;";
            conexao.Conectar();
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            MySqlDataReader leitor = comandoMySql.ExecuteReader();
            //Limpa os itens da combobox antes de inserir 
            cboNomePalestrante.Items.Clear();

            //Preenche os itens da combobox com os nomes dos palestrantes existentes
            while (leitor.Read())
            {
                cboNomePalestrante.Items.Add(leitor.GetString(0).ToUpper());
            }
            leitor.Close();
            conexao.Desconectar();
        }


        public void CarregarAnos()
        {
            string query = "SELECT DISTINCT YEAR(Data) FROM palestras;";
            conexao.Conectar();
            MySqlCommand comandoMySql = new MySqlCommand(query, conexao.ObjetoConexao);
            MySqlDataReader leitor = comandoMySql.ExecuteReader();
            //Limpa os itens da combobox antes de inserir 
            cboAnoBusca.Items.Clear();
            cboAnoBusca.Items.Add("TODOS");
            //Preenche os itens da combobox com os anos
            while (leitor.Read())
            {
                cboAnoBusca.Items.Add(leitor.GetString(0));
            }
            leitor.Close();
            conexao.Desconectar();
            cboAnoBusca.SelectedIndex = 0;
        }


        //Deixa os campos de nome e tema em maiúsculo
        private void txtNome_Leave(object sender, EventArgs e)
        {
            txtNome.Text = txtNome.Text.ToUpper();
        }


        private void btn_Fechar_Click(object sender, EventArgs e)
        {
            if (TelaLogin.instanciaDaClasse != null)
            {
                TelaLogin.instanciaDaClasse.Close();
            }
            this.Close();
        }


        private void btn_Minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void cboAnoBusca_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAnoBusca.SelectedIndex == 0)
            {
                btnEmitirCertificadosDoAnoSelecionado.Enabled = false;
            }
            else
            {
                btnEmitirCertificadosDoAnoSelecionado.Enabled = true;
            }
        }


        private void btnDeslogar_Click(object sender, EventArgs e)
        {
            if (TelaLogin.instanciaDaClasse != null)
            {
                TelaLogin.instanciaDaClasse.Show();
            }
            else
            {
                TelaLogin.instanciaDaClasse = new TelaLogin();
                TelaLogin.instanciaDaClasse.Show();
            }
            this.Hide();
        }
    }
}
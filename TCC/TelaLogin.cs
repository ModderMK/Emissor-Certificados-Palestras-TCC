using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCC
{
    public partial class TelaLogin : Form
    {

        public Usuario usuario = new Usuario();
        public static TelaLogin instanciaDaClasse;

        public TelaLogin()
        {
            InitializeComponent();
            //Arredonda as bordas do formulário
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            instanciaDaClasse = this;
        }

        //Importa a referência necessária para arredondar as bordas
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

        //Importa os métodos necessários para que se possa arrastar o formulário.
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);



        //Habilita a movimentação do formulário quando o cabeçalho da tela for arrastado.
        private void pnlCabecalho_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        //Botões da tela
        private void btnLogar_Click(object sender, EventArgs e)
        {
            usuario.setNome(txtUsuario.Text);
            usuario.setSenha(txtsenha.Text);
            if (usuario.ValidarUsuario())
            {
                MessageBox.Show("Bem vindo");
                if (TelaPrincipal.instanciaDaClasse == null)
                {
                    TelaPrincipal inicio = new TelaPrincipal(usuario);
                    inicio.Show();
                }
                else
                {
                    TelaPrincipal.instanciaDaClasse = new TelaPrincipal(usuario);
                    TelaPrincipal.instanciaDaClasse.Show();
                }

                this.Hide();
            }
            else
            {
                MessageBox.Show("Senha ou usuário inválidos.");
            }
            LimparTela();
        }


        //Limpa os campos da tela
        private void LimparTela()
        {
            txtsenha.Clear();
            txtUsuario.Clear();
        }


        //Botões de cabeçalho
        private void btn_Fechar_Click(object sender, EventArgs e)
        {
            if (TelaPrincipal.instanciaDaClasse != null)
            {
                TelaPrincipal.instanciaDaClasse.Close();
            }
            this.Close();
        }


        private void btn_Minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
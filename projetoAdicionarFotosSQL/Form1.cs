using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projetoAdicionarFotosSQL
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        //CONEXÃO BBANCO
        const string string_conexao = @"AQUI ADICIONE SE BANCO LOCAL OU ONLINE";
        //ABRIR CONEXÃO
        SqlConnection conexao = new SqlConnection(string_conexao);


        public object Imageformat { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }
        //BOTAO SELECIONAR IMAGEM DENTRO DO PC PARA SALVAR
        private void buttonSelecionarImagem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string nome = openFileDialog1.FileName;
                bmp = new Bitmap(nome);
                pictureBox1.Image = bmp;
            }
        }
        //BOTAO PARA SALVAR A IMAGEM NO BANCO
        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            MemoryStream memory = new MemoryStream();

            bmp.Save(memory, ImageFormat.Bmp);

            byte[] foto = memory.ToArray();

            const string string_conexao = @"AQUI ADICIONE SE BANCO LOCAL OU ONLINE";

            SqlConnection conexao = new SqlConnection(string_conexao);
            // SUA TABELA E EM NOME, IMAGEM A COLUNA DA ASUA TABELA
            SqlCommand comando = new SqlCommand("INSERT INTO SUA TABELA (nome,imagem) VALUES (@nome, @imagem)", conexao);

            SqlParameter nome = new SqlParameter("@nome", SqlDbType.VarChar);
            SqlParameter imagem = new SqlParameter("@imagem", SqlDbType.Binary);
            nome.Value = textBox1.Text;
            imagem.Value = foto;

            comando.Parameters.Add(nome);
            comando.Parameters.Add(imagem);

            try
            {
                conexao.Open();

                comando.ExecuteNonQuery();

                MessageBox.Show("Imagem Gravada Com Sucesso !");
            }

            catch (Exception E)
            {
                MessageBox.Show(E.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
        //BOTAO PARA  BUSCAR A IMAGEM NO BANCO E APARECER NO pictureBox
        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            SqlCommand comando = new SqlCommand("SELECT * FROM SUA TABELA WHERE nome = @nome", conexao);
            SqlParameter nome = new SqlParameter("@nome", SqlDbType.VarChar);
            nome.Value = textBox1.Text;
            comando.Parameters.Add(nome);

            //try
            {
                conexao.Open();

                SqlDataReader reader = comando.ExecuteReader();

                reader.Read();

                if (reader.HasRows)
                {
                    textBox1.Text = reader[1].ToString();

                    byte[] Foto = (byte[])(reader[2]);
                    if (Foto == null)
                        pictureBox1.Image = null;
                    else
                    {
                        MemoryStream memory = new MemoryStream(Foto);

                        pictureBox1.Image = Image.FromStream(memory);
                    }

                }

                conexao.Close();

            }
            //catch (Exception E)
            {
            //    MessageBox.Show(E.Message);
            }
        }
    }
}

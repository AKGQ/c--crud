using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace CRUD_Notas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-KCHMD84\SQLEXPRESS;Initial Catalog=Notas;Integrated Security=True;");
        public int NotaID;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            GetNotas();
        }

        private void GetNotas()
        {
            SqlCommand cmd = new SqlCommand("Select * from NotasTable", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            dgvNota.DataSource = dt;
        }

        private void btn_crear(object sender, EventArgs e)
        {
            if(isValid()) 
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO NotasTable VALUES (@Titulo, @Descripcion)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Titulo", txtNotaTitulo.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtNotaDescripcion.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Nota agregada", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetNotas();
                ReiniciarForm();
            }
        }

        private bool isValid()
        {
            if(txtNotaTitulo.Text == string.Empty)
            {
                MessageBox.Show("Se requiere el titulo", "Fallido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReiniciarForm();
        }

        private void ReiniciarForm()
        {
            NotaID = 0;
            txtNotaTitulo.Clear();
            txtNotaDescripcion.Clear();
            txtNotaTitulo.Focus();
        }

        private void dgvNota_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            NotaID = Convert.ToInt32(dgvNota.SelectedRows[0].Cells[0].Value);
            txtNotaTitulo.Text = dgvNota.SelectedRows[0].Cells[1].Value.ToString();
            txtNotaDescripcion.Text = dgvNota.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void btn_actualizar(object sender, EventArgs e)
        {
            if (NotaID > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE NotasTable SET Titulo = @Titulo, Descripcion = @Descripcion WHERE NotaID = @NotaID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Titulo", txtNotaTitulo.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtNotaDescripcion.Text);
                cmd.Parameters.AddWithValue("@NotaID", this.NotaID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Nota actualizada", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetNotas();
                ReiniciarForm();
            }
            else
            {
                MessageBox.Show("Selecciona una nota para actualizar", "Seleccionar?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (NotaID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM NotasTable WHERE NotaID = @NotaID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@NotaID", this.NotaID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Nota eliminada", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetNotas();
                ReiniciarForm();
            }
            else
            {
                MessageBox.Show("Selecciona una nota para eliminar", "Seleccionar?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

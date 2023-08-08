using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace Proyecto_IJ
{
    public partial class Principal : Form
    {
        public void Consultarimg()
        {
            SqlConnection conexion = new SqlConnection("Data Source=ISMAELPAULINO;Initial Catalog=Practica_reporte;Integrated Security=True");


            SqlCommand cmdIMG = new SqlCommand();
            DataTable tabla = new DataTable();

            conexion.Open();

            cmdIMG.Connection = conexion;

            cmdIMG.CommandText = "mostrarImagen";
            cmdIMG.CommandType = CommandType.StoredProcedure;
            cmdIMG.Parameters.AddWithValue("@Codigo", Convert.ToInt32(txtcodigo.Text));

            SqlDataAdapter llenar = new SqlDataAdapter(cmdIMG);
            llenar.Fill(tabla);

            Byte[] archivo = (byte[])tabla.Rows[0]["Foto"];

            Stream imagen_xd = new MemoryStream(archivo);

            pictureBox1.Image = Image.FromStream(imagen_xd);

            conexion.Close();


        }
        public Principal()
        {
            InitializeComponent();
        }

        private void bntguardar_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Inventario (Codigo, Producto, Descripcion, Cantidad, Fecha_Entrada, Cantidad_S, Fecha_Salida, Disponibilidad, foto) VALUES (@Codigo, @Producto, @Descripcion, @Cantidad, @Fecha_Entrada, @Cantidad_S, @Fecha_Salida, @Disponibilidad, @foto)";

            SqlConnection conexion = new SqlConnection("Data Source=ISMAELPAULINO;Initial Catalog=Practica_reporte;Integrated Security=True");

            conexion.Open();

            Image pimg = pictureBox1.Image;
            ImageConverter Converter = new ImageConverter();
            var ImageConvert = Converter.ConvertTo(pimg, typeof(byte[]));

            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@Codigo", txtcodigo.Text);
            cmd.Parameters.AddWithValue("@Producto", txtproducto.Text);
            cmd.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text);
            cmd.Parameters.AddWithValue("@Cantidad", txtcantidad.Text);
            cmd.Parameters.AddWithValue("@Fecha_Entrada", txtfecha_e.Text);
            cmd.Parameters.AddWithValue("@Cantidad_S", txtcantidad_s.Text);
            cmd.Parameters.AddWithValue("@Fecha_Salida", txtfecha_s.Text);
            cmd.Parameters.AddWithValue("@Disponibilidad", cbdisponibilidad.Text);
            cmd.Parameters.AddWithValue("@foto", ImageConvert);
            cmd.ExecuteNonQuery();
            Limpiar();
            MessageBox.Show("Guardado correctamente");
        }

        private void btnmodificar_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Inventario SET Producto=@Producto, Descripcion=@Descripcion, Cantidad=@Cantidad, Fecha_Entrada=@Fecha_Entrada, Cantidad_S=@Cantidad_S, Fecha_Salida=@Fecha_Salida, Disponibilidad=@Disponibilidad, foto=@foto WHERE Codigo=@Codigo";

            SqlConnection conexion = new SqlConnection("Data Source=ISMAELPAULINO;Initial Catalog=Practica_reporte;Integrated Security=True");

            conexion.Open();

            Image pimg = pictureBox1.Image;
            ImageConverter Converter = new ImageConverter();
            var ImageConvert = Converter.ConvertTo(pimg, typeof(byte[]));

            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@Codigo", txtcodigo.Text);
            cmd.Parameters.AddWithValue("@Producto", txtproducto.Text);
            cmd.Parameters.AddWithValue("@Descripcion", txtdescripcion.Text);
            cmd.Parameters.AddWithValue("@Cantidad", txtcantidad.Text);
            cmd.Parameters.AddWithValue("@Fecha_Entrada", txtfecha_e.Text);
            cmd.Parameters.AddWithValue("@Cantidad_S", txtcantidad_s.Text);
            cmd.Parameters.AddWithValue("@Fecha_Salida", txtfecha_s.Text);
            cmd.Parameters.AddWithValue("@Disponibilidad", cbdisponibilidad.Text);
            cmd.Parameters.AddWithValue("@foto", ImageConvert);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Se modifico satifactoriamente");
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Inventario WHERE Codigo = @Codigo";

            SqlConnection conexion = new SqlConnection("Data Source=ISMAELPAULINO;Initial Catalog=Practica_reporte;Integrated Security=True");

            conexion.Open();
            SqlCommand cmd = new SqlCommand(query, conexion);
            cmd.Parameters.AddWithValue("@Codigo", txtcodigo.Text);
            cmd.ExecuteNonQuery();
            conexion.Close();
            Limpiar();
            MessageBox.Show("Se elimino satifactoriamente");

        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection("Data Source=ISMAELPAULINO;Initial Catalog=Practica_reporte;Integrated Security=True");
            conexion.Open();

            string busca = "SELECT Codigo, Producto, Descripcion, Cantidad, Fecha_Entrada, Cantidad_S, Fecha_Salida, Disponibilidad FROM Inventario WHERE Codigo = @Codigo";

            SqlCommand Buscaremos = new SqlCommand(busca, conexion);

            Buscaremos.Parameters.AddWithValue("@Codigo", txtcodigo.Text);

            SqlDataReader buscar = Buscaremos.ExecuteReader();

            if (txtcodigo.Text.Length <= 0)
            {
                MessageBox.Show("No puede dejar el campo Vacio");

            }
            else
            {
                if (buscar.Read())
                {
                    txtproducto.Text = buscar["Producto"].ToString();
                    txtdescripcion.Text = buscar["Descripcion"].ToString();
                    txtcantidad.Text = buscar["Cantidad"].ToString();
                    txtfecha_e.Text = buscar["Fecha_Entrada"].ToString();
                    txtcantidad_s.Text = buscar["Cantidad_S"].ToString();
                    txtfecha_s.Text = buscar["Fecha_Salida"].ToString();
                    cbdisponibilidad.Text = buscar["Disponibilidad"].ToString();
                    Consultarimg();
                    MessageBox.Show("Datos Encontrados");

                }
                else
                {
                    MessageBox.Show("No se encuentran los datos");
                    Limpiar();
                }
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            txtcodigo.Clear();
            txtproducto.Clear();
            txtdescripcion.Clear();
            cbdisponibilidad.Text = "";
            txtcantidad_s.Clear();
            txtfecha_s.Clear();
            txtcantidad.Clear();
            txtfecha_e.Clear();
            pictureBox1.Image = null;
        }

        private void limpiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtrne_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            OpenFileDialog Buscar_img = new OpenFileDialog();
            DialogResult Encontrado = Buscar_img.ShowDialog();
            if (Encontrado == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(Buscar_img.FileName);
            }
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Acercade frm = new Acercade();
            frm.Show();
        }

        private void txtedad_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


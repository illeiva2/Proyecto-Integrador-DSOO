﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace proyectoIntegrador
{
    public partial class MostrarCarnet : Form
    {
        public MostrarCarnet()
        {
            InitializeComponent();
        }

        private void MostrarCarnet_Load(object sender, EventArgs e)
        {

        }


        private void txtNroCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Verificar que se haya ingresado un número de cliente
            if (string.IsNullOrWhiteSpace(txtNroCliente.Text))
            {
                MessageBox.Show("Por favor, ingresa el número de DNI del cliente.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lblId_Cliente.Text))
            {
                MessageBox.Show("Por favor, presione el boton buscar.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF File|*.pdf",
                Title = "Guardar carnet como PDF",
                FileName = "Carnet_Socio.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Crear el documento PDF
                Document pdfDoc = new Document(PageSize.A4);
                try
                {

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(saveFileDialog.FileName, FileMode.Create));
                    pdfDoc.Open();

                    // Mostrar Título
                    pdfDoc.Add(new Paragraph("Carnet de Socio"));
                    pdfDoc.Add(new Paragraph(" "));

                    // Mostrar Datos
                    pdfDoc.Add(new Paragraph("Nro de Socio: " + lblId_Cliente.Text));
                    pdfDoc.Add(new Paragraph("Nombre: " + lblNombre.Text));
                    pdfDoc.Add(new Paragraph("Apellido: " + lblApellido.Text));
                    pdfDoc.Add(new Paragraph("DNI: " + lblDNI.Text));
                    pdfDoc.Add(new Paragraph("Fecha de Nacimiento: " + lblFechaNac.Text));

                    MessageBox.Show("Carnet generado exitosamente como PDF.", "PDF Creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el PDF: " + ex.Message);
                }
                finally
                {
                    pdfDoc.Close();
                }
            }

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Limpiar todos los campos de texto
            LimpiarCampos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Home formPrevio = new Home();
            this.Close();
            formPrevio.Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Verificar que se haya ingresado un número de cliente
            if (string.IsNullOrWhiteSpace(txtNroCliente.Text))
            {
                MessageBox.Show("Por favor, ingresa el número de cliente.");
                return;
            }

            string conexionString = "Server=localhost;Database=clubdeportivo;User ID=root;Password=root;";

            // Consulta SQL 
            string query = "SELECT id_cliente, nombre, apellido, dni, fechaNacimiento FROM cliente WHERE dni = @dni";

            using (MySqlConnection conexion = new MySqlConnection(conexionString))
            {
                try
                {
                    // Abrir la conexión a la base de datos
                    conexion.Open();


                    using (MySqlCommand command = new MySqlCommand(query, conexion))
                    {
                        // Buscar por  @dni en la consulta SQL
                        command.Parameters.AddWithValue("@dni", txtNroCliente.Text);


                        MySqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // Asignar los valores obtenidos a los labels
                            lblId_Cliente.Text = reader["id_cliente"].ToString();
                            lblNombre.Text = reader["nombre"].ToString();
                            lblApellido.Text = reader["apellido"].ToString();
                            lblDNI.Text = reader["dni"].ToString();
                            lblFechaNac.Text = Convert.ToDateTime(reader["fechaNacimiento"]).ToShortDateString();
                        }
                        else
                        {
                            MessageBox.Show("Cliente no encontrado.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al acceder a la base de datos: " + ex.Message);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNroCliente.Text = "";
            lblId_Cliente.Text = "";
            lblNombre.Text = "";
            lblApellido.Text = "";
            lblDNI.Text = "";
            lblFechaNac.Text = "";
        }
    }
}

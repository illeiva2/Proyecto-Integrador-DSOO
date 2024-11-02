﻿using MySql.Data.MySqlClient;
using proyectoIntegrador.Datos;
using proyectoIntegrador.Transacciones;
using System.Data;


namespace proyectoIntegrador
{
    internal class Cuota
    {


        public void RegistrarCuota(ECuota cuota)
        {
            using (MySqlConnection sqlCon = Conexion.getInstancia().CrearConexion())
            {
                try
                {
                    sqlCon.Open();

                    using (MySqlCommand command = new MySqlCommand("RegistrarCuota", sqlCon))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("c_id_cliente", cuota.IdCliente);
                        command.Parameters.AddWithValue("c_fecha_pago", cuota.FechaPago);
                        command.Parameters.AddWithValue("c_medio_pago", cuota.MedioPago);
                        command.Parameters.AddWithValue("c_monto", cuota.Monto);
                        command.Parameters.AddWithValue("c_tipo_cuota", cuota.TipoCuota ? 1 : 0);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al registrar cuota: " + ex.Message);
                }
            }
        }
        public DataTable RecuperarDatosCuota(int idPago)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection conexion = Conexion.getInstancia().CrearConexion();

            try
            {
                conexion.Open();
                string query = @"SELECT CONCAT(cl.nombre, ' ', cl.apellido) AS 'Nombre Completo',
                                    c.Monto, c.fecha_Pago, c.medio_pago, c.tipo_cuota, c.id_pago                                    
                                    FROM  cuota c
                                    JOIN cliente cl ON cl.id_cliente = c.id_cliente
                                    WHERE c.id_pago = LAST_INSERT_ID();";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id_pago", idPago); // Asignar el valor del parámetro

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable); // Rellenar el DataTable con los resultados
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se encontraron datos, este catch es de cuotas " + ex.Message);
            }


            return dataTable; // Devolver el DataTable
        }
    }
}

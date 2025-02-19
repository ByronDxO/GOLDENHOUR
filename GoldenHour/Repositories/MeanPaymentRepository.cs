using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GoldenHour.Model;

namespace GoldenHour.Repositories
{
    public class MeanPaymentRepository : RepositoryBase
    {
        /// <summary>
        /// Obtiene la lista de medios de pago desde la base de datos.
        /// </summary>
        /// <returns>Una colección de GT_MeanPayment.</returns>
        public IEnumerable<GT_MeanPayment> GetMeanPayments()
        {
            var payments = new List<GT_MeanPayment>();

            using (SqlConnection conn = GetConnection())
            {
                string query = @"
                    SELECT mep_idMeanPayment, mep_name, mep_status, mep_date
                    FROM GT_MeanPayment";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new GT_MeanPayment
                            {
                                mep_idMeanPayment = Convert.ToInt32(reader["mep_idMeanPayment"]),
                                mep_name = reader["mep_name"].ToString(),
                                mep_status = reader["mep_status"] != DBNull.Value ? (int?)Convert.ToInt32(reader["mep_status"]) : null,
                                mep_date = reader["mep_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["mep_date"]) : null
                            });
                        }
                    }
                    conn.Close();
                }
            }
            return payments;
        }


        public void UpdateMeanPayment(GT_MeanPayment payment)
        {
            using (var conn = GetConnection())
            {
                string sql = @"
            UPDATE GT_MeanPayment 
            SET mep_name = @name,
                mep_status = @status,
                mep_date = @date
            WHERE mep_idMeanPayment = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", payment.mep_name);
                    cmd.Parameters.AddWithValue("@status", payment.mep_status.HasValue ? (object)payment.mep_status.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@date", payment.mep_date.HasValue ? (object)payment.mep_date.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", payment.mep_idMeanPayment);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("No se actualizó el medio de pago. Verifica los datos ingresados.");
                    }
                }
            }
        }

        public void DeleteMeanPayment(int meanPaymentId)
        {
            using (var conn = GetConnection())
            {
                string sql = "DELETE FROM GT_MeanPayment WHERE mep_idMeanPayment = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", meanPaymentId);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("No se eliminó el medio de pago. Verifica que el registro exista.");
                    }
                }
            }
        }


    }
}

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
    }
}

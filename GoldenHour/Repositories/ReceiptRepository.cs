using GoldenHour.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Repositories
{
    public class ReceiptRepository : RepositoryBase
    {
        public int InsertReceipt(GT_Receipt receipt)
        {
            int insertedId = 0;
            using (SqlConnection conn = GetConnection())
            {
                string sql = @"
                    INSERT INTO GT_Receipt 
                        (rec_total, rec_client, rec_mail, rec_date, rec_status, fk_idUser, fk_idPayment, fk_idModifier)
                    OUTPUT INSERTED.rec_idReceipt
                    VALUES 
                        (@total, @client, @mail, @date, @status, @fkUser, @fkPayment, @fkModifier)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@total", receipt.rec_total.HasValue ? (object)receipt.rec_total.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@client", string.IsNullOrEmpty(receipt.rec_client) ? (object)DBNull.Value : receipt.rec_client);
                    cmd.Parameters.AddWithValue("@mail", string.IsNullOrEmpty(receipt.rec_mail) ? (object)DBNull.Value : receipt.rec_mail);
                    cmd.Parameters.AddWithValue("@date", receipt.rec_date.HasValue ? (object)receipt.rec_date.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", receipt.rec_status.HasValue ? (object)receipt.rec_status.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fkUser", receipt.fk_idUser.HasValue ? (object)receipt.fk_idUser.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fkPayment", receipt.fk_idPayment.HasValue ? (object)receipt.fk_idPayment.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fkModifier", receipt.fk_idModifier.HasValue ? (object)receipt.fk_idModifier.Value : DBNull.Value);

                    conn.Open();
                    insertedId = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            return insertedId;
        }


        public IEnumerable<GT_Receipt> GetReceipts()
        {
            var receipts = new List<GT_Receipt>();
            using (SqlConnection conn = GetConnection())
            {
                string sql = @"
                    SELECT rec_idReceipt, rec_total, rec_client, rec_mail, rec_date, rec_status, 
                           fk_idUser, fk_idPayment, fk_idModifier
                    FROM GT_Receipt";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            receipts.Add(new GT_Receipt
                            {
                                rec_idReceipt = Convert.ToInt32(reader["rec_idReceipt"]),
                                rec_total = reader["rec_total"] != DBNull.Value ? Convert.ToDecimal(reader["rec_total"]) : (decimal?)null,
                                rec_client = reader["rec_client"].ToString(),
                                rec_mail = reader["rec_mail"].ToString(),
                                rec_date = reader["rec_date"] != DBNull.Value ? Convert.ToDateTime(reader["rec_date"]) : (DateTime?)null,
                                rec_status = reader["rec_status"] != DBNull.Value ? Convert.ToInt32(reader["rec_status"]) : (int?)null,
                                fk_idUser = reader["fk_idUser"] != DBNull.Value ? Convert.ToInt32(reader["fk_idUser"]) : (int?)null,
                                fk_idPayment = reader["fk_idPayment"] != DBNull.Value ? Convert.ToInt32(reader["fk_idPayment"]) : (int?)null,
                                fk_idModifier = reader["fk_idModifier"] != DBNull.Value ? Convert.ToInt32(reader["fk_idModifier"]) : (int?)null
                            });
                        }
                    }
                    conn.Close();
                }
            }
            return receipts;
        }
    }
}

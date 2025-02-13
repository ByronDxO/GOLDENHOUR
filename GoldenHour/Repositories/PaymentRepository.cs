using GoldenHour.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Repositories
{
    public class PaymentRepository : RepositoryBase
    {
        public int InsertPayment(GT_Payment payment)
        {
            int insertedId = 0;
            using (SqlConnection conn = GetConnection())
            {
                string sql = @"
                    INSERT INTO GT_Payment (pay_transaction, pay_date, fk_idMeanPayment)
                    OUTPUT INSERTED.pay_idPayment
                    VALUES (@transaction, @date, @fkMeanPayment)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@transaction", string.IsNullOrEmpty(payment.pay_transaction) ? (object)DBNull.Value : payment.pay_transaction);
                    cmd.Parameters.AddWithValue("@date", payment.pay_date.HasValue ? (object)payment.pay_date.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@fkMeanPayment", payment.fk_idMeanPayment.HasValue ? (object)payment.fk_idMeanPayment.Value : DBNull.Value);

                    conn.Open();
                    insertedId = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            return insertedId;
        }
    }
}

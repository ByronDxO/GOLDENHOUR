using GoldenHour.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Repositories
{
    public class ModifierRepository : RepositoryBase
    {
        public int InsertModifier(GT_Modifier modifier)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = GetConnection())
            {
                string query = @"
                    INSERT INTO GT_Modifier
                        (mdf_name, mdf_description, mdf_status, mdf_percentage)
                    VALUES
                        (@mdf_name, @mdf_description, @mdf_status, @mdf_percentage)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@mdf_name", modifier.mdf_name);
                    cmd.Parameters.AddWithValue("@mdf_description", string.IsNullOrEmpty(modifier.mdf_description) ? (object)DBNull.Value : modifier.mdf_description);
                    cmd.Parameters.AddWithValue("@mdf_status", modifier.mdf_status.HasValue ? (object)modifier.mdf_status.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@mdf_percentage", modifier.mdf_percentage.HasValue ? (object)modifier.mdf_percentage.Value : DBNull.Value);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return rowsAffected;
        }



        /// <summary>
        /// Obtiene todos los modificadores de la base de datos.
        /// </summary>
        /// <returns>Una colección de objetos GT_Modifier.</returns>
        public IEnumerable<GT_Modifier> GetModifiers()
        {
            var modifiers = new List<GT_Modifier>();

            using (SqlConnection conn = GetConnection())
            {
                string query = @"
                    SELECT mdf_idModifier, mdf_name, mdf_description, mdf_status, mdf_percentage 
                    FROM GT_Modifier";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var mod = new GT_Modifier
                            {
                                mdf_idModifier = Convert.ToInt32(reader["mdf_idModifier"]),
                                mdf_name = reader["mdf_name"].ToString(),
                                mdf_description = reader["mdf_description"] != DBNull.Value ? reader["mdf_description"].ToString() : null,
                                mdf_status = reader["mdf_status"] != DBNull.Value ? (int?)Convert.ToInt32(reader["mdf_status"]) : null,
                                mdf_percentage = reader["mdf_percentage"] != DBNull.Value ? (int?)Convert.ToInt32(reader["mdf_percentage"]) : null
                            };
                            modifiers.Add(mod);
                        }
                    }
                    conn.Close();
                }
            }
            return modifiers;
        }


        public void UpdateModifier(GT_Modifier modifier)
        {
            using (var conn = GetConnection())
            {
                string sql = @"
            UPDATE GT_Modifier 
            SET mdf_name = @name, 
                mdf_description = @description, 
                mdf_percentage = @percentage,
                mdf_status = @status
            WHERE mdf_idModifier = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", modifier.mdf_name);
                    cmd.Parameters.AddWithValue("@description", string.IsNullOrEmpty(modifier.mdf_description) ? (object)DBNull.Value : modifier.mdf_description);
                    cmd.Parameters.AddWithValue("@percentage", modifier.mdf_percentage.HasValue ? (object)modifier.mdf_percentage.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", modifier.mdf_status.HasValue ? (object)modifier.mdf_status.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", modifier.mdf_idModifier);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("No se actualizó el modificador. Verifica los datos ingresados.");
                    }
                }
            }
        }

        public void DeleteModifier(int modifierId)
        {
            using (var conn = GetConnection())
            {
                string sql = "DELETE FROM GT_Modifier WHERE mdf_idModifier = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", modifierId);
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("No se eliminó el modificador. Verifica que el registro exista.");
                    }
                }
            }
        }


    }
}
using GoldenHour.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenHour.Repositories
{
    public class CategoryRepository : RepositoryBase

    {
        /// <summary>
        /// Inserta una nueva categoría en la base de datos.
        /// </summary>
        /// <param name="category">La categoría a insertar.</param>
        /// <returns>Número de filas afectadas (usualmente 1 si se insertó correctamente).</returns>
        public int InsertCategory(GT_Category category)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = GetConnection())
            {
                // La consulta de inserción (no se incluye el ID ya que es autoincremental)
                string query = @"
                    INSERT INTO GT_Category (cat_name, cat_description, cat_status, cat_date)
                    VALUES (@cat_name, @cat_description, @cat_status, @cat_date)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cat_name", category.cat_name);
                    cmd.Parameters.AddWithValue("@cat_description", category.cat_description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@cat_status", category.cat_status.HasValue ? (object)category.cat_status.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@cat_date", category.cat_date.HasValue ? (object)category.cat_date.Value : DBNull.Value);

                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Obtiene todas las categorías de la base de datos.
        /// </summary>
        /// <returns>Una lista de categorías.</returns>
        /// 

        public IEnumerable<GT_Category> GetCategories()
        {
            var categories = new List<GT_Category>();

            using (SqlConnection conn = GetConnection())
            {
                string query = "SELECT cat_idCategory, cat_name, cat_description, cat_status, cat_date FROM GT_Category";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cat = new GT_Category
                            {
                                cat_idCategory = Convert.ToInt32(reader["cat_idCategory"]),
                                cat_name = reader["cat_name"].ToString(),
                                cat_description = reader["cat_description"] != DBNull.Value ? reader["cat_description"].ToString() : null,
                                cat_status = reader["cat_status"] != DBNull.Value ? (int?)Convert.ToInt32(reader["cat_status"]) : null,
                                cat_date = reader["cat_date"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["cat_date"]) : null
                            };

                            categories.Add(cat);
                        }
                    }
                    conn.Close();
                }
            }

            return categories;
        }


    }
}

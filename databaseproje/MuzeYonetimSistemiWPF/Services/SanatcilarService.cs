using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SanatcilarService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<Sanatci> GetAllSanatcilar()
        {
            List<Sanatci> sanatcilar = new List<Sanatci>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Sanatcilar", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sanatcilar.Add(new Sanatci
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        DogumTarihi = Convert.ToDateTime(reader["DogumTarihi"]),
                        OlumTarihi = reader["OlumTarihi"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["OlumTarihi"]),
                        Ulke = reader["Ulke"].ToString(),
                        Biyografi = reader["Biyografi"].ToString()
                    });
                }
            }
            return sanatcilar;
        }

        public void Add(Sanatci sanatci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Sanatcilar (Ad, DogumTarihi, OlumTarihi, Ulke, Biyografi) VALUES (@Ad, @DogumTarihi, @OlumTarihi, @Ulke, @Biyografi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", sanatci.Ad);
                cmd.Parameters.AddWithValue("@DogumTarihi", sanatci.DogumTarihi);
                cmd.Parameters.AddWithValue("@OlumTarihi", (object?)sanatci.OlumTarihi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ulke", sanatci.Ulke);
                cmd.Parameters.AddWithValue("@Biyografi", sanatci.Biyografi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Sanatci sanatci)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sanatcilar SET Ad = @Ad, DogumTarihi = @DogumTarihi, OlumTarihi = @OlumTarihi, Ulke = @Ulke, Biyografi = @Biyografi WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", sanatci.ID);
                cmd.Parameters.AddWithValue("@Ad", sanatci.Ad);
                cmd.Parameters.AddWithValue("@DogumTarihi", sanatci.DogumTarihi);
                cmd.Parameters.AddWithValue("@OlumTarihi", (object?)sanatci.OlumTarihi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ulke", sanatci.Ulke);
                cmd.Parameters.AddWithValue("@Biyografi", sanatci.Biyografi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Sanatcilar WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
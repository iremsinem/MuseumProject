using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SanatAkimiService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<SanatAkimi> GetAllSanatAkimi()
        {
            List<SanatAkimi> akimlar = new List<SanatAkimi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM SanatAkimlari", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    akimlar.Add(new SanatAkimi
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Aciklama = reader["Aciklama"].ToString()
                    });
                }
            }
            return akimlar;
        }

        public void Add(SanatAkimi akim)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO SanatAkimlari (Ad, Aciklama)
                                 VALUES (@Ad, @Aciklama)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", akim.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", akim.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(SanatAkimi akim)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE SanatAkimlari SET 
                                 Ad = @Ad,
                                 Aciklama = @Aciklama
                                 WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", akim.ID);
                cmd.Parameters.AddWithValue("@Ad", akim.Ad);
                cmd.Parameters.AddWithValue("@Aciklama", akim.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM SanatAkimlari WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
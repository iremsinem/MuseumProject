using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class MuzeGideriService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<MuzeGideri> GetAllMuzeGideri()
        {
            List<MuzeGideri> giderler = new List<MuzeGideri>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM MuzeGiderleri", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    giderler.Add(new MuzeGideri
                    {
                        ID = (int)reader["ID"],
                        Aciklama = reader["Aciklama"].ToString(),
                        Tutar = (decimal)reader["Tutar"],
                        Tarih = (DateTime)reader["Tarih"]
                    });
                }
            }
            return giderler;
        }

        public void Add(MuzeGideri gider)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO MuzeGiderleri (Aciklama, Tutar, Tarih) 
                                VALUES (@Aciklama, @Tutar, @Tarih)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Aciklama", gider.Aciklama);
                cmd.Parameters.AddWithValue("@Tutar", gider.Tutar);
                cmd.Parameters.AddWithValue("@Tarih", gider.Tarih);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(MuzeGideri gider)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE MuzeGiderleri SET 
                                Aciklama = @Aciklama,
                                Tutar = @Tutar,
                                Tarih = @Tarih
                                WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", gider.ID);
                cmd.Parameters.AddWithValue("@Aciklama", gider.Aciklama);
                cmd.Parameters.AddWithValue("@Tutar", gider.Tutar);
                cmd.Parameters.AddWithValue("@Tarih", gider.Tarih);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM MuzeGiderleri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
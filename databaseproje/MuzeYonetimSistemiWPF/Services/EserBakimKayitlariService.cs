using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserBakimKaydiService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<EserBakimKaydi> GetAllEserBakimKaydi()
        {
            List<EserBakimKaydi> bakimKayitlari = new List<EserBakimKaydi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EserBakimKayitlari", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bakimKayitlari.Add(new EserBakimKaydi
                    {
                        ID = (int)reader["ID"],
                        EserID = (int)reader["EserID"],
                        BakimTarihi = (DateTime)reader["BakimTarihi"],
                        YapilanIslem = reader["YapilanIslem"].ToString(),
                        SorumluKisi = (int)reader["SorumluKisi"]
                    });
                }
            }
            return bakimKayitlari;
        }

        public void Add(EserBakimKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO EserBakimKayitlari (EserID, BakimTarihi, YapilanIslem, SorumluKisi)
                                 VALUES (@EserID, @BakimTarihi, @YapilanIslem, @SorumluKisi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EserID", kayit.EserID);
                cmd.Parameters.AddWithValue("@BakimTarihi", kayit.BakimTarihi);
                cmd.Parameters.AddWithValue("@YapilanIslem", kayit.YapilanIslem);
                cmd.Parameters.AddWithValue("@SorumluKisi", kayit.SorumluKisi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EserBakimKaydi kayit)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"UPDATE EserBakimKayitlari SET 
                                 EserID = @EserID,
                                 BakimTarihi = @BakimTarihi,
                                 YapilanIslem = @YapilanIslem,
                                 SorumluKisi = @SorumluKisi
                                 WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", kayit.ID);
                cmd.Parameters.AddWithValue("@EserID", kayit.EserID);
                cmd.Parameters.AddWithValue("@BakimTarihi", kayit.BakimTarihi);
                cmd.Parameters.AddWithValue("@YapilanIslem", kayit.YapilanIslem);
                cmd.Parameters.AddWithValue("@SorumluKisi", kayit.SorumluKisi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EserBakimKayitlari WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
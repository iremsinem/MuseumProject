using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class EserSergileriService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<EserSergi> GetAllEserSergi()
        {
            List<EserSergi> eserSergileri = new List<EserSergi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM EserSergileri", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    eserSergileri.Add(new EserSergi
                    {
                        ID = (int)reader["ID"],
                        EserID = (int)reader["EserID"],
                        SergiID = (int)reader["SergiID"],
                        BaslangicTarihi = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        BitisTarihi = Convert.ToDateTime(reader["BitisTarihi"])
                    });
                }
            }
            return eserSergileri;
        }

        public void Add(EserSergi eserSergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO EserSergileri (EserID, SergiID, BaslangicTarihi, BitisTarihi) VALUES (@EserID, @SergiID, @BaslangicTarihi, @BitisTarihi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EserID", eserSergi.EserID);
                cmd.Parameters.AddWithValue("@SergiID", eserSergi.SergiID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", eserSergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", eserSergi.BitisTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(EserSergi eserSergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE EserSergileri SET EserID = @EserID, SergiID = @SergiID, BaslangicTarihi = @BaslangicTarihi, BitisTarihi = @BitisTarihi WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", eserSergi.ID);
                cmd.Parameters.AddWithValue("@EserID", eserSergi.EserID);
                cmd.Parameters.AddWithValue("@SergiID", eserSergi.SergiID);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", eserSergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", eserSergi.BitisTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM EserSergileri WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
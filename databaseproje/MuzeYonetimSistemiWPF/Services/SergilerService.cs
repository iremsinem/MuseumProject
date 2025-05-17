using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SergilerService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<Sergi> GetAllSergiler()
        {
            List<Sergi> sergiler = new List<Sergi>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Sergiler", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sergiler.Add(new Sergi
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Konum = reader["Konum"].ToString(),
                        BaslangicTarihi = Convert.ToDateTime(reader["BaslangicTarihi"]),
                        BitisTarihi = Convert.ToDateTime(reader["BitisTarihi"]),
                        Aciklama = reader["Aciklama"].ToString()
                    });
                }
            }
            return sergiler;
        }

        public void Add(Sergi sergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Sergiler (Ad, Konum, BaslangicTarihi, BitisTarihi, Aciklama) VALUES (@Ad, @Konum, @BaslangicTarihi, @BitisTarihi, @Aciklama)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", sergi.Ad);
                cmd.Parameters.AddWithValue("@Konum", sergi.Konum);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", sergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", sergi.BitisTarihi);
                cmd.Parameters.AddWithValue("@Aciklama", sergi.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Sergi sergi)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Sergiler SET Ad = @Ad, Konum = @Konum, BaslangicTarihi = @BaslangicTarihi, BitisTarihi = @BitisTarihi, Aciklama = @Aciklama WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", sergi.ID);
                cmd.Parameters.AddWithValue("@Ad", sergi.Ad);
                cmd.Parameters.AddWithValue("@Konum", sergi.Konum);
                cmd.Parameters.AddWithValue("@BaslangicTarihi", sergi.BaslangicTarihi);
                cmd.Parameters.AddWithValue("@BitisTarihi", sergi.BitisTarihi);
                cmd.Parameters.AddWithValue("@Aciklama", sergi.Aciklama);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Sergiler WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
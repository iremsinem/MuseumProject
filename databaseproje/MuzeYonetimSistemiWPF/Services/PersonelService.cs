using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class PersonelService
    {
        private string connectionString = ".."; // SQL bağlantı string'i
        public List<Personel> GetAllPersonel()
        {
            List<Personel> personeller = new List<Personel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Personel", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    personeller.Add(new Personel
                    {
                        ID = (int)reader["ID"],
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Gorev = reader["Gorev"].ToString(),
                        Maas = Convert.ToDecimal(reader["Maas"]),
                        IseBaslamaTarihi = Convert.ToDateTime(reader["IseBaslamaTarihi"])
                    });
                }
            }
            return personeller;
        }

        public void Add(Personel personel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Personel (Ad, Soyad, Gorev, Maas, IseBaslamaTarihi) VALUES (@Ad, @Soyad, @Gorev, @Maas, @IseBaslamaTarihi)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Ad", personel.Ad);
                cmd.Parameters.AddWithValue("@Soyad", personel.Soyad);
                cmd.Parameters.AddWithValue("@Gorev", personel.Gorev);
                cmd.Parameters.AddWithValue("@Maas", personel.Maas);
                cmd.Parameters.AddWithValue("@IseBaslamaTarihi", personel.IseBaslamaTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Personel personel)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Personel SET Ad = @Ad, Soyad = @Soyad, Gorev = @Gorev, Maas = @Maas, IseBaslamaTarihi = @IseBaslamaTarihi WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", personel.ID);
                cmd.Parameters.AddWithValue("@Ad", personel.Ad);
                cmd.Parameters.AddWithValue("@Soyad", personel.Soyad);
                cmd.Parameters.AddWithValue("@Gorev", personel.Gorev);
                cmd.Parameters.AddWithValue("@Maas", personel.Maas);
                cmd.Parameters.AddWithValue("@IseBaslamaTarihi", personel.IseBaslamaTarihi);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Personel WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
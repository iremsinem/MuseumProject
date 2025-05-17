using MuzeYonetimSistemiWPF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuzeYonetimSistemiWPF.Services
{
    public class SanatciAkimService
    {
        private string connectionString = ".."; // SQL bağlantı string'i

        public List<SanatciAkim> GetAllSanatciAkim()
        {
            List<SanatciAkim> records = new List<SanatciAkim>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT SanatciID, AkimID FROM SanatciAkim", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var sanatciAkim = new SanatciAkim
                    {
                        SanatciID = (int)reader["SanatciID"],
                        AkimID = (int)reader["AkimID"]
                    };
                    records.Add(sanatciAkim);
                }
            }

            return records;
        }

        public void Add(int sanatciID, int akimID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO SanatciAkim (SanatciID, AkimID)
                                 VALUES (@SanatciID, @AkimID)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SanatciID", sanatciID);
                cmd.Parameters.AddWithValue("@AkimID", akimID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int sanatciID, int akimID)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM SanatciAkim WHERE SanatciID = @SanatciID AND AkimID = @AkimID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SanatciID", sanatciID);
                cmd.Parameters.AddWithValue("@AkimID", akimID);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
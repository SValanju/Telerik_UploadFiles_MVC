using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TelerikFileUpload_MVC.Models;

namespace TelerikFileUpload_MVC.DAL
{
    public class MVC_DAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbName"].ConnectionString);

        public List<Attachments> AttachmentDetails_Read(int vId)
        {
            try
            {
                List<Attachments> list = new List<Attachments>();
                string sql = "select ID, FileName, FileDesc, FilePath from SaveFiles where vId='" + vId + "'";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Attachments attachments = new Attachments
                            {
                                id = Convert.ToInt32(sdr["ID"]),
                                fileName = sdr["FileName"].ToString(),
                                fileDesc = sdr["FileDesc"].ToString(),
                                filePath = sdr["FilePath"].ToString()
                            };
                            list.Add(attachments);
                        }
                    }
                    con.Close();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AttachmentDetails_Insert(Attachments attachments)
        {
            try
            {
                string sql = "insert into SaveFiles(vId, FileName, FileDesc, FilePath) values('" + attachments.vid + "', '" + attachments.fileName + "', '" + attachments.fileDesc + "', '" + attachments.filePath + "')";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                int status = cmd.ExecuteNonQuery();
                con.Close();
                return status;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int AttachmentDetails_Update(Attachments attachments)
        {
            try
            {
                string sql = "update SaveFiles set FileDesc='" + attachments.fileDesc + "' where ID='" + attachments.id + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                int status = cmd.ExecuteNonQuery();
                con.Close();
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AttachmentDetails_Delete(int id)
        {
            try
            {
                string sql = "delete from SaveFiles where ID='" + id + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                int status = cmd.ExecuteNonQuery();
                con.Close();
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Async_Remove(Attachments attachments)
        {
            try
            {
                string sql = "delete from SaveFiles where vId='" + attachments.vid + "' and FileName='" + attachments.fileName + "' and FilePath='" + attachments.filePath + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                int status = cmd.ExecuteNonQuery();
                con.Close();
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
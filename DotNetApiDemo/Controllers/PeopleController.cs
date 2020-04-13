using DotNetApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Data;
using System.Collections;

namespace DotNetApiDemo.Controllers
{
    /// <summary>
    /// This is where I give you all the information about People 
    /// </summary>
    public class PeopleController : ApiController
    {
        List<Person> people = new List<Person>();
        private Hashtable _Parameter = new Hashtable();
        //calling the DB Connection from Web.config
        private string _ConnStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        private SqlDataReader dr;
       
        // GET: api/People
        public List<Person> Get()
        {
            SqlQuery("GetAll", _Parameter);
            return people;
        }


        // GET: api/People/5
        public Person Get(int id)
        {

            _Parameter.Add("@Id", id);
            SqlQuery("GetById", _Parameter);
            return people[0];
        }
        /// <summary>
        /// Get a list of first name for all users
        /// </summary>
        /// <param name = "Id" > The unique identifier for this person. </param>
        /// <param name = "age" > we want to know how old they are. </param>
        /// <returns> A list of first names. </returns>
        [HttpGet]
        [Route("api/People/GetFirstName/{Id:int}/{age:int}")]
        public List<string> GetFirstName(int Id, int age)
        {
            
            _Parameter.Add("@Id", Id);
            _Parameter.Add("@Age", age);
            SqlQuery("GetFirstName", _Parameter);
            List<string> Output = new List<string>();
            foreach (var item in people)
            {
                Output.Add(item.FirstName);
            }
            return Output;
        }



        //POST: api/People
        public void Post(Person person)
        {
            _Parameter.Add("@Id",person.ID);
            _Parameter.Add("@FName", person.FirstName);
            _Parameter.Add("@LName", person.LastName);
            _Parameter.Add("@Address", person.Address);
            _Parameter.Add("@Phone", person.Phone);
            _Parameter.Add("@Age", person.Age);
            SqlQuery("Post", _Parameter);
          
        }

        // DELETE: api/People/5
        public void Delete(int id)
        {
            _Parameter.Add("@Id", id);
           
            SqlQuery("DeletePerson", _Parameter);

        }


        // ******* SQL Calling ******* //
        public void SqlQuery(string _commandText, Hashtable Parameters)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _ConnStr;
            try
            {

                SqlCommand commnd = new SqlCommand();
                commnd.Connection = conn;
                commnd.CommandType = CommandType.StoredProcedure;
                commnd.CommandText = _commandText;
                if (Parameters.Count > 0)
                {
                    foreach (DictionaryEntry param in Parameters)
                        commnd.Parameters.Add((string)param.Key, param.Value);

                }
                if (_commandText.Contains("Get"))
                {


                    conn.Open();
                    dr = commnd.ExecuteReader();
                    while (dr.Read())
                    {
                        people.Add(new Person()
                        {
                            ID = dr.GetInt32(dr.GetOrdinal("ID")),
                            FirstName = dr.GetString(dr.GetOrdinal("FirstName")),
                            LastName = dr.GetString(dr.GetOrdinal("LastName")),
                            Address = dr.GetString(dr.GetOrdinal("Address")),
                            Phone = dr.GetString(dr.GetOrdinal("Phone")),
                        });
                    }

                    conn.Close();
                }
                else if (_commandText == "Post" || _commandText.Contains("Delete"))
                {
                    conn.Open();
                    commnd.ExecuteNonQuery();
                    conn.Close();
                }
                


            }
            catch (SqlException e)
            {
                Console.Write(e.StackTrace);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

        }

    }
}

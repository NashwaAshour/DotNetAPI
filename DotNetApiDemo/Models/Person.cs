using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetApiDemo.Models
{
    /// <summary>
    /// Represents one specific person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// ID from SQL.
        /// </summary>
        public int ID { get; set; } = 0;

        /// <summary>
        /// User's FisrtName from SQL
        /// </summary>
        public string FirstName { get; set; } = "";

        /// <summary>
        /// User's LastName from SQL
        /// </summary>
        public string LastName { get; set; } = "";


        /// <summary>
        /// User's Address from SQL
        /// </summary>
        public string Address { get; set; } = "";


        /// <summary>
        /// User's Phone from SQL
        /// </summary>
        public string Phone { get; set; } = "";


        /// <summary>
        /// User's Phone from SQL
        /// </summary>
        public int Age { get; set; } = 0;
    }
}
using lemco_id_core_api.Interfaces;
using lemco_id_core_api.Models;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace lemco_id_core_api.Data
{
    public class EmployeeMySqlDbContext : IEmployeeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string? _conStr;
        
        public EmployeeMySqlDbContext(IConfiguration configuration) { 
            _configuration = configuration;
            _conStr = _configuration.GetConnectionString("MySQLConnection");
        }

        public Employee GetByID(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_conStr)) {

                string qry = "SELECT * FROM tbl_personalinfo WHERE IDNumber=@id";

                using (MySqlCommand cmd = new MySqlCommand(qry, conn)) {    
                    
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader()) {
                        if (!reader.Read()) return null;

                        Employee emp = new Employee()
                        {
                            systemId = int.Parse(reader["IDNumber"].ToString()),
                            id = reader["EmpID"].ToString(),
                            firstName = reader["Fname"].ToString(),
                            lastName = reader["Lname"].ToString(),
                            middleName = reader["Mname"].ToString(),
                            birthDate = Convert.ToDateTime(reader["Bday"]),
                            address = reader["Address"].ToString(),
                            mID = reader["MID"].ToString(),
                            sssId = reader["SSS"].ToString(),
                            tin = reader["TIN"].ToString(),
                            philhealth = reader["PH"].ToString(),
                            pagibig = reader["Pagibig"].ToString(),
                            contactPerson = reader["ContactPerson"].ToString(),
                            contactAddress = reader["Address"].ToString(), //Update to actual contact update
                            contactNumber = "N/A", //create a separate table for emergency contact details
                        };
                        conn.Close();
                        return emp;
                    }
                }
            }
        }

        public ResponseFormat GeToptEmployees(int size, string searchKey)    
        {
            using (MySqlConnection conn = new MySqlConnection(_conStr))
            {
                string qry =    "SELECT " +
                                " datePrinted, " +
                                "a.IDNumber ,EmpID ,Lname, Fname,Mname,Bday, Address, MID, SSS, TIN, PH,Pagibig, Address , max(datePrinted), " +
                                "c.ContactPerson,c.ContactNumber,c.ContactAddress, " +
                                "concat(a.IDnumber,' - ', Lname,', ' ,Fname ) as label " +
                                "FROM tbl_personalinfo a " +
                                "LEFT JOIN tbl_idprintlogs b ON a.IDNumber = b.IDNumber " +
                                "LEFT JOIN tbl_emergencyinfo c ON a.IDNumber = c.IDNumber " +
                                "GROUP BY a.IDNumber " +
                                "HAVING label like @searchKey " +
                                $"LIMIT {size.ToString()} ";

                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("searchKey", $"%{searchKey}%");
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader == null) return null;

                        ResponseFormat empList = new ResponseFormat();

                        DateTime datePrinted;

                        while (reader.Read()) {
                            Employee emp = new Employee()
                            {
                                systemId    = int.Parse(reader["IDNumber"].ToString()),
                                id          = reader["EmpID"].ToString(),
                                firstName   = reader["Fname"].ToString(),
                                lastName    = reader["Lname"].ToString(),
                                middleName  = reader["Mname"].ToString(),
                                birthDate   = Convert.ToDateTime(reader["Bday"]),
                                address     = reader["Address"].ToString(),
                                mID         = reader["MID"].ToString(),
                                sssId       = reader["SSS"].ToString(),
                                tin         = reader["TIN"].ToString(),
                                philhealth  = reader["PH"].ToString(),
                                pagibig     = reader["Pagibig"].ToString(),
                                contactPerson = reader["ContactPerson"].ToString(),
                                contactAddress = reader["ContactAddress"].ToString(),
                                contactNumber = reader["ContactNumber"].ToString(),
                                imgURL = $"https://localhost:44371/api/Employee/{int.Parse(reader["IDNumber"].ToString())}/image"
                            };

                            if (DateTime.TryParse(reader["datePrinted"].ToString(), out datePrinted)) { 
                            emp.DatePrinted = datePrinted;}

                           empList.data.Add( new ResponseFormatNode() {
                               label = $"{emp.systemId} - {emp.lastName}, {emp.firstName}",
                               value = emp
                           } );
                        }
                        conn.Close();
                        return empList;
                    }
                }
            }
        }

        private bool UpdateContacts(Employee e) {
            
            string sql = "UPDATE tbl_emergencyinfo " +
                            " SET " +
                            "ContactPerson=@ContactPerson, " +
                            "ContactAddress=@ContactAddress, " +
                            "ContactNumber=@ContactNumber " +
                            " WHERE IDNumber=@IDNumber";

            using (MySqlConnection conn = new MySqlConnection(_conStr)) {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                    conn.Open();

                    cmd.Parameters.AddWithValue("ContactPerson", e.contactPerson);
                    cmd.Parameters.AddWithValue("ContactAddress", e.contactAddress);
                    cmd.Parameters.AddWithValue("ContactNumber",e.contactNumber);
                    cmd.Parameters.AddWithValue("IDNumber",e.systemId);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
        }
        private bool checkLoggedContact(Employee e) {

            string sql = "SELECT * FROM tbl_emergencyinfo " +
                         "WHERE IDNumber=@IDNumber";
            using (MySqlConnection conn = new MySqlConnection(_conStr) ) {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                    conn.Open();
                    cmd.Parameters.AddWithValue("IDNumber", e.systemId);

                    using (MySqlDataReader reader = cmd.ExecuteReader()) { 
                        
                        return !reader.Read() ? false : true; 
                    }
                }
            }

        }

        private bool InsertContacts(Employee e)
        {

            string sql = "INSERT INTO tbl_emergencyinfo " +
                            " (IDNumber,ContactPerson,ContactAddress,ContactNumber) " +
                            " VALUES " +
                            "(@IDNumber,@ContactPerson,@ContactAddress,@ContactNumber)";

            using (MySqlConnection conn = new MySqlConnection(_conStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    conn.Open();

                        cmd.Parameters.AddWithValue("ContactPerson", e.contactPerson);
                        cmd.Parameters.AddWithValue("ContactAddress", e.contactAddress);
                        cmd.Parameters.AddWithValue("ContactNumber", e.contactNumber);
                        cmd.Parameters.AddWithValue("IDNumber", e.systemId);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
        }

        public bool UpdateEmployee(Employee e)
        {
            string sql = "UPDATE tbl_personalinfo " +
                         " SET " +
                         "Fname=@Fname, " +
                         "Lname=@Lname, " +
                         "Mname=@Mname, " +
                         "Bday=@Bday, " +
                         "Address=@Address, " +
                         "SSS=@SSS, " +
                         "TIN=@TIN, " +
                         "PH=@PH, " +
                         "Pagibig=@Pagibig, " +
                         "ContactPerson=@ContactPerson " +
                         " Where IDNumber=@IDNumber";

            using (MySqlConnection conn = new MySqlConnection(_conStr)) {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn)) { 
                    conn.Open();

                        cmd.Parameters.AddWithValue("Lname", e.lastName);
                        cmd.Parameters.AddWithValue("Fname",e.firstName);
                        cmd.Parameters.AddWithValue("Mname",e.middleName);
                        cmd.Parameters.AddWithValue("Bday", e.birthDate);
                        cmd.Parameters.AddWithValue("Address",e.address);
                        cmd.Parameters.AddWithValue("SSS",e.sssId);
                        cmd.Parameters.AddWithValue("TIN",e.tin);
                        cmd.Parameters.AddWithValue("PH",e.philhealth);
                        cmd.Parameters.AddWithValue("Pagibig",e.pagibig);
                        cmd.Parameters.AddWithValue("ContactPerson",e.contactPerson);

                        cmd.Parameters.AddWithValue("IDNumber",e.systemId);

                        cmd.ExecuteNonQuery();
                        conn.Close();

                        if (checkLoggedContact(e)) {
                            UpdateContacts(e);
                        }else { 
                            InsertContacts(e);
                        }

                        return true;
                }   
            }
        }
        public bool MarkAsPrinted(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(_conStr)) {
                string sql = "INSERT INTO tbl_idprintlogs " +
                             "(IDNumber,datePrinted)" +
                             "VALUES" +
                             "(@IDNumber,@datePrinted)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn)) {
                    conn.Open();

                    cmd.Parameters.AddWithValue("IDNumber", id);
                    cmd.Parameters.AddWithValue("datePrinted", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthOrganizationController : ControllerBase
    {
        private readonly ILogger<HealthOrganizationController> _logger;

        public HealthOrganizationController(ILogger<HealthOrganizationController> logger)
        {
            _logger = logger;
        }

        private int GetNumberOfWorkers()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            String param = "SELECT * FROM `workers`";//Добавить таблицу не забыть!!!!
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return table.Rows.Count;
        }
        private int GetNumberOfPatients()
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            String param = "SELECT * FROM `patients`";//Добавить таблицу не забыть!!!!
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return table.Rows.Count;
        }

        [HttpGet]
        public HealthOrganization GetInfo()
        {
            return new HealthOrganization
            {
                Name = "Minsk scientific practiacal center of surgery, transplotology and hemotology",
                Info = "3000 hospital beds, 693 doctor, 1475 patients",
                Location = "Minsk, Semashko 8"
            }; 
        }
        [HttpGet("workers")]
        public IEnumerable<Workers>GetWorkers()//
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string param = "SELECT * FROM `workers`";//Параметры для передачи в БД
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            Console.WriteLine("-------------");

            string[] name = new string[GetNumberOfWorkers()];
            int[] id = new int[GetNumberOfWorkers()];
            string[] spec = new string[GetNumberOfWorkers()];
            int[] numb = new int[GetNumberOfWorkers()];
            int[] exp = new int[GetNumberOfWorkers()];
            int i = 0;

            foreach (DataRow row in table.Rows)
            { 
                id[i] = (int)row.ItemArray[0];
                name[i] = Convert.ToString(row.ItemArray[1]);
                spec[i] = Convert.ToString(row.ItemArray[2]);
                numb[i] = (int)row.ItemArray[3];
                exp[i] = (int)row.ItemArray[4];
                i++;
            }

            return Enumerable.Range(0, GetNumberOfWorkers()).Select(index => new Workers
            {

                Name = name[index],
                Id = id[index],
                Specialization = spec[index],
                NumberOfSick = numb[index],
                Expirience = exp[index]
            });
        }
        [HttpGet("workers/{id}")]
        public Workers GetWorkerById(int id)//
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string param = "SELECT * FROM `workers` WHERE `Id` = " + id;//Параметры для передачи в БД
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            string name = "NULL";
            string spec = "NULL";
            int numb = 0;
            int exp = 0;

            foreach (DataRow row in table.Rows)
            {
                name = Convert.ToString(row.ItemArray[1]);
                spec = Convert.ToString(row.ItemArray[2]);
                numb = (int)row.ItemArray[3];
                exp = (int)row.ItemArray[4];
            }
            return new Workers
            {
                Name = name,
                Id = id,
                Specialization = spec,
                NumberOfSick = numb,
                Expirience = exp
            };
        }

        [HttpGet("patients")]
        public IEnumerable<Sick> GetPatients()//
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string param = "SELECT * FROM `patients`";//Параметры для передачи в БД
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            Console.WriteLine("-------------");

            DateTime[] date_adm = new DateTime[GetNumberOfWorkers()];
            string[] name = new string[GetNumberOfWorkers()];
            string[] deagnose  = new string[GetNumberOfWorkers()];
            string[] treatment  = new string[GetNumberOfWorkers()];
            string[] doctor = new string[GetNumberOfWorkers()];
            int i = 0;

            foreach (DataRow row in table.Rows)
            {
                date_adm[i] = (DateTime)row.ItemArray[0];
                name[i] = Convert.ToString(row.ItemArray[1]);
                deagnose[i] = (string)row.ItemArray[2];
                treatment[i] = (string)row.ItemArray[3];
                doctor[i] = (string)row.ItemArray[4];
                i++;
            }

            return Enumerable.Range(0, GetNumberOfPatients()).Select(index => new Sick
            {
                DateOfAdmission = date_adm[index],
                Name = name[index],
                Deagnose = deagnose[index],
                Treatment = treatment[index],
                Doctor = doctor[index]
            }) ;
        }

        [HttpPost("workers")]
        public void SetNewWorker(string name, string spec, int numb, int exp)
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string param = "INSERT INTO `workers` (`Id`, `Name`, `Specialization`, `NumberOfSick`, `Expirience`) VALUES (NULL, @ul, @un, @um, @ub)";//Параметры для передачи в БД
            MySqlCommand command = new MySqlCommand(param, db.getConnection());

            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value= name;
            command.Parameters.Add("@un", MySqlDbType.VarChar).Value = spec;
            command.Parameters.Add("@um", MySqlDbType.VarChar).Value = numb;
            command.Parameters.Add("@ub", MySqlDbType.VarChar).Value = exp;

            adapter.SelectCommand = command;
            adapter.Fill(table);
        }
        [HttpDelete("workers/{id}")]
        public void DelWorker(int id)
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string param = "DELETE FROM `workers` WHERE `workers`.`Id` = @ul";//Параметры для передачи в БД
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = id;
            adapter.SelectCommand = command;
            adapter.Fill(table);
        }
        [HttpPost("patients")]
        public void SetNewPatient(DateTime date, string name, string deagnose, string treatment, string doctor)
        {
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string param = "INSERT INTO `patients` (`date_of_admission`, `name`, `diagnose`, `treatment`, `doctor`) VALUES (@ul, @ol, @el, @ql, @yl);";//Параметры для передачи в БД
            MySqlCommand command = new MySqlCommand(param, db.getConnection());
            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = date;
            command.Parameters.Add("@ol", MySqlDbType.VarChar).Value = name;
            command.Parameters.Add("@el", MySqlDbType.VarChar).Value = deagnose;
            command.Parameters.Add("@ql", MySqlDbType.VarChar).Value = treatment;
            command.Parameters.Add("@yl", MySqlDbType.VarChar).Value = doctor;
            adapter.SelectCommand = command;
            adapter.Fill(table);
        }
    }
}

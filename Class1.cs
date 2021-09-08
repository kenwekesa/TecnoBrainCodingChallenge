using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace EmpDetails
{
    public class Employees
    {
        String emplist;
        List<Emp> Emplist = new List<Emp>();

        public Employees(String emp_list)
        {
           
            this.emplist = emp_list;
           

            var csv_path = Path.Combine(Environment.CurrentDirectory, emp_list);
            var myStreamReader = File.OpenText(csv_path);
            var csvReader = new CsvReader(myStreamReader, CultureInfo.CurrentCulture);

            String value;

            int record_index = 1, CEO_number = 0;
            
            
            while (csvReader.Read())
            {


                String employee_id="";
                String manager_id="";
                String salary="";
                for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
                {
                     
                    int sal;
                    if(i==0)
                    {
                        employee_id = csvReader.GetField(0);
                    }
                    else if(i==1)
                    {
                        manager_id = csvReader.GetField(1);
                        if (manager_id.Equals(""))
                            {
                            CEO_number++;
                            }
                    }
                    else
                    {

                        salary = csvReader.GetField(2);

                        if (!(int.TryParse(salary, out sal)))
                        {
                            Console.WriteLine("Salary for employee number: "+employee_id + " Not correct");
                        }
                    }
                   
                    

                    
                }
                
                Emplist.Add(new Emp { EmployeeID = employee_id, ManagerID = manager_id, Salary = salary });
               

          
                Console.WriteLine("\n");
            }





            //--------------------------------Validating One manager to one  employee-----------------------------------------
            var duplicateKeys = Emplist.GroupBy(p => p.EmployeeID).Select(g => new { g.Key, Count = g.Count() }).Where(x => x.Count > 1).ToList().Select(d => d.Key);
            var duplicatePersons = Emplist.Where(p => duplicateKeys.Contains(p.EmployeeID)).ToList();
           
            if(duplicatePersons.Any())
            {
                Console.WriteLine("One employee cannot have more than one manager");
            }





            //-------------------Validating CEO to be only one-----------------------
            if(CEO_number>1)
            {
                Console.WriteLine("CEO cannot be more than one");
            }



      
            
           

    
       

            if (emp_list.Equals(null))
            {
                Console.WriteLine("The list is empty");

                Console.WriteLine();
            }
        }

        public int TotalSalary(String EmpID)
        {
            int totalsalary = 0;
            
            
            for(int i=0; i<Emplist.Count; i++)
            {
                if(Emplist.ElementAt(i).EmployeeID==EmpID || Emplist.ElementAt(i).ManagerID == EmpID)
                {
                    totalsalary+= Int32.Parse(Emplist.ElementAt(i).Salary);
                }
               
            }
            int mak = totalsalary;
            return totalsalary;
        }

    }

    class Emp
    {

        public string EmployeeID { get; set; }
        public string ManagerID { get; set; }
       public String Salary { get; set; }

    }


    class MainClass
    {
        static void Main()
        {
            Employees em = new Employees("Employees.csv");

            Console.WriteLine(em.TotalSalary("Employee2"));
            Console.Read();
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> employeeList;

        public MockEmployeeRepository()
        {
            employeeList = new List<Employee>();
            employeeList.Add(new Employee { Id = 1, Name = "Ram", Email = "ram123", Department = Dept.HR });
            employeeList.Add(new Employee { Id = 2, Name = "Shaym", Email = "shyam123", Department = Dept.IT });
            employeeList.Add(new Employee { Id = 3, Name = "Sohan", Email = "sohan123", Department = Dept.HR });
        }

        public Employee Add(Employee employee)
        {
            employee.Id = employeeList.Max(e => e.Id) + 1;
            employeeList.Add(employee);
            return employee;
        }

        public Employee Detele(int id)
        {
           Employee _employee =employeeList.FirstOrDefault(e => e.Id == id);
            if(_employee!=null)
            {
                employeeList.Remove(_employee);
            }
            return _employee;
        }

        public Employee GetEmployee(int Id)
        {
            return employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return employeeList;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee _employee = employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (_employee != null)
            {
                _employee.Name = employeeChanges.Name;
                _employee.Email = employeeChanges.Email;
                _employee.Department = employeeChanges.Department;
            }
            return _employee;
        }
    }
}

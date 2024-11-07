using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class Employee
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }
}

public class EmployeeManager
{
    private List<Employee> employees = new List<Employee>();

    public void AddEmployee(Employee employee)
    {
        employees.Add(employee);
    }

    public void RemoveEmployee(string name)
    {
        Employee employee = employees.Find(e => e.Name == name);
        if (employee != null)
        {
            employees.Remove(employee);
        }
    }

    public void GenerateReport()
    {
        // 1. **Single Responsibility Principle (SRP)** Violation: This method handles generating a report.
        // It has nothing to do with managing employees.
        Console.WriteLine("Generating Employee Report...");
        foreach (var employee in employees)
        {
            Console.WriteLine($"Employee: {employee.Name}, Age: {employee.Age}, Department: {employee.Department}");
        }
    }

    public void SaveEmployeeDataToDatabase()
    {
        // 2. **Open/Closed Principle (OCP)** Violation: To change how data is saved, we have to modify this class.
        // We should be able to add new saving mechanisms without changing this class.
        Console.WriteLine("Saving employee data to the database...");
    }

    public void SendEmailToEmployee(Employee employee)
    {
        // 3. **Dependency Inversion Principle (DIP)** Violation: This method is tightly coupled with specific functionality for sending emails.
        Console.WriteLine($"Sending email to {employee.Name}");
    }
}
public interface INotificationService
{
    void SendNotification(Employee employee);
   
}
public class EMailService : INotificationService
{
    public void SendNotification(Employee employee)
    {
        Console.WriteLine($"Sending email to {employee.Name}");
    }
}
public interface ISave
{ 
    void Save(Employee employee);
}
public class SaveDataBase : ISave
{
    public void Save(Employee employee)
    {
        Console.WriteLine("Saving employee data to the database...");
    }
}
public interface IGenerate
{
    void Generate(List<Employee> LstEmployee);
}
public class GenerateReport : IGenerate
{
    public void Generate(List<Employee> LstEmployee)
    {
        Console.WriteLine("Generating Employee Report...");
        foreach (var employee in LstEmployee)
        {
            Console.WriteLine($"Employee: {employee.Name}, Age: {employee.Age}, Department: {employee.Department}");
        }
    }
}
public interface IManagement
{
    void Add(List<Employee> employees,Employee employee);
    void Remove(List<Employee> employee,string employeeName);
}
public class EmployeeManagement : IManagement
{
    public IGenerate _ReportGenerate;
    public ISave _DataBaseSave;
    public INotificationService _NotificationService;
    public EmployeeManagement(IGenerate ReportGenerate,ISave DataBaseSave,INotificationService NotificationService)
    {
        _ReportGenerate = ReportGenerate;
        _DataBaseSave = DataBaseSave;
        _NotificationService = NotificationService;
    }
    public void Add(List<Employee> employees,Employee employee)
    {
        employees.Add(employee);
        _DataBaseSave.Save(employee);
        _ReportGenerate.Generate(employees);
        _NotificationService.SendNotification(employee);
    }

    public void Remove(List<Employee> employee,string employeeName)
    {
        Employee employees = employee.Find(e => e.Name == employeeName);
        if (employees != null)
        {
            employee.Remove(employees);
        }
    }
}


public class Program
{
    public static void Main()
    {
        List<Employee> LstEmployees = new List<Employee>();
        Employee employee = new Employee
        {
           Name = "John Doe",
            Age = 30,
            Department = "HR"
        };
        INotificationService notificationService = new EMailService();
        ISave DBsave = new SaveDataBase();
        IGenerate reportGenerate = new GenerateReport();
        IManagement Employeemanagement = new EmployeeManagement(reportGenerate,DBsave,notificationService);
        Employeemanagement.Add(LstEmployees, employee);



    }
}
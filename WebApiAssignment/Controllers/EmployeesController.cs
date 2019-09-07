using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAssignment.Models;

namespace WebApiAssignment.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmsContext _context;

        public EmployeesController(EmsContext context)
        {
            _context = context;

            if (Startup.ResetEmployeeDatabase == true)
            {
                Startup.ResetEmployeeDatabase = false;
                _context.Employees.RemoveRange(_context.Employees);
                _context.SaveChanges();

                _context.Employees.Add(new Employee
                {
                    Name = "Vighnesh",
                    Age = 21,
                    Salary = 10000000
                });
                _context.Employees.Add(new Employee
                {
                    Name = "Shubham",
                    Age = 30,
                    Salary = 1232123
                });
                _context.Employees.Add(new Employee
                {
                    Name = "Omkar",
                    Age = 32,
                    Salary = 131232
                });
                _context.Employees.Add(new Employee
                {
                    Name = "Bhanu",
                    Age = 45,
                    Salary = 12313200
                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return Created("Employee added to DataBase", employee);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (await _context.Employees.FindAsync(id) is null)
                return BadRequest();

            if (id != employee.Id)
                return BadRequest();

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee is null)
                return BadRequest();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok("Employee removed sucessfully!");
        }
    }
}
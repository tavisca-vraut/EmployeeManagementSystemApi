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

            if (_context.Employees.Count() == 0)
            {
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {
            var employees = await _context.Employees.FindAsync(id);

            if (employees == null)
            {
                return NotFound();
            }

            return employees;
        }
    }
}
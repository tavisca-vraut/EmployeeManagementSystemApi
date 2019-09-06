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
    public class ManagersController : ControllerBase
    {
        private readonly EmsContext _context;

        private async Task<IEnumerable<int>> GetManagerIds()
        {
            return (await _context.Managers.ToListAsync())
                            .Select(m => m.ManagerId)
                            .Distinct();
        }

        public ManagersController(EmsContext context)
        {
            _context = context;

            if (_context.Managers.Count() == 0)
            {
                _context.Managers.Add(new Managers
                {
                    EmployeeId = 4,
                    ManagerId = 1
                });
                _context.Managers.Add(new Managers
                {
                    EmployeeId = 3,
                    ManagerId = 2
                });
                _context.Managers.Add(new Managers
                {
                    EmployeeId = 2,
                    ManagerId = 2
                });
                _context.Managers.Add(new Managers
                {
                    EmployeeId = 1,
                    ManagerId = 1
                });

                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Managers>>> GetManagers()
        {
            var managerIds = await GetManagerIds();

            //return (await _context.Employees.ToListAsync())
            //        .Where(e => managerIds.Contains(e.Id))
            //        .ToList();
            return (await _context.Managers.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetManager(int id)
        {
            var managerIds = await GetManagerIds();

            if (managerIds.Contains(id) == false)
            {
                return NotFound();
            }

            return await _context.Employees.FindAsync(id);
        }

        [HttpGet("{id}/employees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(int id)
        {
            var employeeIds = (await _context.Managers.ToListAsync())
                            .Where(m => m.ManagerId == id)
                            .Select(m => m.EmployeeId)
                            .ToList();

            return (await _context.Employees.ToListAsync())
                    .Where(e => employeeIds.Contains(e.Id))
                    .ToList();
        }
    }
}
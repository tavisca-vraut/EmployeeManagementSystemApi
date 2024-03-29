﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            if (Startup.ResetManagerDatabase == true)
            {
                Startup.ResetManagerDatabase = false;
                _context.Managers.RemoveRange(_context.Managers);
                _context.SaveChanges();

                var vighnesh = _context.Employees.Where(e => e.Name == "Vighnesh").First().Id;
                var omkar = _context.Employees.Where(e => e.Name == "Omkar").First().Id;
                var bhanu = _context.Employees.Where(e => e.Name == "Bhanu").First().Id;
                var shubham = _context.Employees.Where(e => e.Name == "Shubham").First().Id;

                _context.Managers.Add(new Managers
                {
                    EmployeeId = shubham,
                    ManagerId = omkar
                });
                _context.Managers.Add(new Managers
                {
                    EmployeeId = bhanu,
                    ManagerId = omkar
                });
                _context.Managers.Add(new Managers
                {
                    EmployeeId = omkar,
                    ManagerId = vighnesh
                });

                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetManagers()
        {
            var managerIds = await GetManagerIds();

            return (await _context.Employees.ToListAsync())
                    .Where(e => managerIds.Contains(e.Id))
                    .ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddManager([FromBody] Managers manager)
        {
            await _context.Managers.AddAsync(manager);
            await _context.SaveChangesAsync();

            return Created("New manager added to DataBase", manager);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteManager(int id)
        {
            var manager = await _context.Employees.FindAsync(id);

            if (manager is null)
                return BadRequest();

            _context.Employees.Remove(manager);
            await _context.SaveChangesAsync();

            return Ok("Manager removed sucessfully!");
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
﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.DTOs.District;
using Models.Entities;
using Repositories.Interfaces;
using Utils.Enum;
using Utils.Middleware;

namespace Repositories
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly ManagementServiceContext _context;
        private readonly IMapper _mapper;

        public DistrictRepository(ManagementServiceContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DistrictDTO>> GetDistricts()
        {
            var districts = await _context.Districts.ToListAsync();

            if (districts.Count == 0)
            {
                throw new KeyNotFoundException("La lista de distritos está vacía.");
            }

            return _mapper.Map<List<DistrictDTO>>(districts);
        }

        public async Task<DistrictDTO> GetDistrictById(int id)
        {
            var district = await _context.Districts
                                        .Include(d => d.Agent)
                                        .FirstOrDefaultAsync(x => x.DistrictId == id);

            if (district == null)
            {
                throw new KeyNotFoundException("No se encontró el distrito.");
            }

            return _mapper.Map<DistrictDTO>(district);
        }

        public async Task<DistrictAgentDTO> GetDistrictsWithAgent(int districtId)
        {
            var district = await _context.Districts
                                        .Include(d => d.Agent)
                                        .FirstOrDefaultAsync(x => x.DistrictId == districtId);

            if (district == null)
            {
                throw new KeyNotFoundException("No se encontró el distrito.");
            }

            if (district.AgentId == null)
            {
                throw new BadRequestException("El distrito no posee agente/s a cargo.");
            }

            return _mapper.Map<DistrictAgentDTO>(district);
        }

        public async Task<bool> AddAgentToDistrict(int agentId, int districtId)
        {
            var agent = await _context.Users.FirstOrDefaultAsync(x => x.UserId == agentId);

            if (agent == null)
            {
                throw new KeyNotFoundException("No se encontró el usuario.");
            }

            if (agent.RoleId != (int)UserRoleEnum.Agent)
            {
                throw new BadRequestException("El usuario no posee rol de agente.");
            }

            var district = await _context.Districts
                                        .Include(d => d.Agent)
                                        .FirstOrDefaultAsync(x => x.DistrictId == districtId);

            if (district == null)
            {
                throw new KeyNotFoundException("No se encontró el distrito.");
            }

            if (district.AgentId != null)
            {
                throw new BadRequestException("El distrito ya tiene un agente asignado.");
            }

            district.AgentId = agentId;
            _context.Entry(district).Property(x => x.AgentId).IsModified = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAgentFromDistrict(int districtId)
        {
            var district = await _context.Districts
                                        .Include(d => d.Agent)
                                        .FirstOrDefaultAsync(x => x.DistrictId == districtId);

            if (district == null)
            {
                throw new KeyNotFoundException("No se encontró el distrito.");
            }

            if (district.AgentId == null)
            {
                throw new BadRequestException("El distrito no posee un agente asignado.");
            }

            district.AgentId = null;
            _context.Entry(district).Property(x => x.AgentId).IsModified = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddServiceToDistrict(int districtId, int serviceId)
        {
            var district = await _context.Districts
                                         .FirstOrDefaultAsync(d => d.DistrictId == districtId);
            if (district == null)
            {
                throw new KeyNotFoundException("No se encontró el distrito.");
            }

            var service = await _context.Services
                                        .FirstOrDefaultAsync(s => s.ServiceId == serviceId);
            if (service == null)
            {
                throw new KeyNotFoundException("No se encontró el servicio.");
            }

            var existingDistrictXservice = await _context.DistrictXservices
                                                        .FirstOrDefaultAsync(dXs =>
                                                            dXs.DistrictId == districtId
                                                            && dXs.ServiceId == serviceId);
            if (existingDistrictXservice == null)
            { // La relación no existe:
                await PostDistrictXservice(districtId, serviceId);
            }
            else if (existingDistrictXservice.Active == false)
            { // La relación existe pero está inactiva:
                existingDistrictXservice.Active = true;

                _context.Entry(existingDistrictXservice)
                    .Property(x => x.Active).IsModified = true;
                await _context.SaveChangesAsync();
            }
            else
            { // La relación existe y ya está activa:
                throw new BadRequestException("El distrito ya posee este servicio.");
            }

            return true;
        }

        public async Task<DistrictXservice> PostDistrictXservice(int districtId, int serviceId)
        {
            DistrictXservice districtXservice = new();
            districtXservice.DistrictId = districtId;
            districtXservice.ServiceId = serviceId;
            districtXservice.Active = true;

            await _context.DistrictXservices.AddAsync(districtXservice);
            await _context.SaveChangesAsync();

            return districtXservice;
        }
    }
}